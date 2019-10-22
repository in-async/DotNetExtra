using System;

namespace Inasync {

    /// <summary>
    /// <see cref="ArraySegment{T}"/> の為の拡張メソッドを提供します。
    /// </summary>
    public static class ArraySegmentExtensions {

        /// <summary>
        /// 対象の配列の上に新しい配列セグメントを作成します。
        /// </summary>
        /// <typeparam name="T">配列項目の型。</typeparam>
        /// <param name="array">変換する配列。</param>
        /// <returns>配列の配列セグメント表現。<paramref name="array"/> が <c>null</c> の場合は <c>default</c>。</returns>
        public static ArraySegment<T> AsSegment<T>(this T[] array) {
            if (array == null) { return default; }

            return new ArraySegment<T>(array);
        }

        /// <summary>
        /// 対象の配列の上に、指定された位置から配列の終端までを表す新しい配列セグメントを作成します。
        /// </summary>
        /// <typeparam name="T">配列項目の型。</typeparam>
        /// <param name="array">変換する配列。</param>
        /// <param name="offset">配列セグメントの開始オフセット。</param>
        /// <returns>配列の配列セグメント表現。<paramref name="array"/> が <c>null</c> の場合は <c>default</c>。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="array"/> が <c>null</c> の時に <paramref name="offset"/> が <c>0</c> でない。
        /// <para>または、</para>
        /// <paramref name="offset"/> が <paramref name="array"/> の範囲内にない。
        /// </exception>
        public static ArraySegment<T> AsSegment<T>(this T[] array, int offset) {
            if (array == null) {
                if (offset != 0) { throw new ArgumentOutOfRangeException(nameof(offset), offset, $"{nameof(array)} が null の時、{nameof(offset)} が 0 でなければなりません。"); }
                return default;
            }
            if (offset < 0) { throw new ArgumentOutOfRangeException(nameof(offset), offset, $"{nameof(offset)} が負の値です。"); }
            if (offset > array.Length) { throw new ArgumentOutOfRangeException(message: $"{nameof(offset)} が ${nameof(array)} の長さを超えています。", innerException: null); }

            return new ArraySegment<T>(array, offset, array.Length - offset);
        }

        /// <summary>
        /// 対象の配列の上に、指定された範囲を表す新しい配列セグメントを作成します。
        /// </summary>
        /// <typeparam name="T">配列項目の型。</typeparam>
        /// <param name="array">変換する配列。</param>
        /// <param name="offset">配列セグメントの開始オフセット。</param>
        /// <param name="count">配列セグメントの長さ。</param>
        /// <returns>配列の配列セグメント表現。<paramref name="array"/> が <c>null</c> の場合は <c>default</c>。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="array"/> が <c>null</c> の時に <paramref name="offset"/> 並びに <paramref name="count"/> が <c>0</c> でない。
        /// <para>または、</para>
        /// <paramref name="offset"/>、<paramref name="count"/> または <paramref name="offset"/> + <paramref name="count"/> が <paramref name="array"/> の範囲内にない。
        /// </exception>
        public static ArraySegment<T> AsSegment<T>(this T[] array, int offset, int count) {
            if (array == null) {
                if (offset != 0) { throw new ArgumentOutOfRangeException(nameof(offset), offset, $"{nameof(array)} が null の時、{nameof(offset)} が 0 でなければなりません。"); }
                if (count != 0) { throw new ArgumentOutOfRangeException(nameof(count), count, $"{nameof(array)} が null の時、{nameof(count)} が 0 でなければなりません。"); }
                return default;
            }
            if (offset < 0) { throw new ArgumentOutOfRangeException(nameof(offset), offset, $"{nameof(offset)} が負の値です。"); }
            if (count < 0) { throw new ArgumentOutOfRangeException(nameof(count), count, $"{nameof(count)} が負の値です。"); }
            if (offset + count > array.Length) { throw new ArgumentOutOfRangeException(message: $"{nameof(offset)} と ${nameof(count)} の和が ${nameof(array)} の長さを超えています。", innerException: null); }

            return new ArraySegment<T>(array, offset, count);
        }
    }
}
