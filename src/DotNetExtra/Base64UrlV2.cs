using System;
using System.Text;

namespace Inasync {

    /// <summary>
    /// base64url のエンコード及びデコードを行うクラス。
    /// https://tools.ietf.org/html/rfc4648#page-7
    /// </summary>
    public static class Base64UrlV2 {

        /// <summary>
        /// <see cref="byte"/> 配列を base64url にエンコードします。
        /// </summary>
        /// <param name="bin">エンコード対象の <see cref="byte"/> 配列。</param>
        /// <returns>base64url エンコード文字列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bin"/> is <c>null</c>.</exception>
        public static string Encode(byte[] bin) {
            var base64 = Convert.ToBase64String(bin);
            var bldr = new StringBuilder(base64)
                .Replace('+', '-')
                .Replace('/', '_')
                ;

            var padIndex = base64.IndexOf('=');
            if (padIndex >= 0) {
                bldr.Remove(padIndex, bldr.Length - padIndex);
            }

            return bldr.ToString();
        }

        /// <summary>
        /// base64url でエンコードされた文字列をデコードします。
        /// </summary>
        /// <param name="encoded">base64url エンコードされた文字列。</param>
        /// <returns>デコード後の <see cref="byte"/> 配列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="encoded"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException"><paramref name="encoded"/> が base64url エンコード文字列ではありません。</exception>
        public static byte[] Decode(string encoded) {
            if (encoded == null) { throw new ArgumentNullException(nameof(encoded)); }

            if (!TryDecode(encoded, out var result)) { throw new FormatException("base64url でエンコードされた文字列ではありません。"); }
            return result;
        }

        /// <summary>
        /// base64url でエンコードされた文字列をデコードします。
        /// </summary>
        /// <param name="encoded">base64url エンコードされた文字列。</param>
        /// <param name="result">デコード後の <see cref="byte"/> 配列。失敗した場合は <c>null</c>。</param>
        /// <returns>デコードに成功した場合は <c>true</c>、それ以外は <c>false</c>。</returns>
        public static bool TryDecode(string encoded, out byte[] result) {
            if (encoded == null) { goto Failure; }

            var paddingLen = encoded.Length % 4;
            if (paddingLen != 0) {
                paddingLen = 4 - paddingLen;
            }

            var bldr = new StringBuilder(encoded)
                .Replace('-', '+')
                .Replace('_', '/')
                .Append('=', paddingLen);

            try {
                result = Convert.FromBase64String(bldr.ToString());
                return true;
            }
            catch (FormatException) { goto Failure; }

Failure:
            result = null;
            return false;
        }
    }
}
