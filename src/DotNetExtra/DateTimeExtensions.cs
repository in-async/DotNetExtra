using System;

namespace DotNetExtra {

    /// <summary>
    /// <see cref="DateTime"/> の拡張クラス。
    /// </summary>
    public static class DateTimeExtensions {

        /// <summary>
        /// 月初日を返します。
        /// </summary>
        /// <param name="dt">対象の <see cref="DateTime"/>。</param>
        /// <returns><paramref name="dt"/> の月初日を返します。</returns>
        public static DateTime FirstDayOfMonth(this DateTime dt) => new DateTime(dt.Year, dt.Month, 1);

        /// <summary>
        /// 月末日を返します。
        /// </summary>
        /// <param name="dt">対象の <see cref="DateTime"/>。</param>
        /// <returns><paramref name="dt"/> の月末日を返します。</returns>
        public static DateTime LastDayOfMonth(this DateTime dt) => new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));

        /// <summary>
        /// 当月の日数を返します。
        /// </summary>
        /// <param name="dt">対象の <see cref="DateTime"/>。</param>
        /// <returns><paramref name="dt"/> が指す月の日数。</returns>
        public static int DaysInMonth(this DateTime dt) => DateTime.DaysInMonth(dt.Year, dt.Month);

        /// <summary>
        /// 当月の残時間を返します。
        /// </summary>
        /// <param name="dt">対象の <see cref="DateTime"/>。</param>
        /// <returns><paramref name="dt"/> の月内の残り時間。</returns>
        public static TimeSpan TimeLeftInMonth(this DateTime dt) => dt.LastDayOfMonth() - dt + TimeSpan.FromDays(1);

        /// <summary>
        /// 秒以下を 0 にして返します。
        /// </summary>
        /// <param name="dt">対象の <see cref="DateTime"/>。</param>
        /// <returns>秒以下を 0 にした <paramref name="dt"/>。</returns>
        public static DateTime TrimSeconds(this DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);

        /// <summary>
        /// 分以下を 0 にして返します。
        /// </summary>
        /// <param name="dt">対象の <see cref="DateTime"/>。</param>
        /// <returns>分以下を 0 にした <paramref name="dt"/>。</returns>
        public static DateTime TrimMinutes(this DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
    }
}