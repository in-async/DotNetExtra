using System;
using System.Globalization;

namespace Inasync {

    /// <summary>
    /// base16 のエンコード及びデコードを行うクラス。
    /// https://tools.ietf.org/html/rfc4648#section-8
    /// </summary>
    public static class Base16 {

        /// <summary>
        /// <see cref="byte"/> 配列を base16 にエンコードします。
        /// </summary>
        /// <param name="bytes">エンコード対象の <see cref="byte"/> 配列。</param>
        /// <param name="toUpper">エンコード後の 16 進文字列を大文字にする場合は <c>true</c>、それ以外は <c>false</c>。</param>
        /// <returns>エンコード後の base16 文字列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is <c>null</c>.</exception>
        public static string Encode(byte[] bytes, bool toUpper = false) {
            if (bytes == null) { throw new ArgumentNullException(nameof(bytes)); }

            var format = toUpper ? "X2" : "x2";
            var numberFormatInfo = CultureInfo.InvariantCulture.NumberFormat;
            var chars = new Char[bytes.Length * 2];

            for (int i = 0, ci = 0; i < bytes.Length; i++, ci += 2) {
                var str = bytes[i].ToString(format, numberFormatInfo);

                chars[ci] = str[0];
                chars[ci + 1] = str[1];
            }
            return new String(chars);
        }

        /// <summary>
        /// base16 文字列を <see cref="byte"/> 配列にデコードします。
        /// </summary>
        /// <param name="hexString">base16 にエンコードされた文字列。</param>
        /// <returns>デコード後の <see cref="byte"/> 配列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hexString"/> is <c>null</c>.</exception>
        /// <exception cref="FormatException"><paramref name="hexString"/> が base16 ではありません。</exception>
        public static byte[] Decode(string hexString) {
            if (hexString == null) { throw new ArgumentNullException(nameof(hexString)); }

            if (!TryDecode(hexString, out var result)) { throw new FormatException("base16 文字列ではありません。"); }
            return result;
        }

        /// <summary>
        /// base16 文字列を <see cref="byte"/> 配列にデコードします。
        /// </summary>
        /// <param name="hexString">base16 にエンコードされた 16 進文字列。</param>
        /// <param name="result">デコード後の <see cref="byte"/> 配列。失敗した場合は <c>null</c>。</param>
        /// <returns>デコードに成功した場合は <c>true</c>、それ以外は <c>false</c>。</returns>
        public static bool TryDecode(string hexString, out byte[] result) {
            if (hexString == null) { goto Failure; }
            if (hexString.Length % 2 == 1) { goto Failure; }

            var numberFormatInfo = CultureInfo.InvariantCulture.NumberFormat;
            var bytes = new byte[hexString.Length / 2];

            for (var i = 0; i < bytes.Length; i++) {
                if (!byte.TryParse(hexString.Substring(i * 2, 2), NumberStyles.HexNumber, numberFormatInfo, out var b)) { goto Failure; }
                bytes[i] = b;
            }

            result = bytes;
            return true;

Failure:
            result = null;
            return false;
        }
    }
}
