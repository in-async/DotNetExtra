﻿using System;
using System.Diagnostics;

namespace Inasync {

    /// <summary>
    /// base64url のエンコード及びデコードを行うクラス。
    /// https://tools.ietf.org/html/rfc4648#section-5
    /// </summary>
    public static class Base64Url {

        /// <summary>
        /// <see cref="byte"/> 配列を base64url にエンコードします。
        /// パディングは省略します。
        /// </summary>
        /// <param name="bytes">エンコード対象の <see cref="byte"/> 配列。</param>
        /// <returns>base64url エンコード文字列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is <c>null</c>.</exception>
        /// <remarks>v2.0 で削除予定 (cf. c391dac2631c061ac36f0eb2736c8e1b83bf4e6e)</remarks>
        public static string Encode(byte[] bytes) => Encode(bytes, padding: false);

        /// <summary>
        /// <see cref="byte"/> 配列を base64url にエンコードします。
        /// </summary>
        /// <param name="bytes">エンコード対象の <see cref="byte"/> 配列。</param>
        /// <param name="padding">パディングをする場合は <c>true</c>、それ以外は <c>false</c>。既定値は <c>false</c>。</param>
        /// <returns>base64url エンコード文字列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is <c>null</c>.</exception>
        public static string Encode(byte[] bytes, bool padding = false) {
            if (bytes == null) { throw new ArgumentNullException(nameof(bytes)); }

            return Encode(new ArraySegment<byte>(bytes), padding);
        }

        /// <summary>
        /// <see cref="byte"/> 配列を base64url にエンコードします。
        /// </summary>
        /// <param name="bytes">エンコード対象の <see cref="byte"/> 配列。</param>
        /// <param name="offset">エンコードの開始位置を示すオフセット。</param>
        /// <param name="length">エンコード対象の要素の数。</param>
        /// <param name="padding">パディングをする場合は <c>true</c>、それ以外は <c>false</c>。既定値は <c>false</c>。</param>
        /// <returns>base64url エンコード文字列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> または <paramref name="length"/> が負の値です。
        /// または <paramref name="offset"/> と <paramref name="length"/> を加算した値が <paramref name="bytes"/> の長さを超えています。
        /// </exception>
        [Obsolete]
        public static string Encode(byte[] bytes, int offset, int length, bool padding = false) {
            if (bytes == null) { throw new ArgumentNullException(nameof(bytes)); }
            if (offset < 0) { throw new ArgumentOutOfRangeException(nameof(offset), offset, $"{nameof(offset)} が負の値です。"); }
            if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length), length, $"{nameof(length)} が負の値です。"); }
            if (offset + length > bytes.Length) { throw new ArgumentOutOfRangeException(message: $"{nameof(offset)} と ${nameof(length)} の和が ${nameof(bytes)} の長さを超えています。", innerException: null); }

            return Encode(new ArraySegment<byte>(bytes, offset, length), padding);
        }

        /// <summary>
        /// <see cref="byte"/> 配列を base64url にエンコードします。
        /// </summary>
        /// <param name="bytes">エンコード対象の <see cref="byte"/> 配列。</param>
        /// <param name="padding">パディングをする場合は <c>true</c>、それ以外は <c>false</c>。既定値は <c>false</c>。</param>
        /// <returns>base64url エンコード文字列。</returns>
        public static string Encode(ArraySegment<byte> bytes, bool padding = false) {
            if (bytes.Count == 0) { return ""; }
            Debug.Assert(bytes != default);

            var encoded = Convert.ToBase64String(bytes.Array, bytes.Offset, bytes.Count);
            Debug.Assert(encoded.Length > 0);

            if (!padding) {
                encoded = encoded.TrimEnd('=');
            }

            return encoded
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

            var paddingLen = unchecked(~encoded.Length + 1) & 0b11;

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
