using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fuqua.CompetativeAnalysis.MarketGame
{
    partial class Food_Good : Good
    {
        private static object _lock = new object();
        private Dictionary<int, double> _distances;

        public Dictionary<int, double> Distances
        {
            get
            {
                if (_distances == null)
                {
                    lock (_lock)
                    {
                        if (_distances == null)
                        {
                            _distances = new Dictionary<int, double>();
                            foreach (Location l in this.Industry.Economy.Locations)
                                _distances.Add(l.LocationId, l.DistanceTo(this.Location));
                        }
                    }
                }
                return _distances;
            }

        }
    }
}
