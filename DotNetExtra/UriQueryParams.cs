using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotNetExtra {

    /// <summary>
    /// URI クエリのクエリパラメーターを順序維持して管理するコレクションクラス。
    /// </summary>
    public class UriQueryParams : IDictionary<string, string> {
        private readonly List<KeyValuePair<string, string>> _params;
        private readonly Dictionary<string, int> _keyIndexes;

        /// <summary>
        /// 空の <see cref="UriQueryParams"/> を作成します。
        /// </summary>
        public UriQueryParams() {
            _params = new List<KeyValuePair<string, string>>();
            _keyIndexes = new Dictionary<string, int>();
        }

        /// <summary>
        /// <see cref="Uri"/> から <see cref="UriQueryParams"/> を構築します。
        /// </summary>
        /// <param name="uri">元となる <see cref="Uri"/> オブジェクト。</param>
        /// <exception cref="ArgumentNullException"><paramref name="uri"/> is <c>null</c>.</exception>
        public UriQueryParams(Uri uri) {
            if (uri == null) { throw new ArgumentNullException(nameof(uri)); }

            _params = uri.Query.TrimStart('?')
                .Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => {
                    var pairs = p.Split(new[] { '=' }, 2);
                    var key = Uri.UnescapeDataString(pairs[0]);
                    var value = (pairs[1] == null) ? null : Uri.UnescapeDataString(pairs[1]);
                    return new KeyValuePair<string, string>(key, value);
                })
                .ToList();

            _keyIndexes = new Dictionary<string, int>(_params.Count);
            for (var i = 0; i < _params.Count; i++) {
                _keyIndexes.Add(_params[i].Key, i);
            }
        }

        /// <summary>
        /// 指定したクエリ名に関連付けられている値を取得または設定します。
        /// </summary>
        /// <param name="key">取得または設定する値のクエリ名。</param>
        /// <returns><paramref name="key"/> に関連付けられている値。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="KeyNotFoundException">コレクション内に <paramref name="key"/> が存在しません。</exception>
        public string this[string key] {
            get => _params[_keyIndexes[key]].Value;
            set {
                if (!_keyIndexes.TryGetValue(key, out var index)) {
                    Add(key, value);
                }
                else {
                    _params[index] = new KeyValuePair<string, string>(key, value);
                }
            }
        }

        /// <summary>
        /// <see cref="UriQueryParams"/> に格納されているクエリ名のコレクションを取得します。
        /// </summary>
        public ICollection<string> Keys => _params.Select(p => p.Key).ToArray();

        /// <summary>
        /// <see cref="UriQueryParams"/> に格納されている値のコレクションを取得します。
        /// </summary>
        public ICollection<string> Values => _params.Select(p => p.Value).ToArray();

        /// <summary>
        /// <see cref="UriQueryParams"/> に格納されているクエリパラメーターの数を取得します。
        /// </summary>
        public int Count => _params.Count;

        bool ICollection<KeyValuePair<string, string>>.IsReadOnly => false;

        /// <summary>
        /// クエリパラメーターを連結してクエリ文字列を返します。
        /// その際、各パラメーターは RFC 3986 / 3987 に従いパーセントエンコードされます。
        /// </summary>
        /// <returns>パーセントエンコードされたクエリ文字列。</returns>
        public override string ToString() {
            return string.Join("&", _params.Select(p => {
                var key = Uri.EscapeDataString(p.Key);
                var value = (p.Value == null) ? null : Uri.EscapeDataString(p.Value);

                return key + '=' + value;
            }));
        }

        /// <summary>
        /// クエリパラメーターを追加します。
        /// </summary>
        /// <param name="key">追加するクエリパラメーターの名前。</param>
        /// <param name="value">追加するクエリパラメーターの値。<see cref="object.ToString"/> で文字列化されてから <see cref="UriQueryParams"/> に追加されます。</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">同じ名前を持つクエリパラメーターが既に存在します。</exception>
        public void Add(string key, object value) => Add(key, value?.ToString());

        /// <summary>
        /// クエリパラメーターを追加します。
        /// </summary>
        /// <param name="key">追加するクエリパラメーターの名前。</param>
        /// <param name="value">追加するクエリパラメーターの値。</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">同じ名前を持つクエリパラメーターが既に存在します。</exception>
        public void Add(string key, string value) => Add(new KeyValuePair<string, string>(key, value));

        /// <summary>
        /// クエリパラメーターを追加します。
        /// </summary>
        /// <param name="item">追加するクエリパラメーター。</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">同じ名前を持つクエリパラメーターが既に存在します。</exception>
        public void Add(KeyValuePair<string, string> item) {
            _keyIndexes.Add(item.Key, _params.Count);
            _params.Add(item);
        }

        /// <summary>
        /// 指定したクエリ名に関連付けられているクエリパラメーターを削除します。
        /// </summary>
        /// <param name="key">削除するクエリパラメーターの名前。</param>
        /// <returns>クエリパラメーターが見つかり、正常に削除された場合は <c>true</c>。それ以外なら <c>false</c>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        public bool Remove(string key) {
            if (!_keyIndexes.TryGetValue(key, out var index)) { return false; }

            _params.RemoveAt(index);
            _keyIndexes.Remove(key);
            return true;
        }

        /// <summary>
        /// 指定したクエリパラメーターを削除します。
        /// </summary>
        /// <param name="item">削除するクエリパラメーター。</param>
        /// <returns>クエリパラメーターが見つかり、正常に削除された場合は <c>true</c>。それ以外なら <c>false</c>。</returns>
        public bool Remove(KeyValuePair<string, string> item) {
            if (!_params.Remove(item)) { return false; }

            _keyIndexes.Remove(item.Key);
            return true;
        }

        /// <summary>
        /// <see cref="UriQueryParams"/> から全てのクエリパラメーターを削除します。
        /// </summary>
        public void Clear() {
            _keyIndexes.Clear();
            _params.Clear();
        }

        /// <summary>
        /// 指定したクエリパラメーターが <see cref="UriQueryParams"/> に格納されているかどうかを判断します。
        /// </summary>
        /// <param name="item"><see cref="UriQueryParams"/> 内で検索されるクエリパラメーター。</param>
        /// <returns><paramref name="item"/> が <see cref="UriQueryParams"/> に格納されている場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        public bool Contains(KeyValuePair<string, string> item) => _params.Contains(item);

        /// <summary>
        /// 指定した名前を持つクエリパラメーターが <see cref="UriQueryParams"/> に格納されているかどうかを判断します。
        /// </summary>
        /// <param name="key"><see cref="UriQueryParams"/> 内で検索されるクエリ名。</param>
        /// <returns><paramref name="key"/> を持つクエリパラメーターが <see cref="UriQueryParams"/> に格納されている場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        public bool ContainsKey(string key) => _keyIndexes.ContainsKey(key);

        /// <summary>
        /// 指定したクエリ名に関連付けられているクエリパラメーターの値を取得します。
        /// </summary>
        /// <param name="key">取得する値のクエリ名。</param>
        /// <param name="value"><paramref name="key"/> に関連付けられているクエリパラメーターの値。クエリパラメーターが見つからなかった場合は <c>null</c>。</param>
        /// <returns><paramref name="key"/> を持つクエリパラメーターが <see cref="UriQueryParams"/> に格納されている場合は <c>true</c>。それ以外の場合は <c>false</c>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        public bool TryGetValue(string key, out string value) {
            if (!_keyIndexes.TryGetValue(key, out var index)) {
                value = null;
                return false;
            }

            value = _params[index].Value;
            return true;
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => _params.CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => _params.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _params.GetEnumerator();
    }
}