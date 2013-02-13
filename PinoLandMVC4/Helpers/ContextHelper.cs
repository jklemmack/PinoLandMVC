using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PinoLandMVC4
{
    public class ContextHelper
    {
        public static string ConnectionString;

        public static Fuqua.CompetativeAnalysis.MarketGame.GameDataObjectContext GetObjectContext()
        {
            return new Fuqua.CompetativeAnalysis.MarketGame.GameDataObjectContext(ConnectionString);
        }
    }
}