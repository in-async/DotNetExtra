namespace Inasync {

    /// <summary>
    /// 空配列をシングルトンに管理するクラス。
    /// <see cref="System.Array.Empty{T}"/> が無い環境への補完です。
    /// </summary>
    public static class EmptyArray {

        /// <summary>
        /// シングルトンな空配列を返します。
        /// </summary>
        /// <typeparam name="T">配列要素の型。</typeparam>
        /// <returns><typeparamref name="T"/> の空の配列。</returns>
        public static T[] Value<T>() {
            return InternalEmptyArray<T>.Value;
        }

        private static class InternalEmptyArray<T> {
            public static readonly T[] Value = new T[0];
        }
    }
}
