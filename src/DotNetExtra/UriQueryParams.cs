using System;
using System.Linq;

namespace DotNetExtra {

    /// <summary>
    /// URI クエリのクエリパラメーターを順序維持して管理するコレクションクラス。
    /// </summary>
    public class UriQueryParams : OrderedDictionary<string, string> {

        /// <summary>
        /// 空の <see cref="UriQueryParams"/> を作成します。
        /// </summary>
        public UriQueryParams() {
        }

        /// <summary>
        /// <see cref="Uri"/> から <see cref="UriQueryParams"/> を構築します。
        /// </summary>
        /// <param name="uri">元となる <see cref="Uri"/> オブジェクト。</param>
        /// <exception cref="ArgumentNullException"><paramref name="uri"/> is <c>null</c>.</exception>
        public UriQueryParams(Uri uri) {
            if (uri == null) { throw new ArgumentNullException(nameof(uri)); }

            var queryParams = uri.Query.TrimStart('?').Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var queryParam in queryParams) {
                var pairs = queryParam.Split(new[] { '=' }, 2);
                var key = Uri.UnescapeDataString(pairs[0]);
                var value = (pairs.Length == 1 || pairs[1] == null) ? null : Uri.UnescapeDataString(pairs[1]);

                base.Add(key, value);
            }
        }

        /// <summary>
        /// クエリパラメーターを連結してクエリ文字列を返します。
        /// その際、各パラメーターは RFC 3986 / 3987 に従いパーセントエンコードされます。
        /// </summary>
        /// <returns>パーセントエンコードされたクエリ文字列。</returns>
        public override string ToString() {
            return string.Join("&", this.Select(p => {
                var key = Uri.EscapeDataString(p.Key);
                return string.IsNullOrEmpty(p.Value) ?
                    key + '=' :
                    key + '=' + Uri.EscapeDataString(p.Value);
            }));
        }

        /// <summary>
        /// クエリパラメーターを追加します。
        /// </summary>
        /// <param name="key">追加するクエリパラメーターの名前。</param>
        /// <param name="value">追加するクエリパラメーターの値。<see cref="object.ToString"/> で文字列化されてから <see cref="UriQueryParams"/> に追加されます。</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">同じ名前を持つクエリパラメーターが既に存在します。</exception>
        public void Add(string key, object value) => base.Add(key, value?.ToString());
    }
}