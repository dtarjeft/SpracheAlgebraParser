using System.Collections;
using System.Collections.Generic;

namespace MathParser
{
    public interface ITermFactory<out T>
    {
        T ToTerm(IEnumerable<char> term, bool negative);
    }
}