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
        [TestCase("7/2", 3.5)]
        [TestCase("14-11", 3.0)]
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
        [TestCase("52^0", 1.0)]
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
        [TestCase("1+2^3+4", 13.0)]
        [TestCase("1+4+2^3", 13.0)]
        [TestCase("2^3+1+4", 13.0)]
        public void MathParser_OrderOfOperations_Exponentiation(string statement, double expected)
        {
            this.Evaluate(statement, expected);
        }

        [Test]
        [TestCase("(1+2)*3", 9.0)]
        [TestCase("(3+2)/(4-1)*6/2", 5.0)]
        [TestCase("-1*(6/(2+2)/4+3)", -3.375)]
        [TestCase("2^(3+1)+4", 20.0)]
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
            var parser = new MathParser<Term>(new Term());
            try
            {
                parser.Parse(statement);
                Assert.Fail("Should have thrown an error.");
            }
            catch (Exception)
            {
                // Do nothing - this is a pass
            }
        }

        private void Evaluate(string statement, double expected)
        {
            //var parser = new MathParser<double>();
            var parser = new MathParser<Term>(new Term());
            Assert.AreEqual(expected,parser.Parse(statement).Coefficient);
        }
    }
}
