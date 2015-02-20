namespace MathParser.UnitTests
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class MathParserTests
    {
        private const double Delta = 0.00001;

        [Test]
        [TestCase("3+4", 7.0)]
        [TestCase("7/2", 3.50)]
        [TestCase("14-11", 3.00)]
        [TestCase("8*12", 96.0)]
        public void MathParser_BasicFourFunctions(string statement, double expected)
        {
            this.Evaluate(statement, expected);
        }

        [Test]
        [TestCase("2^3", 8.0)]
        [TestCase("3^4", 81.0)]
        [TestCase("9^0.5", 3.0)]
        [TestCase("4^-1", 0.25)]
        [TestCase("52^0", 1.00)]
        public void MathParser_Exponentiation(string statement, double expected)
        {
            this.Evaluate(statement, expected);
        }

        [Test]
        [TestCase("1+2*3", 7.0)]
        [TestCase("2*3+1", 7.0)]
        [TestCase("3+2/4-1*6/2", 0.5)]
        [TestCase("2/4+3-1*6/2", 0.5)]
        [TestCase("-1*6/2+2/4+3", 0.5)]
        public void MathParser_OrderOfOperations_BasicFourFunctions(string statement, double expected)
        {
            this.Evaluate(statement, expected);
        }

        [Test]
        [TestCase("1+2^3+4", 13.00)]
        [TestCase("1+4+2^3", 13.00)]
        [TestCase("2^3+1+4", 13.00)]
        public void MathParser_OrderOfOperations_Exponentiation(string statement, double expected)
        {
            this.Evaluate(statement, expected);
        }

        [Test]
        [TestCase("(1+2)*3", 9.0)]
        [TestCase("(3+2)/(4-1)*6/2", 5.00)]
        [TestCase("-1*(6/(2+2)/4+3)", -3.375)]
        [TestCase("2^(3+1)+4", 20.00)]
        public void MathParser_OrderOfOperations_Parentheses(string statement, double expected)
        {
            this.Evaluate(statement, expected);
        }

        [Test]
        [TestCase("(1+2")]
        [TestCase("2**3+4")]
        [TestCase("3+((4*2)/2")]
        [TestCase("1+2*3^1+4)")]
        public void MathParser_InvalidStatements(string statement)
        {
            var parser = new MathParser();
            try
            {
                MathParser.Parse(statement);
                Assert.Fail("Should have thrown an error.");
            }
            catch (Exception)
            {
                // Do nothing - this is a pass
            }
        }

        [Test]
        [TestCase("1*x", "1*x")]
        [TestCase("2+(3*x)+4", "6+3*x")]
        [TestCase("2+3*x+4", "6+3*x")]
        [TestCase("2*x+3*y", "2*x+3*y")]
        [TestCase("2*(3*x+1)", "6*x+2")]
        [TestCase("2*3*x+3*4*5*t", "6*x+60*t")]
        [TestCase("(2*x+4)*(3*x+1)", "6x^2+14x+4")]
        [TestCase("x", "x")]
        [TestCase("x^3*y", "x^3*y")]
        [TestCase("3^x*3^y", "3^xy")]
        [TestCase("x*x", "x^2")]
        public void MathParser_AlgebraicStatements(string statement, string expected)
        {
            this.Evaluate(statement, expected);
        }

        private void Evaluate(string statement, string expected)
        {
           Assert.AreEqual(expected, MathParser.Parse(statement));
        }

        private void Evaluate(string statement, double expected)
        {
            Assert.AreEqual(expected, Convert.ToDouble(MathParser.Parse(statement)));
        }
    }

    [TestFixture]
    public class CalcParserTests
    {
        private const double Delta = 0.00001;

        [Test]
        [TestCase("{1*x}dx", "1")]
        [TestCase("{3*x^2}dx", "6")]
        [TestCase("{4*y}dy", "4")]
        [TestCase("{3*x*y}dx", "3*(dy/dx)")]
        public void Product_Rule(string statement, string expected)
        {
            Evaluate(statement, expected);

            
        }

        private void Evaluate(string statement, string expected)
        {
            var parser = new CalcParser();
            Assert.AreEqual(expected, parser.Parse(statement));
        }
    }
}
