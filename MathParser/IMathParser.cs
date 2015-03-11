namespace MathParser
{
    /// <summary>
    /// Simple interface for a parser that reads string equivalents of mathematical statements and evaluates them.
    /// </summary>
    public interface IMathParser<out T>
    {
        /// <summary>
        /// Evaluates the given statement.
        /// </summary>
        /// <param name="statement">A string representatin of a mathematical statement.</param>
        /// <returns>The value of the statement once evaluated.</returns>
        T Parse(string statement);
    }
}
