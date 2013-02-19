using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Fuqua.CompetativeAnalysis.MarketGame
{
    partial class Economy
    {
        private static object _lock = new object();
        private List<Profile_Age_Wealth> profileBins = null;
        internal InitializeCallbacks _callbacks;

        public struct GeoPosition
        {
            public float Latitude;
            public float Longitude;
        }

        public class InitializeCallbacks
        {
            public Func<Household, Dictionary<Color, Profile>, GeoPosition> PositionHousehold;
        }


        public static Economy CreateEconomy(string name)
        {
            Economy economy = new Economy();
            economy.Name = name;
            economy.DateCreated = DateTime.Now;
            return economy;
        }


        /// <summary>
        /// Create a new population profile that can be assigned to a location
        /// </summary>
        /// <param name="economy"></param>
        /// <param name="weights">An array of weights for each age and wealth.  
        /// The weights are ordered by the Age first, then each Wealth for that Age.  All sequences are in alphabetical order.  </param>
        public Profile CreateProfile(string name, int totalPopulation, double[] weights)
        {
            Profile profile = new Profile();
            profile.Name = name;
            profile.Economy = this;
            profile.TotalPopulation = totalPopulation;

            if (weights.Length != this.Ages.Count * this.Wealths.Count)
                throw new ArgumentOutOfRangeException("weights", string.Format("\"weights\" array must have {0} elements; it has {1}.", this.Ages.Count * this.Wealths.Count, weights.Length));

            int i = 0;
            foreach (Age age in this.Ages.OrderBy(o => o.DisplayOrder))
                foreach (Wealth wealth in this.Wealths.OrderBy(o => o.DisplayOrder))
                    profile.AddAgeWealth(age, wealth, weights[i++]);

            return profile;
        }

        public void AddLocations(IEnumerable<Location> locations)
        {
            foreach (Location location in locations)
            {
                location.Economy = this;
                this.Locations.Add(location);
            }
        }

        public void AddIndustry(Industry industry)
        {
            industry.Economy = this;
            this.Industries.Add(industry);
        }

        public void Initialize(InitializeCallbacks callbacks)
        {
            this._callbacks = callbacks;
            //Create the first round
            this.CurrentRound = Round.CreateRound(this, null);

            //Setup each industry
            foreach (Industry industry in this.Industries)
            {
                Dictionary<Color, Profile> mapping = new Dictionary<Color, Profile>();
                //mapping.Add(Color.Blue, this.Profiles.Single(p => string.Compare(p.Name, "Rural", true) == 0));
                mapping.Add(Color.Red, this.Profiles.Single(p => string.Compare(p.Name, "Urban", true) == 0));
                mapping.Add(Color.Yellow, this.Profiles.Single(p => string.Compare(p.Name, "Suburban", true) == 0));
                mapping.Add(Color.FromArgb(0, 255, 0), this.Profiles.Single(p => string.Compare(p.Name, "Rural", true) == 0));

                industry.InitializeIndustry();
                foreach (Profile profile in this.Profiles)
                {
                    for (int i = 0; i < profile.TotalPopulation; i++)
                    {
                        Household h = new Household();
                        h.Profile = profile;
                        h.Identifier = string.Format("{0}:{1:D5}", profile.Name, i + 1);

                        Profile_Age_Wealth paw = this.GetProfileAgeWealth(profile, Statistics.Instance.Sample());
                        h.Age = paw.Age;
                        h.Wealth = paw.Wealth;

                        if (this._callbacks != null && this._callbacks.PositionHousehold != null)
                        {
                            Economy.GeoPosition pos = this._callbacks.PositionHousehold(h, mapping);
                            h.Latitude = pos.Latitude;
                            h.Longitude = pos.Longitude;

                        }

                    }
                }

                // Spin over the locations & make households
                //foreach (Location location in this.Locations)
                //{
                //    //Create base households
                //    location.InitializeHouseholds();

                //    //Now set industry-specific parameters for each household, as required.
                //    //foreach (Household household in location.Households)
                //    //    industry.GenerateHouseholdIndustryData(household);
                //}
            }

        }

        public void ProcessRound()
        {
            Round round = this.CurrentRound;
            ProcessRound(round);
        }

        public void ProcessRound(Round round)
        {
            // Process each industry & household
            foreach (Industry industry in this.Industries)
            {
                industry.PreProcessRound(round);

                //Let's randomize the household sequence.  This prevents clustering in the top & left of the map as we process each industry
                Random rnd = new Random();
                foreach (Household household in this.Households.OrderBy(h => rnd.NextDouble()))
                {
                    ProcessHouseholdInIndustry(household, industry, round);
                }

                industry.PostProcessRound(round);
            }

            //Record the company's revenue & expenses in aggregate
            foreach (Company company in this.Companies)
            {
                Company_Round cr = company.Company_Round.Single(x => x.Round == round);

                cr.Revenue = this.Industries.Sum(i => i.CalculateRevenue(company, round));
                cr.Expenses = this.Industries.Sum(i => i.CalculateExpenses(company, round));

                double cashAfterExpenses = cr.StartingCash - cr.Expenses;
                cr.FinanceActivity = Economy.CalculateInterest(cashAfterExpenses);
                cr.EndingCash = cr.StartingCash + cr.Revenue - cr.Expenses + cr.FinanceActivity;
            }

            //Make the next round
            this.CurrentRound = Round.CreateRound(this, round);

            foreach (Industry industry in this.Industries)
            {
                industry.PreProcessRound(this.CurrentRound);
            }

        }

        private void PreInitializeRound(Round previousRound)
        {

        }

        private void ProcessHouseholdInIndustry(Household household, Industry industry, Round round)
        {
            //Find all goods available to this household.
            IEnumerable<Good> goods = industry.GetAvailableGoods(household);

            //Some variables for keeping track of the winning good
            double maxSurplus = double.MinValue;
            Good winningGood = null;
            double winningCost = double.MinValue;

            //Loop through each good, calculate the consumer surplus, and keep track of which one is ahead
            foreach (Good good in goods)
            {
                double? value = industry.CalculateConsumerValue(household, good, round);
                if (value != null && value.Value > 0)   //Has a value & its positive
                {
                    // Possible parallelism optimization - don't track winner here just yet...
                    double cost = industry.CalculateConsumerCost(household, good, round);
                    double surplus = value.Value - cost;
                    if (surplus > maxSurplus && surplus > 0)
                    {
                        winningGood = good;
                        winningCost = cost;
                        maxSurplus = surplus;
                    }
                }
            }

            //If we have a winning good, record the sale
            // Possible parallelism optimization - single mutex here to check the winner & 
            // record the sale.
            if (winningGood != null)
                industry.RecordSale(household, winningGood, round, winningCost, maxSurplus);

        }

        internal Profile_Age_Wealth GetProfileAgeWealth(Profile profile, double draw)
        {
            //Create a List<> of age bins if one does not exist.  This is a common "singleton" pattern in C#.
            if (profileBins == null)    //Test if null
            {
                lock (_lock)        //Obtain a lock.
                {
                    if (profileBins == null)    //Because there could be a race condition between the first "if" and "lock", we check again (so as to not initialize twice)
                        profileBins = new List<Profile_Age_Wealth>(profile.Profile_Age_Wealth);
                }
            }

            //Match the probability with one of the bins.  Basically we walk down the 
            // list of bins until we find the one our draw value belongs to.
            foreach (Profile_Age_Wealth bin in profileBins)
            {
                if (draw < bin.Probability)
                    return bin;
                draw -= bin.Probability;
            }

            //Really should never happen.
            return null;
        }

        private static double CalculateInterest(double balance)
        {
            if (balance >= 0)
                return balance * 0.02;

            double remainingBalance = balance;
            double charges = 0;
            if (balance <= -5000000)
            {
                charges += (5000000 - remainingBalance) * 0.09;
                remainingBalance = -5000000;
            }
            if (balance <= -1000000)
            {
                charges += (1000000 - remainingBalance) * 0.07;
                remainingBalance = -1000000;
            }
            if (balance <= 0)
            {
                charges += remainingBalance * 0.05;
            }

            //Negative because we're paying interest on a loan
            return -1 * charges;

        }
    }
}
