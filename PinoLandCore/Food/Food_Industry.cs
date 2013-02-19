using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuqua.CompetativeAnalysis.MarketGame
{
    partial class Food_Industry : Industry
    {
        object _lock = new object();

        #region Setup methods

        public override void AddPopulationSettings(string type, double[] settings)
        {
            int i = 0;

            foreach (Age age in this.Economy.Ages.OrderBy(o => o.DisplayOrder))
            {
                foreach (Wealth wealth in this.Economy.Wealths.OrderBy(o => o.DisplayOrder))
                {
                    Food_Industry_Age_Wealth fiaw = Economy.Food_Industry_Age_Wealth.Single(x => x.Age == age && x.Wealth == wealth);
                    if (fiaw == null)
                    {
                        fiaw = new Food_Industry_Age_Wealth();
                    }

                    switch (type.ToLower())
                    {
                        case "sigma":
                            fiaw.Sigma = (double)settings[i];
                            break;
                        case "sensitivitytype":
                            fiaw.SensitivityType = (double)settings[i];
                            break;
                        case "sensitivitydistance":
                            fiaw.SensitivityDistance = (double)settings[i];
                            break;
                    }
                    i++;
                }
            }

        }

        //public override void AddProfileSettings(Profile p, string type, double[] settings)
        //{
        //    int i = 0;
        //    foreach (Age age in p.Economy.Ages.OrderBy(o => o.DisplayOrder))
        //    {
        //        foreach (Wealth wealth in p.Economy.Wealths.OrderBy(o => o.DisplayOrder))
        //        {
        //            Profile_Age_Wealth paw = p.Profile_Age_Wealth.Single(x => x.Age == age && x.Wealth == wealth);
        //            Food_Profile_Age_Wealth fpaw = paw.Food_Profile_Age_Wealth;
        //            if (fpaw == null)
        //            {
        //                fpaw = new Food_Profile_Age_Wealth();
        //                paw.Food_Profile_Age_Wealth = fpaw;
        //            }

        //            switch (type.ToLower())
        //            {
        //                case "sigma":
        //                    fpaw.Sigma = (double)settings[i];
        //                    break;
        //                case "sensitivitytype":
        //                    fpaw.SensitivityType = (double)settings[i];
        //                    break;
        //                case "sensitivitydistance":
        //                    fpaw.SensitivityDistance = (double)settings[i];
        //                    break;
        //            }
        //            i++;
        //        }
        //    }

        //}

        internal override void InitializeIndustry()
        {
            base.InitializeIndustry();

            foreach (Company company in this.Economy.Companies)
            {
                Food_Industry_Company fic = new Food_Industry_Company();
                fic.Company = company;
                fic.Food_Industry = this;
                fic.EntryCost = Statistics.Instance.NormalSample(this.EntryCostMean, this.EntryCostStdDev);
                fic.CapacityCost = Statistics.Instance.NormalSample(this.CapacityCostMean, this.CapacityCostStdDev);
                fic.MarginalCost = Statistics.Instance.NormalSample(this.MarginalCostMean, this.MarginalCostStdDev);
                fic.MaintenanceCost = Statistics.Instance.NormalSample(this.MaintenanceCostMean, this.MaintenanceCostStdDev);
                fic.InventoryCost = Statistics.Instance.NormalSample(this.InventoryCostMean, this.InventoryCostStdDev);
            }
        }

        public void EnterTeamAction(Company company, Location location, double latitude, double longitude, string type
                        , double newCapacity, double soldCapacity, double production, double price)
        {
            Food_Good_Round action = GenerateRoundAction(company, location, latitude, longitude, type, newCapacity, soldCapacity, production, price);

            // Finally update our known cost from this action
            UpdateTeamCosts(action);
        }

        public Food_Good_Round GenerateRoundAction(Company company, Location location, double latitude, double longitude, string type, double newCapacity, double soldCapacity, double production, double price)
        {
            Food_Industry_Good_Type figType = this.Food_Industry_Good_Type.Single(x => x.Name == type);
            //Company company = this.Economy.Companies.Single(c => c == companyId);

            // First see if the good exists (ie, a restaurant in a location owned by a company.  Don't filter on "type")
            Food_Good fgood = this.Food_Good.SingleOrDefault(x => x.Location == location && x.Company == company);

            if (fgood == null)
            {
                fgood = new Food_Good();
                fgood.Industry = this;
                fgood.Company = company;
                fgood.Location = location;
                fgood.Food_Industry_Good_Type = this.Food_Industry_Good_Type.Single(x => x.Name == type);
                fgood.Identifier = string.Format("{0} : {1} : {2}", location.Identifier, figType.Name, fgood.Company.Name);
                fgood.Latitude = latitude;
                fgood.Longitude = longitude;
            }

            // Then create a company-level record if needed
            Food_Industry_Company fic = this.Food_Industry_Company.Single(x => x.Company == company);
            Food_Industry_Company_Round ficr = fic.Food_Industry_Company_Round.SingleOrDefault(x => x.Round == this.Economy.CurrentRound);

            if (ficr == null)
            {
                ficr = new Food_Industry_Company_Round();
                ficr.Food_Industry_Company = fic;
                ficr.Round = this.Economy.CurrentRound;
            }

            // Next see if a record for the round exists
            Food_Good_Round action = fgood.Food_Good_Round.SingleOrDefault(x => x.Round == this.Economy.CurrentRound);
            if (action == null)
            {
                action = new Food_Good_Round();
                action.Food_Good = fgood;   //economy, good, industry
                action.Food_Industry_Company_Round = ficr;  // economy, industry, company, round
            }
            action.CapacityNew = newCapacity;
            action.CapacitySold = soldCapacity;
            action.Price = price;
            action.Production = production;
            return action;
        }

        #endregion


        #region Processing methods

        internal override void PreProcessRound(Round round)
        {
            //Scan through all "actions" (Food_Good_Round) & create a current record if one doesn't exist
            // This is just a saftey check so we can carry forward capacity, inventory, etc. etc.
            List<Food_Good_Round> previousActions = this.Food_Good.SelectMany(x => x.Food_Good_Round)
                .Where(x => x.Round == round.PreviousRound).ToList();

            foreach (Food_Good_Round prevAction in previousActions)
            {
                //See if a current action exists
                Food_Good_Round currentAction = this.Food_Good.SelectMany(x => x.Food_Good_Round)
                    .SingleOrDefault(x => x.Round == round && x.Food_Good == prevAction.Food_Good);

                if (currentAction == null)
                {
                    currentAction = new Food_Good_Round();
                    currentAction.Food_Good = prevAction.Food_Good; //Economy, Good, industry
                    currentAction.Company = prevAction.Company;
                    currentAction.Price = prevAction.Price;             // make life easier for companies
                    currentAction.Production = prevAction.Production;   // make life easier for companies
                    currentAction.Round = round;
                }

                currentAction.IsRollover = true;    // indicates this restaurant is NOT in its first round
                //Carry-over any inventory
                if (this.CanHoldInventory)
                    currentAction.InventoryStart = prevAction.InventoryEnd;
                else
                    currentAction.InventoryStart = 0;
            }



            //Now scan through all current actions
            IEnumerable<Food_Good_Round> currentActions = this.Food_Good.SelectMany(x => x.Food_Good_Round)
                                                                    .Where(x => x.Round == round);
            foreach (Food_Good_Round currentAction in currentActions)
            {
                //Find the previous action, if any
                Food_Good_Round previousAction = previousActions.SingleOrDefault(x => x.Food_Good == currentAction.Food_Good);

                //Copy over previous "end" values (if any)
                if (previousAction != null)
                    currentAction.CapacityStart = previousAction.CapacityEnd;


                //Cap production if we don't have any capacity to produce with!
                if (currentAction.CapacityStart == 0)
                    currentAction.Production = 0;

                //Cannot sell more capacity than exists at beginning of round
                if (currentAction.CapacitySold > currentAction.CapacityStart)
                {
                    currentAction.CapacitySold = currentAction.CapacityStart;
                }

                UpdateTeamCosts(currentAction);
            }

            //TODO: Implement reputaiton that works a bit faster w/ out DB thrash
            //Finally, execute the Household-based reputation modification.
            //foreach (Company company in this.Economy.Companies)
            //{
            //    Food_Household_Company_Round defaultFHCR = new Food_Household_Company_Round() { Reputation = 1 };

            //    //Get previous round's reputation
            //    //IQueryable<Food_Household_Company_Round> prevRep = company.Food_Household_Company_Round.Where(x => x.Round == round.PreviousRound).AsQueryable();
            //    var newRep = from h in this.Economy.Households
            //                 join prevRep in company.Food_Household_Company_Round.Where(x => x.Round == round.PreviousRound)
            //                 on h.HouseholdId equals prevRep.HouseholdId into PrevRepEmpty
            //                 from rep in PrevRepEmpty.DefaultIfEmpty(defaultFHCR)
            //                 select new Food_Household_Company_Round()
            //                 {
            //                     Household = h,
            //                     Company = company,
            //                     Round = round,
            //                     Reputation = CalculateReputation(h, company, round, rep.Reputation)
            //                 };
            //    long execute = newRep.Count();
            //}
        }

        internal override void PostProcessRound(Round round)
        {
            base.PostProcessRound(round);

            // Loop over each company
            foreach (Company company in this.Economy.Companies)
            {
                // If a team entered, mark that & charge 'em
                Food_Industry_Company fic = this.Food_Industry_Company.Single(x => x.Company == company && x.Food_Industry == this);
                Food_Industry_Company_Round ficr = fic.Food_Industry_Company_Round.SingleOrDefault(x => x.Round == round);

                if (ficr == null)
                {
                    ficr = new Food_Industry_Company_Round();
                    ficr.Food_Industry_Company = fic;
                    ficr.Round = round;
                }

                // Update each "good" to reflect sold/decayed equipment
                foreach (Food_Good_Round action in ficr.Food_Good_Round)
                {
                    action.CapacityEnd = action.CapacityStart - action.CapacitySold;

                    //Decay anything remaining after selling, so teams can't "cheat" and always sell a % of their capacity to avoid decay
                    action.CapacityDecay = action.CapacityEnd * fic.Food_Industry.CapacityDecayRate;
                    action.CapacityEnd -= action.CapacityDecay;
                    action.CapacityEnd += action.CapacityNew;

                    //Mark ending inventory
                    action.InventoryEnd = action.InventoryStart + action.Production - action.ActualSales;
                    action.CostOfInventory = action.InventoryEnd * fic.InventoryCost;
                    action.CostOfMaintenance = (action.CapacityEnd - action.CapacityNew) * fic.MaintenanceCost;

                    // Now create the action for the next round, to carry-over inventory
                }

                var aggs = (from fcr in company.Food_Good_Round
                            where fcr.Round == round
                            where fcr.IndustryId == this.IndustryId
                            group fcr by fcr.Company into g
                            select new
                            {
                                CapX = g.Sum(p => p.CostOfCapacity),
                                MC = g.Sum(p => p.CostOfProduction),
                                Revenue = g.Sum(p => p.ActualSales * p.Price),
                                TotalCapacityNew = g.Sum(p => p.CapacityNew),
                                TotalCapacityEnd = g.Sum(p => p.CapacityEnd),
                                TotalInventoryEnd = g.Sum(p => p.InventoryEnd),
                                TotalCostOfMaintenance = g.Sum(p => p.CostOfMaintenance),
                                TotalCostOfInventory = g.Sum(p => p.CostOfInventory)
                            }).SingleOrDefault();

                // Aggregate per company over all
                ficr.CostOfCapacity = (aggs == null) ? 0 : aggs.CapX;
                ficr.CostOfProduction = (aggs == null) ? 0 : aggs.MC;
                ficr.Revenue = (aggs == null) ? 0 : aggs.Revenue;
                ficr.CostOfMaintenance = (aggs == null) ? 0 : aggs.TotalCostOfMaintenance;
                ficr.CostOfInventory = (aggs == null) ? 0 : aggs.TotalCostOfInventory;

                //Mark entry if it happened, and record the cost
                if (fic.EntranceRoundId == null && ((aggs == null) ? 0 : aggs.TotalCapacityNew) > 0)
                {
                    fic.EntranceRoundId = round.RoundId;
                    ficr.CostOfEntry = fic.EntryCost;
                }
            }

        }

        internal override IEnumerable<Good> GetAvailableGoods(Household h)
        {
            //Find all restaurants within 10 units where their is some quantity 
            // on hand (beginning inventory + production - sales so far)
            //  -- Possible parallelism optimization - don't check qty here 
            //      - check it when we consume later in  the algorithm
            var x = from goods in this.Food_Good
                    where goods.Location.DistanceTo(h.Location) <= 10
                    from rounds in goods.Food_Good_Round
                    where (rounds.InventoryStart + rounds.Production - rounds.ActualSales) > 0
                    select goods;

            return x.AsEnumerable();
        }

        internal override double CalculateConsumerCost(Household h, Good g, Round r)
        {
            //Just the price charged
            Food_Good good = (Food_Good)g;

            return good.Food_Good_Round.Single(x => x.Company == good.Company && x.Round == r).Price;
        }

        internal override double? CalculateConsumerValue(Household h, Good g, Round r)
        {
            // First check if we have any quantity left
            Food_Good good = (Food_Good)g;
            Food_Good_Round action = good.Food_Good_Round.FirstOrDefault(x => x.Round == r);
            if (action == null || action.ActualSales >= action.Production)
                return null;

            Food_Industry_Age_Wealth profile = this.Food_Industry_Age_Wealth.Single(x => x.Age == h.Age && x.Wealth == h.Wealth);

            //Get first term:  theta * rho  (value * reputation)
            double theta = Statistics.Instance.HalfNormalSample(profile.Sigma);
            double reputation = 1;

            //TODO: Implement reputaiton that works a bit faster w/ out DB thrash
            //var rep = h.Food_Household_Company_Round.FirstOrDefault(x => x.Round == r.PreviousRound && x.Company == good.Company);
            //if (rep != null)
            //    reputation = rep.Reputation;


            double distance = h.Location.DistanceTo(good.Location);

            return theta * reputation
                - profile.SensitivityDistance * distance
                - profile.SensitivityType * GetTypeDifference(good.Food_Industry_Good_Type, GetPreferredType(h));
        }

        internal override void RecordSale(Household h, Good g, Round r, double price, double surplus)
        {
            Food_Good good = (Food_Good)g;
            Food_Good_Round sale = good.Food_Good_Round.First(x => x.Round == r);

            //Food_Household_Company_Round 
            sale.ActualSales++;

            //And record the actual sale for the household
            Food_Industry_Household_Company_Round fihcr = new Food_Industry_Household_Company_Round();
            fihcr.Household = h;
            fihcr.Food_Good = good;
            fihcr.CompanyId = good.CompanyId;
            fihcr.Round = r;
            fihcr.QuantityBought = 1;
            fihcr.PriceBought = price;
            fihcr.Surplus = surplus;
        }

        internal override double CalculateExpenses(Company company, Round round)
        {
            Food_Industry_Company_Round ficr = this.Food_Industry_Company.Single(x => x.Company == company).Food_Industry_Company_Round.Single(x => x.Round == round);
            return ficr.CostOfCapacity + ficr.CostOfEntry + ficr.CostOfInventory + ficr.CostOfProduction + ficr.CostOfMaintenance;
        }

        internal override double CalculateRevenue(Company company, Round round)
        {
            return this.Food_Industry_Company.Single(x => x.Company == company).Food_Industry_Company_Round.Single(x => x.Round == round).Revenue;
        }

        #endregion


        #region Helper methods

        private double GetTypeDifference(Food_Industry_Good_Type type1, Food_Industry_Good_Type type2)
        {
            if (type1.TypeId == type2.TypeId)
                return 0;
            else
                return 1;
        }

        private Food_Industry_Good_Type GetPreferredType(Household household)
        {
            var types = this.Food_Industry_Good_Type;
            switch (household.Wealth.Name)
            {
                case "Rich":
                    return types.First(x => x.Name == "Healthy");

                case "Middle":
                    return types.First(x => x.Name == "Ethnic");

                case "Poor":
                    return types.First(x => x.Name == "Junk");

                default:
                    return types.First(x => x.Name == "Junk");
            }
        }

        private double CalculateReputation(Household househould, Company company, Round round, double currentReputation)
        {
            return 1.0;
        }

        private void UpdateTeamCosts(Food_Good_Round currentAction)
        {
            Food_Industry_Company fic = this.Food_Industry_Company.Single(x => x.Company == currentAction.Company
                                                                                    && x.Food_Industry == this);

            //Record cost of capacity
            currentAction.CostOfCapacity = currentAction.CapacityNew * fic.CapacityCost             //"Cost" is positive $$$
                                        - currentAction.CapacitySold * this.CapacityResaleRate;     // revenue from sale is negative $$$

            // c * Q
            currentAction.CostOfProduction = fic.MarginalCost * currentAction.Production;

            // If company is doing excessive production, add that
            if (currentAction.Production > currentAction.CapacityStart)
            {
                // 0.5 * c * (E / K) * ( (Q-K)^2 + Q - k)
                double excess = currentAction.Production - currentAction.CapacityStart; // (Q - k)
                currentAction.CostOfProduction += 0.5 * fic.MarginalCost * (this.Elasticity / currentAction.CapacityStart) * (Math.Pow(excess, 2) + excess);
            }

            // If a team entered, mark that & charge 'em
            Food_Industry_Company_Round ficr = fic.Food_Industry_Company_Round.SingleOrDefault(x => x.Round == currentAction.Round);
            if (ficr == null)
            {
                ficr = new Food_Industry_Company_Round();
                ficr.Food_Industry_Company = fic;
                ficr.Round = currentAction.Round;
                ficr.CostOfEntry = fic.EntryCost;
            }
        }

        public void CalculateActionCosts(Economy economy, Food_Good_Round currentAction, out double capacity, out double production, out double entrance)
        {

            Food_Industry_Company fic = this.Food_Industry_Company.Single(x => x.Company == currentAction.Company
                                                                                    && x.Food_Industry == this);

            //Record cost of capacity
            capacity = currentAction.CapacityNew * fic.CapacityCost             //"Cost" is positive $$$
                    - currentAction.CapacitySold * this.CapacityResaleRate;     // revenue from sale is negative $$$

            // c * Q
            if (currentAction.CapacityStart <= 0)
            {
                currentAction.Production = 0;
                production = 0;
            }
            else
            {
                production = fic.MarginalCost * currentAction.Production;

                // If company is doing excessive production, add that
                if (currentAction.Production > currentAction.CapacityStart)
                {
                    // 0.5 * c * (E / K) * ( (Q-K)^2 + Q - k)
                    double excess = currentAction.Production - currentAction.CapacityStart; // (Q - k)
                    production += 0.5 * fic.MarginalCost * (this.Elasticity / currentAction.CapacityStart) * (Math.Pow(excess, 2) + excess);
                }
            }

            // If a team entered, mark that & charge 'em
            entrance = 0;
            if (fic.EntranceRoundId == null || fic.EntranceRoundId == economy.CurrentRoundId)
            {
                entrance = fic.EntryCost;
            }
        }

        #endregion
    }
}
