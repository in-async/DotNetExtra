namespace System {

    /// <summary>
    /// 乱数ヘルパー クラス。
    /// </summary>
    public static class Rand {
        private static readonly Random _rnd = new Random();

        /// <summary>
        /// ランダムな <see cref="byte"/> の配列を返します。
        /// </summary>
        /// <param name="minLength">最小配列長。既定値は 0。</param>
        /// <param name="maxLength">最大配列長。既定値は 10。</param>
        /// <returns>配列長が <paramref name="minLength"/> 以上 <paramref name="maxLength"/> 未満のランダムな <see cref="byte"/> 配列。常に非 <c>null</c>。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minLength"/> または <paramref name="maxLength"/> が負の値です。
        /// または、<paramref name="minLength"/> の値が <paramref name="maxLength"/> を超えています。
        /// </exception>
        public static byte[] Bytes(int minLength = 0, int maxLength = 10) {
            if (minLength < 0) { throw new ArgumentOutOfRangeException("負の値は許容されません。", nameof(minLength)); }
            if (maxLength < 0) { throw new ArgumentOutOfRangeException("負の値は許容されません。", nameof(maxLength)); }

            var bytes = new byte[_rnd.Next(minLength, maxLength)];
            _rnd.NextBytes(bytes);
            return bytes;
        }
    }
}
