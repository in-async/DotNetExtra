using System;
using System.Globalization;

namespace Inasync {

    /// <summary>
    /// <see cref="byte"/> 配列と 16 進文字列の相互変換を行うクラス。
    /// </summary>
    public static class Binary {

        /// <summary>
        /// <see cref="byte"/> 配列を 16 進文字列に変換します。
        /// </summary>
        /// <param name="data">変換対象の <see cref="byte"/> 配列。</param>
        /// <param name="toUpper">変換後の 16 進文字列を大文字にする場合は <c>true</c>、それ以外は <c>false</c>。</param>
        /// <returns>変換後の 16 進文字列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <c>null</c>.</exception>
        public static string ToHexString(this byte[] data, bool toUpper = false) {
            if (data == null) { throw new ArgumentNullException(nameof(data)); }

            var format = toUpper ? "X2" : "x2";
            var numberFormatInfo = CultureInfo.InvariantCulture.NumberFormat;
            var chars = new Char[data.Length * 2];

            for (int i = 0, ci = 0; i < data.Length; i++, ci += 2) {
                var str = data[i].ToString(format, numberFormatInfo);

                chars[ci] = str[0];
                chars[ci + 1] = str[1];
            }
            return new String(chars);
        }

        /// <summary>
        /// 16進文字列を <see cref="byte"/> 配列に変換します。
        /// </summary>
        /// <param name="hexString">変換対象の 16 進文字列。</param>
        /// <returns>変換後の <see cref="byte"/> 配列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hexString"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="hexString"/> の文字数が奇数の場合に投げられます。</exception>
        public static byte[] Parse(string hexString) {
            if (hexString == null) { throw new ArgumentNullException(nameof(hexString)); }
            if (hexString.Length % 2 == 1) { throw new ArgumentOutOfRangeException(nameof(hexString), hexString, "16進文字列の長さが無効です。"); }

            var bin = new byte[hexString.Length / 2];
            for (int i = 0; i < bin.Length; i++) {
                bin[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bin;
        }
    }
}
