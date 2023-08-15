using System;
using System.Collections;
using System.Collections.Generic;
namespace NL4.DataStructure
{
    /// <summary>
    /// EfficientStackedStorageは一時的な期間において頻繁にデータが追加・削除される状況で高い効率を発揮するデータ構造です。
    /// データは内部のリストに格納され、各データは一意のIDによりアクセス可能です。
    /// データが取得されると、そのデータのIDは再利用のためにスタックにプッシュされ、次のデータの追加時に再利用されます。
    /// これにより、新たなメモリの確保を最小限に抑えつつ、データの追加と取得を効率的に行うことができます。
    /// 
    ///
    /// The EfficientStackedStorage is a data structure that exhibits high efficiency in situations where data is frequently added and removed. 
    /// Data is stored in an internal list, and each piece of data can be accessed by a unique ID. 
    /// When data is retrieved, the ID of that data is pushed onto a stack for reuse, and is reused the next time data is added. 
    /// This allows for efficient addition and retrieval of data, while minimizing the allocation of new memory.
    /// 
    /// 注意：スレッドセーフではありません。順番は保証されません。
    /// </summary>
    public class EfficientStackedStorage<T> : IEnumerable<T>
    {
        private Stack<int> _availableIds = new Stack<int>();
        private List<T> _items = new List<T>();
        public int Count
        {
            get
            {
                return _items.Count - _availableIds.Count;
            }
        }
        /// <summary>
        /// デフォルトのコンストラクタ。
        /// Default constructor.
        /// </summary>
        public EfficientStackedStorage() { }
        /// <summary>
        /// 初期容量を指定してEfficientRecyclingListを生成します。
        /// Creates an EfficientRecyclingList with a specified initial capacity.
        /// </summary>
        /// <param name="capacity">初期容量。The initial capacity.</param>
        public EfficientStackedStorage(int capacity)
        {
            _items = new List<T>(capacity);
            _availableIds = new Stack<int>(capacity);
        }
        /// <summary>
        /// アイテムをデポジットし、アイテムに対応する一意のIDを返します。
        /// Deposits an item and returns a unique ID corresponding to the item.
        /// </summary>
        /// <param name="item">追加するアイテム。The item to add.</param>
        /// <returns>アイテムに対応する一意のID。A unique ID corresponding to the item.</returns>
        public int Deposit(T item)
        {
            int id = 0;
            if (_availableIds.TryPop(out id))
            {
                _items[id] = item;
            }
            else
            {
                _items.Add(item);
                id = _items.Count - 1;
            }
            return id;
        }
        /// <summary>
        /// 一意のIDを指定して対応するアイテムを取得します。
        /// Retrieves the item corresponding to the specified unique ID.
        /// </summary>
        /// <param name="id">アイテムに対応する一意のID。A unique ID corresponding to the item.</param>
        /// <returns>一意のIDに対応するアイテム。The item corresponding to the unique ID.</returns>
        public T Retrieve(int id)
        {
            ValidateId(id);
            return _items[id];
        }
        /// <summary>
        /// 一意のIDを指定して対応するアイテムをリリースします。この操作により、IDは再利用可能となります。
        /// Releases the item corresponding to the specified unique ID. This operation makes the ID available for reuse.
        /// </summary>
        /// <param name="id">リリースするアイテムに対応する一意のID。A unique ID corresponding to the item to be released.</param>
        public void Release(int id)
        {
            ValidateId(id);
            _availableIds.Push(id);
            _items[id] = default;
        }

        public void AllRelease()
        {
            SortedSet<int> availableIds = new SortedSet<int>(_availableIds);
            for (int i = 0; i < _items.Count; i++)
            {
                if (availableIds.Contains(i))
                    continue;

                Release(i);
            }
        }
        public void Clear()
        {
            _items.Clear();
            _availableIds.Clear();
        }

        /// <summary>
        /// IDが有効な範囲内であることを確認します。範囲外の場合、ArgumentOutOfRangeExceptionがスローされます。
        /// Validates that the ID is within a valid range. If it is out of range, an ArgumentOutOfRangeException is thrown.
        /// </summary>
        /// <param name="id">検証するID。The ID to validate.</param>
        private void ValidateId(int id)
        {
            if (id < 0 || id >= _items.Count)
                throw new ArgumentOutOfRangeException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            SortedSet<int> availableIds = new SortedSet<int>(_availableIds);
            for (int i = 0; i < _items.Count; i++)
            {
                if (availableIds.Contains(i))
                {
                    continue;
                }

                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}