using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Threading.Tasks;

namespace Fuqua.CompetativeAnalysis.MarketGame
{
    partial class Location
    {
        static object _lock = new object();
        public double DistanceTo(Location other)
        {
            //Replace this with a Haversine calculation when doing geospatial
            // http://www.movable-type.co.uk/scripts/latlong.html
            return Math.Sqrt(Math.Pow(this.X - other.X, 2)
                            + Math.Pow(this.Y - other.Y, 2));
        }

        public void InitializeHouseholds()
        {
            //ConcurrentStack<Household> households = new ConcurrentStack<Household>();
            //Parallel.For(0, this.TotalPopulation, i =>
            for (int i = 0; i < this.TotalPopulation; i++)
            {
                Profile_Age_Wealth profile = this.Economy.GetProfileAgeWealth(this.Profile, Statistics.Instance.Sample());

                //lock (_lock)
                //{
                //Household h = new Household();
                //h.Age = profile.Age;
                //h.Wealth = profile.Wealth;

                //h.Location = this;
                //h.Identifier = string.Format("{0}:{1:D5}", this.Identifier, i + 1);
                //h.Profile = this.Profile;
                //}
                Household h = new Household();
                h.EconomyId = this.EconomyId;
                h.Identifier = string.Format("{0}:{1:D5}", this.Identifier, i + 1);
                h.AgeId = profile.AgeId;
                h.WealthId = profile.WealthId;
                h.LocationId = this.LocationId;
                h.ProfileId = profile.ProfileId;
                //households.Add(h);
                lock (_lock)
                    this.Economy.Households.Add(h);
            }//);

            //foreach (Household h in households)
            //    this.Economy.Households.Add(h);
        }

    }

    public static class LocationExtensions
    {
        public static Location Find(this EntityCollection<Location> locations, string Identifier)
        {
            return locations.FirstOrDefault(l => string.Compare(l.Identifier, Identifier, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}
