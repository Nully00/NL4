using System;
using System.Collections.Generic;
using UnityEngine;

namespace NL4.DataStructure
{
    /// <summary>
    /// リストを循環的にアクセスするためのクラスです。
    /// A class for accessing a list in a circular manner.
    /// </summary>
    public class NLCircularList<T>
    {
        private List<T> _source;
        private NLCircular _circular;
        /// <summary>
        /// ソースリストと最初のインデックスを指定して新しいインスタンスを初期化します。
        /// Initializes a new instance with the specified source list and an optional starting index.
        /// </summary>
        /// <param name="source">循環リストの元となるリスト。The source list for the circular list.</param>
        /// <param name="firstIndex">開始インデックス。Starting index.</param>
        public NLCircularList(List<T> source, int firstIndex = 0)
        {
            this._source = new List<T>(source);
            _circular = new NLCircular(firstIndex, 0, _source.Count - 1);
        }
        /// <summary>
        /// 現在の要素を取得します。
        /// Gets the current element.
        /// </summary>
        public T Current()
        {
            return _source[_circular.Current()];
        }
        /// <summary>
        /// 次の要素を取得します。
        /// Gets the next element.
        public T Next()
        {
            return _source[_circular.Next()];
        }
        /// <summary>
        /// 前の要素を取得します。
        /// Gets the previous element.
        /// </summary>
        public T Prev()
        {
            return _source[_circular.Prev()];
        }
        /// <summary>
        /// 指定されたオフセットの要素を取得します。
        /// Gets the element at the specified offset.
        /// </summary>
        /// <param name="offset">オフセット。Offset.</param>
        public T Offset(int offset)
        {
            return _source[_circular.Offset(offset)];
        }
    }
    /// <summary>
    /// 循環的なインデックスを提供するクラスです。
    /// A class that provides a circular index.
    /// </summary>
    public class NLCircular
    {
        /// <summary>
        /// 指定された初期インデックス、最小インデックス、および最大インデックスで新しいインスタンスを初期化します。
        /// Initializes a new instance with the specified starting index, minimum index, and maximum index.
        /// </summary>
        /// <param name="first">初期インデックス。Starting index.</param>
        /// <param name="min">最小インデックス。Minimum index.</param>
        /// <param name="max">最大インデックス。Maximum index.</param>
        public NLCircular(int first, int min, int max)
        {
            this.currentIndex = first;
            this.max = max;
            this.min = min;
        }

        public int currentIndex { get; private set; }
        public int min { get; private set; }
        public int max { get; private set; }
        /// <summary>
        /// 現在のインデックスを取得します。
        /// Gets the current index.
        /// </summary>
        public int Current()
        {
            return currentIndex;
        }
        /// <summary>
        /// 次のインデックスを取得します。
        /// Gets the next index.
        /// </summary>
        public int Next()
        {
            currentIndex = Next_Internal(currentIndex);
            return currentIndex;
        }
        /// <summary>
        /// 前のインデックスを取得します。
        /// Gets the previous index.
        /// </summary>
        public int Prev()
        {
            currentIndex = Prev_Internal(currentIndex);
            return currentIndex;
        }
        /// <summary>
        /// 指定されたオフセットのインデックスを取得します。
        /// Gets the index at the specified offset.
        /// </summary>
        /// <param name="offset">オフセット。Offset.</param>
        public int Offset(int offset)
        {
            if (offset == 0) return Current();
            int offsetAbs = Mathf.Abs(offset);
            int tempCurrent = currentIndex;
            Func<int, int> func = (offset > 0) ? Next_Internal : Prev_Internal;

            for (int i = 0; i < offsetAbs; i++)
            {
                tempCurrent = func(tempCurrent);
            }
            return tempCurrent;
        }
        private int Next_Internal(int current)
        {
            current = (max == current) ? min : current + 1;
            return current;
        }
        private int Prev_Internal(int current)
        {
            current = (min == current) ? max : current - 1;
            return current;
        }
    }
}
