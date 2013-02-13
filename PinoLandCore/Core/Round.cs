using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fuqua.CompetativeAnalysis.MarketGame
{
    partial class Round
    {
        internal static Round CreateRound(Economy economy, Round previousRound)
        {
            Round round = new Round();
            round.PreviousRound = previousRound;
            round.EconomyId = economy.EconomyId;

            if (previousRound == null)
            {
                round.Sequence = 0;
                round.Identifier = "Round 0";
            }
            else
            {
                round.Sequence = previousRound.Sequence + 1;
                round.Identifier = String.Format("Round {0:N0}", round.Sequence);
            }


            foreach (Company company in economy.Companies)
            {
                Company_Round cr = new Company_Round();
                cr.Company = company;

                if (previousRound != null)
                    cr.StartingCash = company.Company_Round.Single(x => x.Round == previousRound).EndingCash;
                else
                    cr.StartingCash = 1000000;  // $1M.  Should pull from some economy setting
            }

            return round;
        }
    }
}
