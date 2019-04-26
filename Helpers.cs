using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// Render a simple list. Nulls are skipped
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">Source</param>
        /// <param name="toString">if null, then object.ToString() is used</param>
        /// <param name="seperator">if null, no seperator is used</param>
        /// <returns>Concatenated string</returns>
        public static string ToStringConcat<T>(this IEnumerable<T> data, Func<T, object> toString, string seperator, bool skipNull = true, int wrap = -1, string wrapPrefix = "", int takeMax = -1)
        {
            if (data == null) return null;

            var sb = new StringBuilder();
            var line = new StringBuilder();
            bool first = true;
            int total = -1;
            if (takeMax > 0)
            {
                total = data.Count();
                data = data.Take(takeMax);
            }
            int cc = 0;
            foreach (T item in data)
            {
                if (skipNull && item == null)
                {
                    continue;
                }
                cc++;

                if (first)
                {
                    first = false;
                }
                else
                {
                    if (seperator != null) line.Append(seperator);
                }

                if (toString != null)
                {
                    object val = toString(item);
                    if (val != null)
                    {
                        line.Append(val.ToString());
                    }
                }
                else
                {
                    line.Append(item.ToString());
                }
                if (wrap < 0)
                {
                    sb.Append(line);
                    line.Clear();
                }
                else
                {
                    if (line.Length > wrap)
                    {
                        sb.Append(wrapPrefix);
                        sb.Append(line);
                        sb.AppendLine();
                        line.Clear();
                    }
                }
            }

            if (takeMax > 0 && cc >= takeMax)
            {
                sb.Append($"... {total - cc} more");
            }

            sb.Append(line);
            return sb.ToString();
        }


    }
}