using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenter.Extensions
{
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> TakeMany<T>(this IEnumerable<T> data, int num)
        {
            var countD = data.Count();
            var output = new List<IEnumerable<T>>();
            for (int i = 0; i < countD; i+=num)
                output.Add(data.Skip(i).Take(num));

            return output;
        }
    }
}
