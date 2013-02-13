using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuqua.CompetativeAnalysis.MarketGame
{
    partial class Profile
    {

        internal Profile()
        {
        }

        internal void AddAgeWealth(Age age, Wealth wealth, double probability)
        {
            Profile_Age_Wealth p = new Profile_Age_Wealth();
            p.Age = age;
            p.Wealth = wealth;
            p.Profile = this;
            p.Probability = probability;
        }
    }

}
