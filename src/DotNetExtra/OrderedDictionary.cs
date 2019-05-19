using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Inasync {

    /// <summary>
    /// エントリー順に順序維持される辞書クラス。
    /// </summary>
    /// <typeparam name="TKey">辞書のキーの型。</typeparam>
    /// <typeparam name="TValue">辞書の値の型。</typeparam>
    public class OrderedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue> {
        private readonly Dictionary<TKey, TValue> _params;
        private readonly List<TKey> _keys;

        /// <summary>
        /// 空の <see cref="OrderedDictionary{TKey, TValue}"/> を作成します。
        /// </summary>
        public OrderedDictionary() : this(null) {
        }

        /// <summary>
        /// 指定されたキー比較子を使用する <see cref="OrderedDictionary{TKey, TValue}"/> を作成します。
        /// </summary>
        /// <param name="comparer">辞書キーの比較に使用される <see cref="IEqualityComparer{T}"/>。</param>
        public OrderedDictionary(IEqualityComparer<TKey> comparer) {
            if (comparer == null) {
                comparer = EqualityComparer<TKey>.Default;
            }

            _keys = new List<TKey>();
            _params = new Dictionary<TKey, TValue>(comparer);
        }

        public TValue this[TKey key] {
            get => _params[key];
            set {
                if (!_params.ContainsKey(key)) {
                    Add(key, value);
                }
                else {
                    _params[key] = value;
                }
            }
        }

        public ICollection<TKey> Keys => _keys;

        public ICollection<TValue> Values => _keys.Select(key => _params[key]).ToArray();

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => _keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => _keys.Select(key => _params[key]);

        public int Count => _keys.Count;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

        /// <summary>
        /// 辞書キーの比較に使用される <see cref="IEqualityComparer{T}"/>。
        /// </summary>
        public IEqualityComparer<TKey> Comparer { get => _params.Comparer; }

        public void Add(TKey key, TValue value) {
            if (key == null) { throw new ArgumentNullException(nameof(key)); }

            _params.Add(key, value);
            _keys.Add(key);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) {
            if (item.Key == null) { throw new ArgumentException("Key プロパティが null です。", nameof(item)); }

            ((ICollection<KeyValuePair<TKey, TValue>>)_params).Add(item);
            _keys.Add(item.Key);
        }

        public bool Remove(TKey key) {
            if (!_params.Remove(key)) { return false; }
            _keys.Remove(key);
            return true;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item) {
            if (!((ICollection<KeyValuePair<TKey, TValue>>)_params).Remove(item)) { return false; }
            _keys.Remove(item.Key);
            return true;
        }

        public void Clear() {
            _params.Clear();
            _keys.Clear();
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item) => ((ICollection<KeyValuePair<TKey, TValue>>)_params).Contains(item);

        public bool ContainsKey(TKey key) => _params.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value) => _params.TryGetValue(key, out value);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((ICollection<KeyValuePair<TKey, TValue>>)_params).CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _keys.Select(key => new KeyValuePair<TKey, TValue>(key, _params[key])).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
