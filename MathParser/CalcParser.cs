using System;
using System.Linq;
using Sprache;

namespace MathParser
{
    public class CalcParser
    {
        public string Parse(string statement)
        {
            return DerivativeExtractor.Parse(statement).Expression;
        }

        private static readonly Parser<Derivative> DerivativeExtractor =
            from open in Sprache.Parse.Char('{')
            from expression in Sprache.Parse.CharExcept('}').Many().Text()
            from close in Sprache.Parse.Char('}')
            from derive in Sprache.Parse.Char('d')
            from var in Sprache.Parse.AnyChar
            select new Derivative(expression, var);
    }

    //Only worrying about single terms (for now...)
    public class Derivative
    {
        public static char DeriveChar;
        public string Expression;

        public Derivative(string expression, char deriveChar)
        {
            Expression = expression;
            DeriveChar = deriveChar;
        }
    }
}