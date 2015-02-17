namespace MathParser
{
    /// <summary>
    /// An interface that parses a string equivalent of an algebraic statement.
    /// </summary>
    public interface IExpressionParser
    {
        /// <summary>
        /// Evaluates a given expression.
        /// </summary>
        /// <param name="statement">A string representation of an algebraic statement.</param>
        /// <returns>The value of the expression, simplified.</returns>
        string Parse(string statement);
    }
}