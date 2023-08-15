using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.NL4.MathNL
{
    public static partial class MathNL
    {
        /// <summary>
        /// 分散を求めます。
        /// </summary>
        public static float Var(this IEnumerable<float> list)
        {
            return (float)list.Select(x => Mathf.Pow(x - list.Average(), 2)).Average();
        }
        /// <summary>
        /// 標準偏差を求めます。
        /// </summary>
        public static float Stdev(this IEnumerable<float> list)
        {
            return (float)Mathf.Sqrt(list.Var());
        }
        /// <summary>
        /// 範囲を求めます。
        /// </summary>
        public static float Range(this IEnumerable<float> list)
        {
            return list.Max() - list.Min();
        }
        /// <summary>
        /// 中央値を求めます。((NL1↓nlog)過去バージョン)
        /// </summary>
        public static float Median(this IEnumerable<float> list)
        {
            return (list.Count() % 2 == 1) ? list.OrderBy(x => x).Skip(list.Count() / 2).ToList()[0]
                            : ((float)list.OrderBy(x => x).Skip((list.Count()) / 2 - 1).ToList()[0] + (float)list.OrderBy(x => x).Skip((list.Count()) / 2).ToList()[0]) / 2.0f;
        }
    }
}
