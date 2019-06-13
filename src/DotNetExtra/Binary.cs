using System;

namespace Inasync {

    /// <summary>
    /// <see cref="byte"/> 配列と 16 進文字列の相互変換を行うクラス。
    /// </summary>
    [Obsolete]
    public static class Binary {

        /// <summary>
        /// <see cref="byte"/> 配列を 16 進文字列に変換します。
        /// </summary>
        /// <param name="data">変換対象の <see cref="byte"/> 配列。</param>
        /// <param name="toUpper">変換後の 16 進文字列を大文字にする場合は <c>true</c>、それ以外は <c>false</c>。</param>
        /// <returns>変換後の 16 進文字列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is <c>null</c>.</exception>
        public static string ToHexString(this byte[] data, bool toUpper = false) {
            return Base16.Encode(data, toUpper);
        }

        /// <summary>
        /// 16進文字列を <see cref="byte"/> 配列に変換します。
        /// </summary>
        /// <param name="hexString">変換対象の 16 進文字列。</param>
        /// <returns>変換後の <see cref="byte"/> 配列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="hexString"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="hexString"/> の文字数が奇数の場合に投げられます。</exception>
        /// <exception cref="FormatException"><paramref name="hexString"/> が 16 進文字列ではない。</exception>
        public static byte[] Parse(string hexString) {
            if (hexString == null) { throw new ArgumentNullException(nameof(hexString)); }
            if (hexString.Length % 2 == 1) { throw new ArgumentOutOfRangeException(nameof(hexString), hexString, "16進文字列の長さが偶数ではありません。"); }

            return Base16.Decode(hexString);
        }
    }
}
