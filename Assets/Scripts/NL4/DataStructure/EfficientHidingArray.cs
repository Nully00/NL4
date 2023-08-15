using System.Collections.Generic;
using NL4.Extensions;

namespace NL4.DataStructure
{
    /// <summary>
    /// DynamicHideListは、データの一時的な隠蔽(無効)を容易にするためのデータ構造を提供します。
    /// このクラスは、内部のリストでデータを管理し、隠蔽する要素のインデックスを効率的に操作することができます。
    /// 要素の隠蔽は、元の配列の要素を変更せずに、単に一時的に取得不可能にする操作であり、
    /// データの一時的な非表示が必要なシナリオで使用されます。
    /// 
    /// Efficiently manages elements in an array that may be hidden.
    /// 
    /// 注意：スレッドセーフではありません。順番は保証されません。
    /// </summary>
    /// <typeparam name="TSource">配列の要素の型。The type of the elements in the array.</typeparam>
    public class EfficientHidingArray<TSource>
    {
        private TSource[] source { get; set; }
        private int[] readyIndices { get; set; }
        private int readyIndicesLength { get; set; }
        private List<int> reserveIndices { get; set; }
        /// <summary>
        /// 配列の有効な要素数を取得します。
        /// Gets the number of visible elements in the array.
        /// </summary>
        public int Length
        {
            get
            {
                return readyIndicesLength;
            }
        }
        /// <summary>
        /// 配列を指定して新しいインスタンスを初期化します。
        /// Initializes a new instance with the specified array.
        /// </summary>
        public EfficientHidingArray(TSource[] source)
        {
            this.source = source;
            readyIndices = GenerateArray.Range(source.Length);
            readyIndicesLength = readyIndices.Length;
            reserveIndices = new List<int>(readyIndicesLength);
        }
        /// <summary>
        /// 指定したインデックスの要素を取得します。
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">要素のインデックス。The index of the element.</param>
        /// <returns>指定したインデックスの要素。The element at the specified index.</returns>
        public TSource this[int index]
        {
            get
            {
                if ((uint)index >= (uint)readyIndices.Length)
                    ThrowHelper.ArgumentOutOfRangeException();

                return source[readyIndices[index]];
            }
        }
        /// <summary>
        /// 指定したインデックスの要素を無効にする予約をします。
        /// Reserves to hide the element at the specified index.
        /// </summary>
        /// <param name="index">無効にする要素のインデックス。The index of the element to hide.</param>
        public void ReserveHideAtIndex(int index)
        {
            reserveIndices.Add(index);
        }
        /// <summary>
        /// 予約された要素を無効にします。
        /// Hides the elements that have been reserved.
        /// </summary>
        public void Hide()
        {
            for (int i = 0; i < reserveIndices.Count; i++)
            {
                SwapAndHide(reserveIndices[i]);
            }

            reserveIndices.Clear();
        }
        private void SwapAndHide(int index)
        {
            if ((uint)index >= (uint)readyIndices.Length)
                ThrowHelper.ArgumentOutOfRangeException();

            (readyIndices[index], readyIndices[readyIndicesLength - 1])
                = (readyIndices[readyIndicesLength - 1], readyIndices[index]);
            readyIndicesLength--;
        }
    }
}