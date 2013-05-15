using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace Fuqua.CompetativeAnalysis.MarketGame
{
    partial class Food_Industry : Industry
    {
        object _lock = new object();
        object _lock2 = new object();
        List<Food_Industry_Household_Company_Round> houseActions;

        #region Setup methods

        internal override void InitializeIndustry()
        {
            base.InitializeIndustry();

            foreach (Company company in this.Economy.Companies)
            {
                Food_Industry_Company fic = new Food_Industry_Company();
                fic.Company = company;
                fic.Food_Industry = this;

                foreach (Location location in this.Economy.Locations)
                {
                    foreach (Food_Industry_Location fil in location.Food_Industry_Location
                                                            .Where(x => x.IndustryId == this.IndustryId && x.Location == location))
                    {
                        Food_Industry_Company_Location ficl = new Food_Industry_Company_Location();
                        ficl.Food_Industry_Location = fil;
                        ficl.Food_Industry_Company = fic;

                        ficl.CapacityCost = Statistics.Instance.NormalSample(fil.CapacityCostMean, fil.CapacityCostStdDev);
                        ficl.MarginalCost = Statistics.Instance.NormalSample(fil.MarginalCostMean, fil.MarginalCostStdDev);
                        ficl.InventoryCost = Statistics.Instance.NormalSample(fil.InventoryCostMean, fil.InventoryCostStdDev);
                    }
                }

                Food_Industry_Company_Round ficr = new Food_Industry_Company_Round()
                {
                    Food_Industry_Company = fic,
                    Round = this.Economy.CurrentRound
                };
            }
        }

        #endregion

        #region Processing methods

        internal override void PreProcessRound(Round round)
        {
            //Create the company round header for each company for this round if necessary
            foreach (Company company in this.Economy.Companies)
            {
                Food_Industry_Company fic = this.Food_Industry_Company.Single(x => x.Company == company);
                Food_Industry_Company_Round ficr = fic.Food_Industry_Company_Round.SingleOrDefault(x => x.Round == this.Economy.CurrentRound);

                if (ficr == null)
                {
                    ficr = new Food_Industry_Company_Round();
                    ficr.Food_Industry_Company = fic;
                    ficr.Round = round;
                }
            }

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
                    currentAction = new Food_Good_Round()
                    {
                        Food_Good = prevAction.Food_Good,
                        Company = prevAction.Company,
                        Price = prevAction.Price,             // make life easier for companies by copying over the price & production qtys
                        Production = prevAction.Production,
                        Round = round
                    };
                }

                //Carry-over any inventory
                currentAction.InventoryStart = prevAction.InventoryEnd;
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

            // setup a list of recorded house actions
            houseActions = new List<Food_Industry_Household_Company_Round>();
            int dummy = this.Food_Industry_Age_Wealth_Type.Count();
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
                    action.CapacityDecay = action.CapacityEnd * action.Food_Good.Food_Industry_Location.CapacityDecayRate;
                    action.CapacityEnd -= action.CapacityDecay;
                    action.CapacityEnd += action.CapacityNew;

                    //Mark ending inventory
                    action.InventoryEnd = action.InventoryStart + action.Production - action.ActualSales;

                    //If there is no capacity, count the store as closed and force-remove all inventory
                    if (action.CapacityEnd == 0)
                        action.InventoryEnd = 0;

                    action.CostOfInventory = action.InventoryEnd * action.Food_Good.Food_Industry_Company_Location.InventoryCost;
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
                    fic.EntranceRoundId = round.RoundId;
            }

        }

        internal override IEnumerable<Good> GetAvailableGoods(Household household)
        {
            //Find all restaurants within the household's max travel distance 
            //  and where there is some quantity on hand (beginning inventory + production - sales so far)

            var x = from goods in this.Food_Good
                    where goods.Distances[household.LocationId.Value] <= 5
                    //from rounds in goods.Food_Good_Round
                    //where (rounds.InventoryStart + rounds.Production - rounds.ActualSales) > 0
                    select goods;

            return x.AsEnumerable<Good>();
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
            if (action == null || action.ActualSales >= action.Production + action.InventoryStart)
                return null;

            Food_Industry_Age_Wealth_Type profile;
            lock (_lock2)
                profile = this.Food_Industry_Age_Wealth_Type.Single(x =>
                   x.Age == h.Age && x.Wealth == h.Wealth && x.Food_Industry_Good_Type == good.Food_Industry_Good_Type);

            //if (profile == null)
            //    return null;

            if (profile.MaxDistance > h.Location.DistanceTo(good.Location))
                return null;

            // Sample the household's value for the restaurant
            double theta = Statistics.Instance.HalfNormalSample(profile.Sigma);

            double distance = h.Location.DistanceTo(good.Location);
            double value = theta - distance * profile.TransportationCost;

            if (value < 0) return null;
            return value;
        }

        //internal override void RecordSale(Household h, Good g, Round r, double price, double surplus)
        //{
        //    Food_Good good = (Food_Good)g;
        //    Food_Good_Round sale = good.Food_Good_Round.First(x => x.Round == r);

        //    //Food_Household_Company_Round 
        //    sale.ActualSales++;

        //    //And record the actual sale for the household
        //    //Food_Industry_Household_Company_Round fihcr = new Food_Industry_Household_Company_Round();
        //    //fihcr.Household = h;
        //    //fihcr.Food_Good = good;
        //    //fihcr.CompanyId = good.CompanyId;
        //    //fihcr.Round = r;
        //    //fihcr.QuantityBought = 1;
        //    //fihcr.PriceBought = price;
        //    //fihcr.Surplus = surplus;
        //}

        internal override Good RecordSale(Household h, Round r, IEnumerable<GoodSelector> goods)
        {
            GoodSelector winner = null;
            List<GoodSelector> sortedGoods = goods.OrderByDescending(g => g.surplus).ToList();
            foreach (GoodSelector option in sortedGoods)
            {
                Food_Good good = (Food_Good)option.good;
                Food_Good_Round sale = good.Food_Good_Round.First(x => x.Round == r);
                lock (_lock)
                {
                    if (sale.ActualSales < sale.InventoryStart + sale.Production)
                    {
                        sale.ActualSales++;
                        winner = option;
                    }
                }
            }

            if (winner == null) return null;
            ////And record the actual sale for the household
            //Food_Industry_Household_Company_Round fihcr = new Food_Industry_Household_Company_Round();
            //fihcr.Household = h;
            //fihcr.Food_Good = (Food_Good)returnValue;
            //fihcr.CompanyId = ((Food_Good)returnValue).CompanyId;
            //fihcr.Round = r;
            //fihcr.QuantityBought = 1;
            //fihcr.PriceBought = price;
            //fihcr.Surplus = surplus;
            return winner.good;
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

        private double CalculateReputation(Household househould, Company company, Round round, double currentReputation)
        {
            return 1.0;
        }

        private void UpdateTeamCosts(Food_Good_Round currentAction)
        {

            //Record cost of capacity
            currentAction.CostOfCapacity = currentAction.CapacityNew * currentAction.Food_Good.Food_Industry_Company_Location.CapacityCost             //"Cost" is positive $$$
                                        - currentAction.CapacitySold * currentAction.Food_Good.Food_Industry_Location.CapacityResaleRate;     // revenue from sale is negative $$$

            UpdateActionCost(currentAction);
        }

        private static double UpdateActionCost(Food_Good_Round currentAction)
        {
            double c = currentAction.Food_Good.Food_Industry_Company_Location.MarginalCost;
            double Q = currentAction.Production;
            double E = currentAction.Food_Good.Food_Industry_Location.Elasticity;
            double K = currentAction.CapacityStart; //Productive capacity

            if (currentAction.Production <= 0)                                      // Invalid - can't do negative production
                currentAction.CostOfProduction = 0;
            else if (currentAction.Production <= currentAction.CapacityStart)       // if Q <= K
                currentAction.CostOfProduction = c * Q;
            else                                                                    // Company is doing excessive production
                currentAction.CostOfProduction = c * (Q + E * Math.Pow(Q - K, 2.0) / (2.0 * K));
            return currentAction.CostOfProduction;
        }

        #endregion
    }
}
