using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NL4.Linq
{
    public static partial class LinqNL
    {
        /// <summary>
        /// シーケンスの各要素にインデックスを付与します。
        /// </summary>
        public static IEnumerable<(TSource val, int index)> WithIndex<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            int index = 0;
            foreach (var i in source)
            {
                yield return (i, index);
                index++;
            }
        }
        /// <summary>
        /// シーケンスの各要素にインデックスを付与します。
        /// </summary>
        public static IEnumerable<(TSource val, int x, int y)> WithIndex<TSource>(this TSource[,] self)
        {
            if (self == null)
                throw new ArgumentNullException(nameof(self));

            for (int x = 0; x < self.GetLength(0); x++)
                for (int y = 0; y < self.GetLength(1); y++)
                    yield return (self[x, y], x, y);
        }

    }
}
