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
        /// 指定された一番小さい値の要素を取得
        /// </summary>
        public static TSource MinBy<TSource, TRusult>(this IEnumerable<TSource> self, Func<TSource, TRusult> selector) where TRusult : IComparable<TRusult>
        {
            return self.Aggregate((a, b) => selector(a).CompareTo(selector(b)) < 0 ? a : b);
        }
        /// <summary>
        /// 指定された一番大きい値の要素を取得
        /// </summary>
        public static TSource MaxBy<TSource, TRusult>(this IEnumerable<TSource> xs, Func<TSource, TRusult> selector) where TRusult : IComparable<TRusult>
        {
            return xs.Aggregate((a, b) => selector(a).CompareTo(selector(b)) > 0 ? a : b);
        }
    }
}
