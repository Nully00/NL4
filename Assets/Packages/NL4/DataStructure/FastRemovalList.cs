using System;
using System.Collections.Generic;
using System.Linq;

namespace NL4.DataStructure
{
    public class FastRemovalList<TSource> : IEnumerable<TSource>
    {
        private List<TSource> _source;
        private HashSet<int> _reserveIndices;

        public FastRemovalList(int capacity = 0)
        {
            _source = new List<TSource>(capacity);
            _reserveIndices = new HashSet<int>();
        }

        public FastRemovalList(List<TSource> list)
        {
            _source = new List<TSource>(list);
            _reserveIndices = new HashSet<int>();
        }

        public TSource this[int index]
        {
            get
            {
                if ((uint)index >= (uint)_source.Count)
                    ThrowHelper.ArgumentOutOfRangeException();

                return _source[index];
            }
        }

        public int Count => _source.Count;

        public void Add(TSource item)
        {
            _source.Add(item);
        }

        public void Reserve(int index)
        {
            if ((uint)index >= (uint)_source.Count)
                ThrowHelper.ArgumentOutOfRangeException();

            _reserveIndices.Add(index);
        }

        public void Commit()
        {
            var sortedIndices = _reserveIndices.OrderByDescending(i => i).ToList();
            foreach (var index in sortedIndices)
            {
                SwapRemove(index);
            }
            _reserveIndices.Clear();
        }

        public void Remove(TSource item)
        {
            int index = _source.IndexOf(item);
            SwapRemove(index);
        }
        public void RemoveAt(int index)
        {
            SwapRemove(index);
        }
        private void SwapRemove(int index)
        {
            _source[index] = _source[_source.Count - 1];
            _source.RemoveAt(_source.Count - 1);
        }

        public void RemoveWhere(Func<TSource, bool> predicate)
        {
            for (int i = _source.Count - 1; i >= 0; i--)
            {
                if (predicate(_source[i]))
                {
                    SwapRemove(i);
                }
            }
        }
        public void Clear()
        {
            _source.Clear();
            _reserveIndices.Clear();
        }

        public bool Contains(TSource item)
        {
            return _source.Contains(item);
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            return _source.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _source.GetEnumerator();
        }
    }

}
