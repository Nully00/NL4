using System.Collections.Generic;
using UnityEngine;

namespace NL4.Linq
{
    public static partial class LinqNL
    {
        /// <summary>
        /// minからmaxのランダムな値を返します。
        /// </summary>
        public static IEnumerable<int> Random(int count, int min, int max)
        {
            for (int i = 0; i < count; i++)
            {
                yield return UnityEngine.Random.Range(min, max + 1);
            }

        }
        /// <summary>
        /// minからmaxのランダムな値を返します。
        /// </summary>
        public static IEnumerable<float> Random(int count, float min, float max)
        {
            for (int i = 0; i < count; i++)
            {
                yield return UnityEngine.Random.Range(min, max);
            }
        }
        /// <summary>
        /// minからmaxのランダムな値を返します。
        /// </summary>
        public static IEnumerable<Vector2> RandomVector2(int count, float min, float max)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Vector2(UnityEngine.Random.Range(min, max), UnityEngine.Random.Range(min, max));
            }
        }
    }
}
