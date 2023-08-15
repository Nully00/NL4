using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NL4.Extensions
{
    public static class GenerateArray
    {
        /// <summary>
        /// 指定された長さの連続した整数の配列を生成します。
        /// Generates an array of consecutive integers of the specified length.
        /// </summary>
        /// <param name="length">生成する配列の長さ。The length of the array to generate.</param>
        /// <returns>連続した整数の配列。An array of consecutive integers.</returns>
        public static int[] Range(int length)
        {
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = i;
            }
            return result;
        }
        /// <summary>
        /// 指定された範囲内のランダムな整数を含む配列を生成します。
        /// Generates an array containing random integers within the specified range.
        /// </summary>
        /// <param name="min">ランダムな整数の最小値。Minimum value of the random integer.</param>
        /// <param name="max">ランダムな整数の最大値。Maximum value of the random integer.</param>
        /// <param name="count">生成する配列の長さ。The length of the array to generate.</param>
        /// <returns>ランダムな整数の配列。An array of random integers.</returns>
        public static int[] Random(int min, int max, int count)
        {
            int[] result = new int[count];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = UnityEngine.Random.Range(min, max);
            }
            return result;
        }
        /// <summary>
        /// 指定された範囲内のランダムな長さと値を持つ整数の配列を生成します。
        /// Generates an array of random integers with random length within the specified range.
        /// </summary>
        /// <param name="min">ランダムな整数の最小値。Minimum value of the random integer.</param>
        /// <param name="max">ランダムな整数の最大値。Maximum value of the random integer.</param>
        /// <param name="minLength">生成する配列の最小の長さ。Minimum length of the array to generate.</param>
        /// <param name="maxLength">生成する配列の最大の長さ。Maximum length of the array to generate.</param>
        /// <returns>ランダムな整数と長さの配列。An array of random integers and length.</returns>
        public static int[] Random(int min, int max, int minLength, int maxLength)
        {
            int[] result = new int[UnityEngine.Random.Range(minLength, maxLength)];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = UnityEngine.Random.Range(min, max);
            }
            return result;
        }
    }
}