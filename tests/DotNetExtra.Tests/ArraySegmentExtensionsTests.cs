using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class ArraySegmentExtensionsTests {

        [TestMethod]
        public void AsSegment() {
            Action TestCase(int testNumber, byte[] bytes, ArraySegment<byte> expected, Type expectedExceptionType = null) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => ArraySegmentExtensions.AsSegment(bytes))
                    .Verify((actual, desc) => Assert.AreEqual(expected, actual, desc), expectedExceptionType);
            };

            var bytes1 = Rand.Bytes(minLength: 1, maxLength: 1);
            new[]{
                TestCase( 0, null  , new ArraySegment<byte>()),
                TestCase(10, bytes1, new ArraySegment<byte>(bytes1)),
            }.Run();
        }

        [TestMethod]
        public void AsSegment_Offset() {
            Action TestCase(int testNumber, byte[] bytes, int offset, ArraySegment<byte> expected, Type expectedExceptionType = null) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => ArraySegmentExtensions.AsSegment(bytes, offset))
                    .Verify((actual, desc) => Assert.AreEqual(expected, actual, desc), expectedExceptionType);
            };

            var bytes1 = Rand.Bytes(minLength: 1, maxLength: 1);
            new[]{
                TestCase( 0, null  , offset: 0 , expected: new ArraySegment<byte>()            ),
                TestCase( 1, null  , offset: 1 , expected: default                             , typeof(ArgumentOutOfRangeException)),
                TestCase(10, bytes1, offset: -1, expected: default                             , typeof(ArgumentOutOfRangeException)),
                TestCase(11, bytes1, offset: 0 , expected: new ArraySegment<byte>(bytes1, 0, 1)),
                TestCase(12, bytes1, offset: 1 , expected: new ArraySegment<byte>(bytes1, 1, 0)),
                TestCase(13, bytes1, offset: 2 , expected: default                             , typeof(ArgumentOutOfRangeException)),
            }.Run();
        }

        [TestMethod]
        public void AsSegment_Range() {
            Action TestCase(int testNumber, byte[] bytes, int offset, int count, ArraySegment<byte> expected, Type expectedExceptionType = null) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => ArraySegmentExtensions.AsSegment(bytes, offset, count))
                    .Verify((actual, desc) => Assert.AreEqual(expected, actual, desc), expectedExceptionType);
            };

            var bytes1 = Rand.Bytes(minLength: 1, maxLength: 1);
            new[]{
                TestCase( 0, null  , offset: 0 , count:  0, expected: new ArraySegment<byte>()            ),
                TestCase( 1, null  , offset: 0 , count:  1, expected: default                             , typeof(ArgumentOutOfRangeException)),
                TestCase( 2, null  , offset: 1 , count:  0, expected: default                             , typeof(ArgumentOutOfRangeException)),
                TestCase(10, bytes1, offset: -1, count:  0, expected: default                             , typeof(ArgumentOutOfRangeException)),
                TestCase(11, bytes1, offset: 0 , count: -1, expected: default                             , typeof(ArgumentOutOfRangeException)),
                TestCase(12, bytes1, offset: 0 , count:  0, expected: new ArraySegment<byte>(bytes1, 0, 0)),
                TestCase(13, bytes1, offset: 0 , count:  1, expected: new ArraySegment<byte>(bytes1, 0, 1)),
                TestCase(14, bytes1, offset: 0 , count:  2, expected: default                             , typeof(ArgumentOutOfRangeException)),
                TestCase(15, bytes1, offset: 1 , count:  0, expected: new ArraySegment<byte>(bytes1, 1, 0)),
                TestCase(16, bytes1, offset: 1 , count:  1, expected: default                             , typeof(ArgumentOutOfRangeException)),
                TestCase(17, bytes1, offset: 2 , count:  0, expected: default                             , typeof(ArgumentOutOfRangeException)),
            }.Run();
        }
    }
}
