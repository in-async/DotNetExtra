using System;
using System.Diagnostics;

namespace Inasync {

    /// <summary>
    /// base64url のエンコード及びデコードを行うクラス。
    /// https://tools.ietf.org/html/rfc4648#page-7
    /// </summary>
    public static class Base64Url {
        private const string _base64UrlChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";
        private static readonly byte[] _base64UrlBytes;

        static Base64Url() {
            _base64UrlBytes = new byte[0x7b];
            for (var i = 0; i < _base64UrlBytes.Length; i++) {
                _base64UrlBytes[i] = 0b1000_0000;
            }
            for (var i = 0; i < _base64UrlChars.Length; i++) {
                _base64UrlBytes[_base64UrlChars[i]] = (byte)i;
            }
        }

        /// <summary>
        /// <see cref="byte"/> 配列を base64url にエンコードします。
        /// </summary>
        /// <param name="bin">エンコード対象の <see cref="byte"/> 配列。</param>
        /// <returns>base64url エンコード文字列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bin"/> is <c>null</c>.</exception>
        public static string Encode(byte[] bin) {
            if (bin == null) { throw new ArgumentNullException(nameof(bin)); }
            if (bin.Length == 0) { return ""; }

            var chars = new char[bin.Length * 4 / 3 + 1];
            var charLen = 0;
            for (var offset = 0; offset < bin.Length; offset += 3) {
                switch (bin.Length - offset) {
                    case 1:
                        chars[charLen++] = _base64UrlChars[bin[offset] >> 2];
                        chars[charLen++] = _base64UrlChars[(bin[offset] & 0b0011) << 4];
                        break;

                    case 2:
                        chars[charLen++] = _base64UrlChars[bin[offset] >> 2];
                        chars[charLen++] = _base64UrlChars[(bin[offset] & 0b0011) << 4 | bin[offset + 1] >> 4];
                        chars[charLen++] = _base64UrlChars[(bin[offset + 1] & 0b1111) << 2];
                        break;

                    default:
                        chars[charLen++] = _base64UrlChars[bin[offset] >> 2];
                        chars[charLen++] = _base64UrlChars[(bin[offset] & 0b0011) << 4 | bin[offset + 1] >> 4];
                        chars[charLen++] = _base64UrlChars[(bin[offset + 1] & 0b1111) << 2 | bin[offset + 2] >> 6];
                        chars[charLen++] = _base64UrlChars[bin[offset + 2] & 0b0011_1111];
                        break;
                }
            }

            return new string(chars, 0, charLen);
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
            if (encoded.Length == 0) {
                result = new byte[0];
                return true;
            }

            var bytes = new byte[(encoded.Length - 1) * 3 / 4 + 1];
            var charValues = new byte[4];

            var bytesLen = 0;
            for (var offset = 0; offset < encoded.Length; offset += 4) {
                var charBlockSize = Math.Min(encoded.Length - offset, 4);

                byte failed = 0;
                for (var i = 0; i < charBlockSize; i++) {
                    var charVal = _base64UrlBytes[encoded[offset + i]];
                    failed |= charVal;

                    charValues[i] = charVal;
                }
                if ((failed & 0b1000_0000) > 0) { goto Failure; }

                switch (charBlockSize) {
                    case 1:
                        goto Failure;

                    case 2:
                        bytes[bytesLen++] = (byte)(charValues[0] << 2 | charValues[1] >> 4);
                        break;

                    case 3:
                        bytes[bytesLen++] = (byte)(charValues[0] << 2 | charValues[1] >> 4);
                        bytes[bytesLen++] = (byte)(charValues[1] << 4 | charValues[2] >> 2);
                        break;

                    default:
                        bytes[bytesLen++] = (byte)(charValues[0] << 2 | charValues[1] >> 4);
                        bytes[bytesLen++] = (byte)(charValues[1] << 4 | charValues[2] >> 2);
                        bytes[bytesLen++] = (byte)(charValues[2] << 6 | charValues[3]);
                        break;
                }
            }
            Debug.Assert(bytes.Length == bytesLen);

            result = bytes;
            return true;

Failure:
            result = null;
            return false;
        }
    }
}
