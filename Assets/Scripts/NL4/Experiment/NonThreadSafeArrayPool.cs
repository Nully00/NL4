using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NL4.Experiment
{
    /// <summary>
    /// 実験的に作成
    /// </summary>
    public class NonThreadSafeArrayPool<T>
    {
        private const int s_maxArrayCount = 4;
        private bool _renting = false;
        private T[][] _arrays = new T[s_maxArrayCount][];
        private int[] _lengths = new int[s_maxArrayCount];

        public int GetLength(int id = 0)
        {
            return _lengths[id];
        }

        public Span<T> Rent(int length, int id = 0)
        {
            if (_renting)
                ThrowHelper_No_Resources_Have_Been_Released();
            if (id >= s_maxArrayCount)
                ThrowHelper_No_More_Than_MaxArrayCount_Can_Be_Secured();


            AllocateArray(length, id);

            _renting = true;
            _lengths[id] = length;
            return _arrays[id].AsSpan(0, length);
        }

        public void Return()
        {
            _renting = false;
        }
        /// <summary>
        /// すべてのリソースが返却されているかどうか
        /// </summary>
        /// <returns></returns>
        public bool IsAllReturn()
        {
            return !_renting;
        }
        #region RentInit
        /// <summary>
        /// GCを回避しながら初期化をし配列をレンタルする。
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Span<T> RentInitNonAlloc(T a, int id)
        {
            var array = Rent(1, id);
            array[0] = a;
            return array;
        }

        public Span<T> RentInitNonAlloc(T a, T b, int id)
        {
            var array = Rent(2, id);
            array[0] = a;
            array[1] = b;
            return array;
        }

        public Span<T> RentInitNonAlloc(T a, T b, T c, int id)
        {
            var array = Rent(3, id);
            array[0] = a;
            array[1] = b;
            array[2] = c;
            return array;
        }

        public Span<T> RentInitNonAlloc(T a, T b, T c, T d, int id)
        {
            var array = Rent(4, id);
            array[0] = a;
            array[1] = b;
            array[2] = c;
            array[3] = d;
            return array;
        }

        public Span<T> RentInitNonAlloc(T a, T b, T c, T d, T e, int id)
        {
            var array = Rent(5, id);
            array[0] = a;
            array[1] = b;
            array[2] = c;
            array[3] = d;
            array[4] = e;
            return array;
        }

        public Span<T> RentInitNonAlloc(T a, T b, T c, T d, T e, T f, int id)
        {
            var array = Rent(6, id);
            array[0] = a;
            array[1] = b;
            array[2] = c;
            array[3] = d;
            array[4] = e;
            array[5] = f;
            return array;
        }

        public Span<T> RentInitNonAlloc(T a, T b, T c, T d, T e, T f, T g, int id)
        {
            var array = Rent(7, id);
            array[0] = a;
            array[1] = b;
            array[2] = c;
            array[3] = d;
            array[4] = e;
            array[5] = f;
            array[6] = g;
            return array;
        }

        public Span<T> RentInitNonAlloc(T a, T b, T c, T d, T e, T f, T g, T h, int id)
        {
            var array = Rent(8, id);
            array[0] = a;
            array[1] = b;
            array[2] = c;
            array[3] = d;
            array[4] = e;
            array[5] = f;
            array[6] = g;
            array[7] = h;
            return array;
        }

        public Span<T> RentInitNonAlloc(T a, T b, T c, T d, T e, T f, T g, T h, T i, int id)
        {
            var array = Rent(9, id);
            array[0] = a;
            array[1] = b;
            array[2] = c;
            array[3] = d;
            array[4] = e;
            array[5] = f;
            array[6] = g;
            array[7] = h;
            array[8] = i;
            return array;
        }

        public Span<T> RentInitNonAlloc(T a, T b, T c, T d, T e, T f, T g, T h, T i, T j, int id)
        {
            var array = Rent(10, id);
            array[0] = a;
            array[1] = b;
            array[2] = c;
            array[3] = d;
            array[4] = e;
            array[5] = f;
            array[6] = g;
            array[7] = h;
            array[8] = i;
            array[9] = j;
            return array;
        }
        public Span<T> RentInit(T[] items, int id = 0)
        {
            var array = Rent(items.Length, id);
            for (int i = 0; i < items.Length; i++)
            {
                array[i] = items[i];
            }
            return array;
        }
        #endregion
        public Span<T> RentSelect(Span<T> source, Func<T, T> selector, int id = 0)
        {
            var span = Rent(source.Length, id);
            for (int i = 0; i < source.Length; i++)
            {
                span[i] = selector(source[i]);
            }
            return span;
        }
        public Span<T> RentWhere(Span<T> source, Func<T, bool> predicate, int id = 0)
        {
            int count = 0;
            AllocateArray(source.Length, id);
            for (int i = 0; i < source.Length; i++)
            {
                if (predicate(source[i]))
                {
                    _arrays[id][count] = source[i];
                    count++;
                }
            }

            return Rent(count, id);
        }
        /// <summary>
        /// 既存の確保された領域の内部状態を直接変更します。
        /// </summary>
        /// <returns></returns>
        public Span<T> SelectModify(Func<T, T> selector, int id = 0)
        {
            Span<T> span = _arrays[id].AsSpan(0, GetLength(id));
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = selector(span[i]);
            }
            return span;
        }
        /// <summary>
        /// 既存の確保された領域の内部状態を直接変更します。
        /// Lengthの長さも変更されます。
        /// </summary>
        public Span<T> WhereModify(Func<T, bool> predicate, int id = 0)
        {
            Span<T> span = _arrays[id].AsSpan(0, GetLength(id));
            int count = 0;
            for (int i = 0; i < span.Length; i++)
            {
                if (predicate(span[i]))
                {
                    span[count] = span[i];
                    count++;
                }
            }
            ChangeLength(count, id);
            return span;
        }

        public void Sort(int id = 0)
        {
            Array.Sort(_arrays[id], 0, GetLength(id));
        }

        public void Reverse(int id = 0)
        {
            Array.Reverse(_arrays[id], 0, GetLength(id));
        }

        public void DebugLog(int id = 0)
        {
            Span<T> span = _arrays[id].AsSpan(0, GetLength(id));
            string text = "";
            for (int i = 0; i < span.Length; i++)
            {
                text += $"{span[i]},";
            }
            Debug.Log(text);
        }

        private void ThrowHelper_No_Resources_Have_Been_Released()
        {
            throw new InvalidOperationException("リソースを返却していません。");
        }
        private void ThrowHelper_No_More_Than_MaxArrayCount_Can_Be_Secured()
        {
            throw new InvalidOperationException("MaxArrayCount以上の配列を確保することはできません。");
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void AllocateArray(int length, int id)
        {
            if (_arrays[id] == null || GetAllocateSize(id) < length)
            {
                _arrays[id] = new T[length];
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void ChangeLength(int length, int id)
        {
            _lengths[id] = length;
        }
        internal int GetAllocateSize(int id = 0)
        {
            return _arrays[id].Length;
        }
    }
    public static class NonThreadSafeArrayPoolEx
    {
        public static Span<int> RentRandom(this NonThreadSafeArrayPool<int> pool, int count, int min, int max, int id = 0)
        {
            var span = pool.Rent(count, id);
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = Random.Range(min, max);
            }
            return span;
        }
        public static Span<float> RentRandom(this NonThreadSafeArrayPool<float> pool, int count, float min, float max, int id = 0)
        {
            var span = pool.Rent(count, id);
            for (int i = 0; i < span.Length; i++)
            {
                span[i] = Random.Range(min, max);
            }
            return span;
        }
    }

}
