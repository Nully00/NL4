using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NL4.MathNL
{
    public static partial class MathNL
    {
        /// <summary>
        /// 一番近い値を取得します。(近さが同じ場合上の値を優先します。)
        /// </summary>
        public static int ApproxUp(this IEnumerable<int> list, int target)
        {
            int num = list.Select(x => Mathf.Abs(x - target)).Min();
            return list.Contains(target + num) ? target + num : target - num;
        }
        /// <summary>
        /// 一番近い値を取得します。(近さが同じ場合上の値を優先します。)
        /// </summary>
        public static float ApproxUp(this IEnumerable<float> list, float target)
        {
            float num = list.Select(x => Mathf.Abs(x - target)).Min();
            return list.Contains(target + num) ? target + num : target - num;
        }
        /// <summary>
        /// 一番近い値を取得します。(近さが同じ場合下の値を優先します。)
        /// </summary>
        public static int ApproxDown(this IEnumerable<int> list, int target)
        {
            int num = list.Select(x => Mathf.Abs(x - target)).Min();
            return list.Contains(target - num) ? target - num : target + num;
        }
        /// <summary>
        /// 一番近い値を取得します。(近さが同じ場合下の値を優先します。)
        /// </summary>
        public static float ApproxDown(this IEnumerable<float> list, float target)
        {
            float num = list.Select(x => Mathf.Abs(x - target)).Min();
            return list.Contains(target - num) ? target - num : target + num;
        }
    }
}
