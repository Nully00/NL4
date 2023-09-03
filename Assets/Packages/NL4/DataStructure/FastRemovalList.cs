using System;
using System.Collections.Generic;
using System.Linq;

namespace NL4.DataStructure
{
    public class FastRemovalList<TSource> : IEnumerable<TSource>
    {
        private List<TSource> _source;
        private List<ID> _ids;

        public FastRemovalList(int capacity = 0)
        {
            _source = new List<TSource>(capacity);
            _ids = new List<ID>(capacity);
        }

        public FastRemovalList(List<TSource> list)
        {
            _source = new List<TSource>(list);
            _ids = new List<ID>(list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                _ids.Add(new ID(i));
            }
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

        public ID Add(TSource item)
        {
            _source.Add(item);
            var id = new ID(_source.Count - 1);
            _ids.Add(id);
            return id;
        }

        public void Remove(TSource item)
        {
            int index = _source.IndexOf(item);
            if (index != -1)
            {
                SwapRemove(index);
            }
        }
        public void Remove(ID id)
        {
            SwapRemove(id.index);
        }
        public void RemoveAt(int index)
        {
            SwapRemove(index);
        }
        private void SwapRemove(int removedindex)
        {
            if ((uint)removedindex >= (uint)_source.Count)
                ThrowHelper.ArgumentOutOfRangeException();

            if (removedindex != _source.Count - 1)
            {
                var swappedItemId = _source.Count - 1;

                _source[removedindex] = _source[swappedItemId];
                _ids[swappedItemId].index = removedindex;

                (_ids[removedindex], _ids[swappedItemId]) = (_ids[swappedItemId], _ids[removedindex]);
            }

            _source.RemoveAt(_source.Count - 1);
            _ids.RemoveAt(_ids.Count - 1);
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
            _ids.Clear();
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


        public class ID
        {
            internal ID(int index)
            {
                this.index = index;
            }

            public int index { get; internal set; }
        }
    }

}
