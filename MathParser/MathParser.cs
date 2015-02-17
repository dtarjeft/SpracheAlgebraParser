using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Sprache;

namespace MathParser
{
    public class MathParser
    {
        public static string Parse(string statement)
        {
            var temp = NestedAddOrSubtract.Parse(statement);

            Console.WriteLine(temp);
            return temp.ToString();
        }

        public static readonly Parser<Expression> ExpressionParser =
            from negative in Sprache.Parse.Char('-').Optional()
            from number in
                Sprache.Parse.Decimal.Or(
                    Sprache.Parse.CharExcept(new[]
                    {
                        '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '+', '-', '*', '/', '^', '%', '.', '(', ')', '{',
                        '}', '[', ']'
                    }).AtLeastOnce())
            select (Expression.Parse(string.Concat(number), negative.IsDefined));

        private static readonly Parser<Func<Expression, Expression, Expression>> Subtract =
            Sprache.Parse.Char('-').Return<char, Func<Expression, Expression, Expression>>((a, b) => a.Subtract(b));

        private static readonly Parser<Func<Expression, Expression, Expression>> Add =
            Sprache.Parse.Char('+').Return<char, Func<Expression, Expression, Expression>>((a, b) => a.Add(b));

        private static readonly Parser<Func<Expression, Expression, Expression>> Multiply =
            Sprache.Parse.Char('*').Return<char, Func<Expression, Expression, Expression>>((a, b) => a.Multiply(b));

        private static readonly Parser<Func<Expression, Expression, Expression>> Divide =
            Sprache.Parse.Char('/').Return<char, Func<Expression, Expression, Expression>>((a, b) => a.Divide(b));

        private static readonly Parser<Func<Expression, Expression, Expression>> Exponent =
            Sprache.Parse.Char('^').Return<char, Func<Expression, Expression, Expression>>((a, b) => a.Exponent(b));

        private static readonly Parser<Func<Expression, Expression, Expression>> AddOrSubtract =
            Add.Or(Subtract);

        private static readonly Parser<Func<Expression, Expression, Expression>> MultiplyOrDivide =
            Multiply.Or(Divide);

        private static readonly Parser<Expression> ParentheticalContained =
            Sprache.Parse.Ref(() => NestedAddOrSubtract.Contained(Sprache.Parse.Char('('), Sprache.Parse.Char(')')));

        private static readonly Parser<Expression> NestedExponent = Sprache.Parse.ChainOperator(Exponent,
            ParentheticalContained.Or(ExpressionParser),
            (op, arg1, arg2) => op(arg1, arg2));

        private static readonly Parser<Expression> NestedMultiplyOrDivide = Sprache.Parse.ChainOperator(
            MultiplyOrDivide, NestedExponent,
            (op, arg1, arg2) => op(arg1, arg2));

        private static readonly Parser<Expression> NestedAddOrSubtract = Sprache.Parse.ChainOperator(AddOrSubtract,
            NestedMultiplyOrDivide,
            (op, arg1, arg2) => op(arg1, arg2));


    }
}


//     Func<char, double, double, double> operation = (c, d, d2) => c == '^'
      //          ? Math.Pow(d, d2)
      //          : c == '*'
      //              ? d*d2
      //              : c == '/'
      //                  ? d/d2
      //                  : c == '+'
      //                      ? d + d2
      //                      : c == '-' ? d - d2 : 0;
      //      var chainOperator = Sprache.Parse.ChainOperator(OperationParser,NumberParser, operation);
      //      return chainOperator.Parse(statement);
      //  }


        
      //  public static Parser<double> NumberParser =
      //      from negative in Sprache.Parse.Char('-').Optional()
      //      from number in Sprache.Parse.Decimal
      //      select (double.Parse(number)*(negative.IsDefined ? -1 : 1));

      //  public static Parser<Func<double, double, double>> SubtractParser =
      //      Sprache.Parse.Char('-').Return<char, Func<double, double, double>>((a, b) => a - b);

      //  public static Parser<char> AddParser =
      //      Sprache.Parse.Char('+').Select(operation => (operation));

      //  public static Parser<char> MultiplicationParser =
      //      Sprache.Parse.Char('*').Select(operation => (operation));

      //  public static Parser<char> DivisionParser =
      //      Sprache.Parse.Char('/').Select(operation => (operation));

      //  public static Parser<char> ExponentParser =
      //      Sprache.Parse.Char('^').Select(operation => (operation));

      ////  public static Parser<double> NestedMultiplicationOrSomething =
      ////      Sprache.Parse.ChainOperator(Sprache.Parse.Char('^'), NumberParser,)

      //  public static Parser<char> OperationParser =
      //      ExponentParser.Or(MultiplicationParser.Or(DivisionParser.Or(AddParser.Or(SubtractParser))));

      //  public static Parser<IEnumerable<char>> OrderOperationsParser =
      //      ExponentParser.Repeat(0, 10)
      //          .Or(
      //              MultiplicationParser.Repeat(0, 10)
      //                  .Or(
      //                      DivisionParser.Repeat(0, 10)
      //                          .Or(AddParser.Repeat(0, 10)
      //                              .Or(SubtractParser.Repeat(0, 10)))));