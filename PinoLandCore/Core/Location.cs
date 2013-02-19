using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;

namespace Fuqua.CompetativeAnalysis.MarketGame
{
    partial class Location
    {
        public double DistanceTo(Location other)
        {
            //Replace this with a Haversine calculation
            // http://www.movable-type.co.uk/scripts/latlong.html
            return Math.Sqrt(Math.Pow(this.CenterX - other.CenterX, 2)
                            + Math.Pow(this.CenterY - other.CenterY, 2));
        }

        public void InitializeHouseholds()
        {
            //for (int i = 0; i < this.TotalPopulation; i++)
            //{
            //    Household h = new Household();

            //    Profile_Age_Wealth profile = this.Economy.GetProfileAgeWealth(this.Profile, Statistics.Instance.Sample());
            //    h.Age = profile.Age;
            //    h.Wealth = profile.Wealth;

            //    h.Location = this;
            //    h.Identifier = string.Format("{0}:{1:D5}", this.Identifier, i + 1);
            //    //h.Profile = this.Profile;
            //    if (this.Economy._callbacks != null && this.Economy._callbacks.PositionHousehold != null)
            //    {
            //        Economy.GeoPosition pos = this.Economy._callbacks.PositionHousehold(this, h);
            //        h.Latitude = pos.Latitude;
            //        h.Longitude = pos.Longitude;
            //    }
            //}
        }

        //public void SetLocation(float north, float west, float south, float east)
        //{

        //}
    }

    public static class LocationExtensions
    {
        public static Location Find(this EntityCollection<Location> locations, string Identifier)
        {
            return locations.FirstOrDefault(l => string.Compare(l.Identifier, Identifier, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}
