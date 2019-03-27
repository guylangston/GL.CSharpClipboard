using System.Collections.Generic;
using System.Threading;

namespace CSharpClipboard
{
    public static class Helpers
    {
        public static IEnumerable<(int index, T value)> WithIndex<T>(this IEnumerable<T> source)
        {
            int cc = -1; // so first index is 0
            foreach (var item in source)
            {
                var ccc = Interlocked.Increment(ref cc);
                yield return (ccc, item);
                
            }
        }

    }
}