using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuqua.CompetativeAnalysis.MarketGame
{
    partial class Industry
    {
        internal class GoodSelector
        {
            public Good good;
            public double price;
            public double surplus;
        }

        internal virtual double? CalculateConsumerValue(Household h, Good g, Round r)
        {
            return null;
        }

        internal virtual double CalculateConsumerCost(Household h, Good g, Round r)
        {
            return 0;
        }

        internal virtual IEnumerable<Good> GetAvailableGoods(Household household)
        {
            return null;
        }

        internal virtual void InitializeIndustry()
        {
        }

        internal virtual void RecordSale(Household h, Good g, Round r, double price, double surplus)
        {
        }

        internal virtual Good RecordSale(Household h, Round r, IEnumerable<GoodSelector> goods)
        {
            return null;
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
