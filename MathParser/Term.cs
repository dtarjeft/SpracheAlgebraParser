using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MathParser
{
    public class Term : ISupportedMathOps<Term>, ITermFactory<Term>
    {

        public Term Power(Term b)
        {
            Coefficient = Math.Pow(Coefficient,b.Coefficient);
            return this;
        }

        public Term Divide(Term b)
        {
            Coefficient /= b.Coefficient;
            return this;
        }

        public Term Add(Term b)
        {
            Coefficient += b.Coefficient;
            return this;
        }

        public Term Subtract(Term b)
        {
            Coefficient -= b.Coefficient;
            return this;
        }

        public Term Multiply(Term b)
        {
            Coefficient *= b.Coefficient;
            return this;
        }

        public Term ToTerm(IEnumerable<char> term, bool negative)
        {
            var output = new Term();
            double coefficient;
            double.TryParse(term as string, out coefficient);
            output.Coefficient = negative ? coefficient*-1 : coefficient;
            return output;
        }

        public double Coefficient { get; set; }

        public override string ToString()
        {
            return Coefficient.ToString();
        }
    }

}
