using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NL4.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// 指定したインデックスの要素を効率的にリストから削除します。
        /// Efficiently removes an element at the specified index from the list.
        /// </summary>
        /// <remarks>
        /// このメソッドは、指定されたインデックスの要素とリストの最後の要素を交換した後、最後の要素を削除することで、要素の削除を効率的に行います。
        /// This method works by swapping the element at the specified index with the last element in the list, and then removing the last element, making the removal operation efficient.
        /// </remarks>
        /// <param name="list">操作対象のリスト。The list to operate on.</param>
        /// <param name="index">削除する要素のインデックス。The index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">指定されたインデックスがリストの範囲外の場合。Thrown when the specified index is out of the range of the list.</exception>
        public static void RemoveAtEfficiently<T>(this List<T> list, int index)
        {
            if ((uint)index >= (uint)list.Count)
            {
                ThrowHelper.ArgumentOutOfRangeException();
            }
            int lastIdx = list.Count - 1;
            list[index] = list[lastIdx];
            list.RemoveAt(lastIdx);
        }
        private static Random s_rng = new Random();

        /// <summary>
        /// 指定されたリスト内の要素をシャッフルします。
        /// Shuffles the elements within the specified list.
        /// </summary>
        /// <typeparam name="T">リストの要素の型。The type of the elements in the list.</typeparam>
        /// <param name="list">シャッフルするリスト。The list to shuffle.</param>
        /// <remarks>
        /// このメソッドは、Fisher-Yates (または Durstenfeld) アルゴリズムを使用してリストを効率的にシャッフルします。
        /// This method uses the Fisher-Yates (or Durstenfeld) algorithm to efficiently shuffle the list.
        /// </remarks>
        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = s_rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}