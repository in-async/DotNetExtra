using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetExtra.Tests {

    [TestClass]
    public class DateTimeExtensionsTests {

        [TestMethod]
        public void FirstDayOfMonth() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => DateTimeExtensions.FirstDayOfMonth(item.dt))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, DateTime dt, DateTime expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, DateTime.MinValue         , new DateTime(1   , 1 , 1), (Type)null),
                ( 1, DateTime.MaxValue         , new DateTime(9999, 12, 1), (Type)null),
                (10, new DateTime(2018, 12, 18), new DateTime(2018, 12, 1), (Type)null),
                (11, new DateTime(2018,  2, 18), new DateTime(2018, 2 , 1), (Type)null),
                (12, new DateTime(2020,  2, 18), new DateTime(2020, 2 , 1), (Type)null),    // 閏年
            };
        }

        [TestMethod]
        public void LastDayOfMonth() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => DateTimeExtensions.LastDayOfMonth(item.dt))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, DateTime dt, DateTime expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, DateTime.MinValue         , new DateTime(1   , 1 , 31), (Type)null),
                ( 1, DateTime.MaxValue         , new DateTime(9999, 12, 31), (Type)null),
                (10, new DateTime(2018, 12, 18), new DateTime(2018, 12, 31), (Type)null),
                (11, new DateTime(2018,  2, 18), new DateTime(2018, 2 , 28), (Type)null),
                (12, new DateTime(2020,  2, 18), new DateTime(2020, 2 , 29), (Type)null),    // 閏年
            };
        }

        [TestMethod]
        public void DaysInMonth() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => DateTimeExtensions.DaysInMonth(item.dt))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, DateTime dt, int expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, DateTime.MinValue         , 31, (Type)null),
                ( 1, DateTime.MaxValue         , 31, (Type)null),
                (10, new DateTime(2018, 12, 18), 31, (Type)null),
                (11, new DateTime(2018,  2, 18), 28, (Type)null),
                (12, new DateTime(2020,  2, 18), 29, (Type)null),    // 閏年
            };
        }

        [TestMethod]
        public void TimeLeftInMonth() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => DateTimeExtensions.TimeLeftInMonth(item.dt))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, DateTime dt, TimeSpan expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, DateTime.MinValue                     , TimeSpan.FromDays(31)       , (Type)null),
                ( 1, DateTime.MaxValue                     , TimeSpan.FromTicks(1)       , (Type)null),
                ( 2, new DateTime(9999, 12, 18)            , TimeSpan.FromDays(14)       , (Type)null),
                (10, new DateTime(2018, 12, 18)            , TimeSpan.FromDays(14)       , (Type)null),
                (11, new DateTime(2018,  2, 18)            , TimeSpan.FromDays(11)       , (Type)null),
                (12, new DateTime(2020,  2, 18)            , TimeSpan.FromDays(12)       , (Type)null),    // 閏年
                (20, new DateTime(2018, 12, 18, 11, 43, 45), new TimeSpan(13, 12, 16, 15), (Type)null),
            };
        }

        [TestMethod]
        public void TrimSeconds() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => DateTimeExtensions.TrimSeconds(item.dt))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, DateTime dt, DateTime expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, DateTime.MinValue                     , new DateTime(1   , 1 , 1 )           , (Type)null),
                ( 1, DateTime.MaxValue                     , new DateTime(9999, 12, 31, 23, 59, 0), (Type)null),
                (10, new DateTime(2018, 12, 18)            , new DateTime(2018, 12, 18)           , (Type)null),
                (11, new DateTime(2018,  2, 18)            , new DateTime(2018,  2, 18)           , (Type)null),
                (12, new DateTime(2020,  2, 18)            , new DateTime(2020,  2, 18)           , (Type)null),    // 閏年
                (20, new DateTime(2018, 12, 18, 11, 43, 45), new DateTime(2018, 12, 18, 11, 43, 0), (Type)null),
            };
        }

        [TestMethod]
        public void TrimMinutes() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => DateTimeExtensions.TrimMinutes(item.dt))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, DateTime dt, DateTime expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, DateTime.MinValue                     , new DateTime(1   , 1 , 1 )          , (Type)null),
                ( 1, DateTime.MaxValue                     , new DateTime(9999, 12, 31, 23, 0, 0), (Type)null),
                (10, new DateTime(2018, 12, 18)            , new DateTime(2018, 12, 18)          , (Type)null),
                (11, new DateTime(2018,  2, 18)            , new DateTime(2018,  2, 18)          , (Type)null),
                (12, new DateTime(2020,  2, 18)            , new DateTime(2020,  2, 18)          , (Type)null),    // 閏年
                (20, new DateTime(2018, 12, 18, 11, 43, 45), new DateTime(2018, 12, 18, 11, 0, 0), (Type)null),
            };
        }
    }
}