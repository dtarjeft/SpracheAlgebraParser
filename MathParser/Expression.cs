using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Sprache;

namespace MathParser
{
    public class Expression
    {
        public Expression(string statement, bool isNum) : this(statement, isNum, false)
        {
            _isNum = isNum;
            _contents = statement;
        }

        public Expression(string statement, bool isNum, bool isNegative)
        {
            _isNum = isNum;
            _contents = isNegative ? "-" + statement : statement;
            Console.WriteLine("{1} _isNum: {0}", _isNum, _contents);
        }

        private readonly string _contents;
        private readonly bool _isNum;

        private static bool IsANumber(string statement)
        {
            return statement.All(ch => new[] {'1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.'}.Contains(ch));
        }

        internal static Expression Parse(string statement, bool isNegative)
        {
            return IsANumber(statement) ? new Expression(statement, true, isNegative) : new Expression(statement, false, isNegative);
        }

        public Expression Subtract(Expression expression)
        {
            if (this._isNum && expression._isNum)
            {
                return new Expression((Convert.ToDouble(_contents) - Convert.ToDouble(expression._contents)).ToString(), true);
            }
            return new Expression(_contents + "-" + expression._contents, false);
        }

        public Expression Add(Expression expression)
        {
            if (this._isNum && expression._isNum)
            {
                return new Expression((Convert.ToDouble(_contents) + Convert.ToDouble(expression._contents)).ToString(), true);
            }
            return new Expression(_contents + "+" + expression._contents, false);
        }

        public Expression Multiply(Expression expression)
        {
            if (this._isNum && expression._isNum)
            {
                return new Expression((Convert.ToDouble(_contents) * Convert.ToDouble(expression._contents)).ToString(), true);
            }
            return new Expression(_contents + "*" + expression._contents, false);
        }

        public Expression Divide(Expression expression)
        {
            if (this._isNum && expression._isNum)
            {
                return new Expression((Convert.ToDouble(_contents) / Convert.ToDouble(expression._contents)).ToString(), true);
            }
            return new Expression(_contents + "/" + expression._contents, false);
        }

        public Expression Exponent(Expression expression)
        {
            if (this._isNum && expression._isNum)
            {
                return new Expression(Math.Pow(Convert.ToDouble(_contents), Convert.ToDouble(expression._contents)).ToString(), true);
            }
            return new Expression(_contents + "^" + expression._contents, false);
        }

        public override string ToString()
        {
            return _contents;
        }

    }
}