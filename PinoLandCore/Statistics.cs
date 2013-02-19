using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.Distributions;

namespace Fuqua.CompetativeAnalysis.MarketGame
{
    /// <summary>
    /// A helper class for statistics.  It is implemented as a singleton.  This lets us have a 
    /// http://en.wikipedia.org/wiki/Singleton_pattern
    /// http://www.dofactory.com/Patterns/PatternSingleton.aspx
    /// 
    /// </summary>
    public class Statistics
    {
        private static object _lockObject = new object();
        private static Statistics _singleton = null;
        private System.Random _randomSource = null;

        private Statistics()
        {
            _randomSource = new Random();
        }

        public static Statistics Instance
        {
            get
            {
                if (_singleton == null)
                {
                    lock (_lockObject)
                    {
                        if (_singleton == null)
                            _singleton = new Statistics();
                    }
                }
                return _singleton;
            }
        }


        public static Random SetRandomSource
        {
            set
            {
                _singleton._randomSource = value;
            }
        }

        public double Sample()
        {
            return _singleton._randomSource.NextDouble();
        }

        public double NormalSample(double mean, double stddev)
        {

            return Normal.Sample(_singleton._randomSource, mean, stddev);
        }

        public double HalfNormalSample(double stddev)
        {
            double value = Normal.Sample(_singleton._randomSource, 0, stddev);
            if (value < 0)
                return -1 * value;
            return value;
        }

        public double DiscreteSample()
        {
            return _singleton._randomSource.NextDouble();
        }

        public double HalfNormalInvCDF(double p, double mean, double stddev)
        {
            double a = MathNet.Numerics.SpecialFunctions.Erf((p + mean) / (Math.Sqrt(2) * stddev));
            double b = MathNet.Numerics.SpecialFunctions.Erf((p - mean) / (Math.Sqrt(2) * stddev));

            return 0.5 * (a + b);
        }

        public double HalfNormalInvCDF(double p, double stddev)
        {
            return HalfNormalInvCDF(p, stddev, 0);
        }
    }
}
