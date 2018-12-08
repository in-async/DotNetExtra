using System;

namespace DotNetExtra {

    /// <summary>
    /// <see cref="DateTime"/> の拡張クラス。
    /// </summary>
    public static class DateTimeExtensions {

        /// <summary>
        /// 月初を返します。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime BeginningOfMonth(this DateTime dt) => new DateTime(dt.Year, dt.Month, 1);

        /// <summary>
        /// 月末を返します。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime dt) => new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));

        /// <summary>
        /// 当月の日数を返します。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int DaysInMonth(this DateTime dt) => DateTime.DaysInMonth(dt.Year, dt.Month);

        /// <summary>
        /// 当月の残日数を返します。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static TimeSpan LeftInMonth(this DateTime dt) => dt.BeginningOfMonth().AddMonths(1) - dt;

        /// <summary>
        /// 秒以下を 0 にして返します。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime TrimSeconds(this DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);

        /// <summary>
        /// 分以下を 0 にして返します。
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime TrimMinutes(this DateTime dt) => new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
    }
}