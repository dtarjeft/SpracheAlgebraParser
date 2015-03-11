using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Sprache;

namespace MathParser
{
    public class MathParser<T> : IMathParser<T> where T : ISupportedMathOps<T>, ITermFactory<T>
    {
        private static ITermFactory<T> _termFactory; 
        public MathParser(ITermFactory<T> termFactory )
        {
            _termFactory = termFactory;
        }
        public T Parse(string statement)
        {
            return NestedAddOrSubtract.Parse(statement);
        }
        public static T Compute(char op, T a, T b)
        {
            switch (op)
            {
                case('+'):
                    return a.Add(b);
                case('-'):
                    return a.Subtract(b);
                case('*'):
                    return a.Multiply(b);
                case('/'):
                    return a.Divide(b);
                case('^'):
                    return a.Power(b);
                default:
                    throw new Exception("Invalid Computation Attempted: "+op);
            }
        }

        private static readonly Parser<Func<T, T, T>> Add =
            Sprache.Parse.Char('+').Return<char, Func<T, T, T>>((a, b) => Compute('+', a, b));

        private static readonly Parser<Func<T, T, T>> Subtract =
            Sprache.Parse.Char('-').Return<char, Func<T, T, T>>((a, b) => Compute('-', a, b));

        private static readonly Parser<Func<T, T, T>> Divide =
            Sprache.Parse.Char('/').Return<char, Func<T, T, T>>((a, b) => Compute('/', a, b));

        private static readonly Parser<Func<T, T, T>> Multiply =
            Sprache.Parse.Char('*').Return<char, Func<T, T, T>>((a, b) => Compute('*', a, b));

        private static readonly Parser<Func<T, T, T>> Exponent =
            Sprache.Parse.Char('^').Return<char, Func<T, T, T>>((a, b) => Compute('^', a, b));

        private static readonly Parser<Func<T, T, T>> AddorSubtract =
            Add.Or(Subtract);

        private static readonly Parser<Func<T, T, T>> MultiplyOrDivide =
            Multiply.Or(Divide);

        private static readonly Parser<T> Term =
            from negative in Sprache.Parse.Char('-').Optional()
            from term in Sprache.Parse.Decimal.Or(Sprache.Parse.CharExcept(new[]
                {
                    '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '+', '-', '*', '/', '^', '%', '.', '(', ')',
                    '{',
                    '}', '[', ']'
                }).AtLeastOnce())
            select _termFactory.ToTerm(term, negative.IsDefined);

        private static T Negate(T term)
        {
            return Compute('*', (T) Convert.ChangeType(-1, typeof (T)), term);
        }

        private static readonly Parser<T> Parenthesis =
            Sprache.Parse.Ref(() => NestedAddOrSubtract.Contained(Sprache.Parse.Char('('), Sprache.Parse.Char(')')));

        private static readonly Parser<T> NestedExponent =
            Sprache.Parse.ChainOperator(Exponent, Parenthesis.Or(Term),
                ((op, a, b) => op(a, b)));

        private static readonly Parser<T> NestedMultiplyOrDivide =
            Sprache.Parse.ChainOperator(MultiplyOrDivide, NestedExponent, 
            ((op, a, b) => op(a, b)));

        private static readonly Parser<T> NestedAddOrSubtract =
            Sprache.Parse.ChainOperator(AddorSubtract, NestedMultiplyOrDivide, 
            ((op, a, b) => op(a, b)));
    }
}

