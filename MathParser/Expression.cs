using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
        }

        private string _contents;
        private bool _isNum;

        private static List<string> SplitByChar(string a, char[] charToFactorBy)
        {
            return new List<string>(a.Split(charToFactorBy, StringSplitOptions.RemoveEmptyEntries));
        }

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
                _contents = (Convert.ToDouble(_contents) - Convert.ToDouble(expression._contents)).ToString();
                return this;
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
            else if (this._isNum && !expression._isNum)
            {
                var factorThat = SplitByChar(expression._contents, new[] {'+'});
                var combinedList = factorThat.Select(str => str.Insert(0, _contents + "*")).ToList();
                var result = new StringBuilder();
                for (var index = 0; index < combinedList.Count; index++)
                {
                    var t = combinedList[index];
                    var tsubTokens = new List<string>(t.Split(new[] {'*'}, StringSplitOptions.RemoveEmptyEntries));
                    if (tsubTokens.Count() > 1)
                    {
                        tsubTokens.Insert(0, "1");
                      
                        for (var i = 1; i < tsubTokens.Count; i++)
                        {
                            var token = tsubTokens[i];
                            if (!IsANumber(token)) continue;
                            tsubTokens[0] = (Convert.ToDouble(tsubTokens[0])*Convert.ToDouble(token)).ToString();
                            tsubTokens[i] = string.Empty;
                        }
                        var mergedOutput = new StringBuilder();
                        tsubTokens.RemoveAll(EmptyString);
                        for (var i = 0; i < tsubTokens.Count; i++)
                        {
                            var tsubToken = tsubTokens[i];
                            mergedOutput.Append(tsubToken);
                            if (i < tsubTokens.Count - 1)
                            {
                                mergedOutput.Append("*");
                            }
                        }
                        
                        t = mergedOutput.ToString();
                    }
                    result.Append(t);
                    if (index < combinedList.Count - 1)
                    {
                        result.Append("+");
                    }
                }
                _contents = result.ToString();
                _isNum = false;
                return this;
            }
            else
            {
                var chArray = new []{'+'};
                var factorThis = SplitByChar(this._contents, chArray);
                var factorThat = SplitByChar(expression._contents, chArray);
                //var result = Recombine(factorThis, factorThat, new[] {'*'});
            }
            return new Expression(_contents + "*" + expression._contents, false);
        }

        private static bool EmptyString(string obj)
        {
            return (obj == string.Empty);
        }

        //private static string Recombine(List<string> s1, List<string> s2, char[] c)
        //{
        //    var result = new List<string>();

        //}

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