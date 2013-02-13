using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuqua.CompetativeAnalysis.MarketGame
{
    partial class Industry
    {
        internal virtual double? CalculateConsumerValue(Household h, Good g, Round r)
        {
            return null;
        }

        internal virtual double CalculateConsumerCost(Household h, Good g, Round r)
        {
            return 0;
        }

        internal virtual IEnumerable<Good> GetAvailableGoods(Household h)
        {
            return null;
        }

        internal virtual void InitializeIndustry()
        {
        }

        internal virtual void RecordSale(Household h, Good g, Round r, double price, double surplus)
        {
        }

        public virtual void AddPopulationSettings(string type, double[] settings)
        {

        }

        public virtual void AddProfileSettings(Profile p, string type, double[] settings)
        {

        }

        internal virtual void PreProcessRound(Round round)
        {
        }

        internal virtual void PostProcessRound(Round round)
        {
        }

        internal virtual double CalculateExpenses(Company company, Round round)
        {
            return 0.0;
        }

        internal virtual double CalculateRevenue(Company company, Round round)
        {
            return 0.0;
        }
    }
}
