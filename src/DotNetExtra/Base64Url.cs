using System;

namespace Inasync {

    /// <summary>
    /// base64url のエンコード及びデコードを行うクラス。
    /// https://tools.ietf.org/html/rfc4648#section-5
    /// </summary>
    public static class Base64Url {

        /// <summary>
        /// <see cref="byte"/> 配列を base64url にエンコードします。
        /// </summary>
        /// <param name="bin">エンコード対象の <see cref="byte"/> 配列。</param>
        /// <returns>base64url エンコード文字列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bin"/> is <c>null</c>.</exception>
        public static string Encode(byte[] bin) {
            return Convert.ToBase64String(bin)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_')
                ;
        }

        /// <summary>
        /// base64url 文字列を <see cref="byte"/> 配列にデコードします。
        /// </summary>
        /// <param name="encoded">base64url にエンコードされた文字列。</param>
        /// <returns>デコード後の <see cref="byte"/> 配列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="encoded"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException"><paramref name="encoded"/> が base64url 文字列ではありません。</exception>
        public static byte[] Decode(string encoded) {
            if (encoded == null) { throw new ArgumentNullException(nameof(encoded)); }

            if (!TryDecode(encoded, out var result)) { throw new FormatException("base64url 文字列ではありません。"); }
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

            var base64Str = encoded
                .Replace('-', '+')
                .Replace('_', '/')
                + new string('=', paddingLen);

            try {
                result = Convert.FromBase64String(base64Str);
                return true;
            }
            catch (FormatException) { goto Failure; }

Failure:
            result = null;
            return false;
        }
    }
}
