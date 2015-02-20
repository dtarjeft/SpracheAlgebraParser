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
        public Expression(string statement, bool isNum, bool isNegative = false)
        {
            _isNum = isNum;
            _contents = isNegative ? "-" + statement : statement;
        }

        private string _contents;
        private bool _isNum;

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
                _contents = (Convert.ToDouble(_contents) + Convert.ToDouble(expression._contents)).ToString();
                return this;
            }
            var factorThis = new List<string>(_contents.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries));
            var factorThat =
                new List<string>(expression._contents.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries));
            for (var i = 0; i < factorThis.Count; i++)
            {
                for (var j = 0; j < factorThat.Count; j++)
                {
                    
                }
            }

            return new Expression(_contents + "+" + expression._contents, false);
        }

        public Expression Multiply(Expression expression)
        {
            if (this._isNum && expression._isNum)
            {
                _contents = (Convert.ToDouble(_contents) * Convert.ToDouble(expression._contents)).ToString();
                return this;
            }
            else if (this._isNum && !expression._isNum)
            {
                var factorThat = expression._contents.Split(new[] {'+'}, StringSplitOptions.RemoveEmptyEntries);
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
                //var result = Recombine(factorThis, factorThat, new[] {'*'});
            }
            return new Expression(_contents + "*" + expression._contents, false);
        }

        private static bool EmptyString(string obj)
        {
            return (obj == string.Empty);
        }

        public Expression Divide(Expression expression)
        {
            if (this._isNum && expression._isNum)
            {
                _contents = (Convert.ToDouble(_contents) / Convert.ToDouble(expression._contents)).ToString();
                return this;
            }
            return new Expression(_contents + "/" + expression._contents, false);
        }

        public Expression Exponent(Expression expression)
        {
            if (this._isNum && expression._isNum)
            {
                _contents = Math.Pow(Convert.ToDouble(_contents), Convert.ToDouble(expression._contents)).ToString();
                return this;
            }
            return new Expression(_contents + "^" + expression._contents, false);
        }

        public override string ToString()
        {
            return _contents;
        }

    }
}