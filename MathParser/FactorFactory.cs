using System.Collections.Generic;

namespace MathParser
{
    public static class FactorFactory
    {
        private static List<Dictionary<string, Expression>> _factorsList;
        private static int _layers;
        public static string FindFactors(string statement)
        {
            _factorsList = new List<Dictionary<string, Expression>>();
            _layers = 1;
            return null;
        }


    }
}