namespace MathParser
{
    public interface ISupportedMathOps<T>
    {
        T Power(T b);
        T Divide(T b);
        T Add(T b);
        T Subtract(T b);
        T Multiply(T b);
    }


}