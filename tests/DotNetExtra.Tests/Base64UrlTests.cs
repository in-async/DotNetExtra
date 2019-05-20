using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class Base64UrlTests {

        [TestMethod]
        public void Encode() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => Base64Url.Encode(item.bin))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, byte[] bin, string expected, Type expectedExceptionType)[] TestCases() => new[]{
                ( 0, null       , null , (Type)typeof(ArgumentNullException)),
                (10, Bin()      , ""   , (Type)null),
                (11, Bin(0)     , "AA" , (Type)null),
                (12, Bin(250)   , "-g" , (Type)null),
                (13, Bin(255, 0), "_wA", (Type)null),
            };
        }

        [TestMethod]
        public void Decode() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => Base64Url.Decode(item.encoded))
                    .Verify((res, desc) => CollectionAssert.AreEqual(item.expected, res, desc), item.expectedExceptionType);
            }

            (int testNumber, string encoded, byte[] expected, Type expectedExceptionType)[] TestCases() => new[]{
                ( 0, null , null       , (Type)typeof(ArgumentNullException)),
                ( 1, "@"  , null       , (Type)typeof(FormatException)),
                (10, ""   , Bin()      , (Type)null),
                (11, "AA" , Bin(0)     , (Type)null),
                (12, "-g" , Bin(250)   , (Type)null),
                (13, "_wA", Bin(255, 0), (Type)null),
            };
        }

        [TestMethod]
        public void TryDecode() {
            Action TestCase(int testNumber, string encoded, (bool, byte[]) expected) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => (Base64Url.TryDecode(encoded, out var result), result))
                    .Verify((actual, desc) => {
                        Assert.AreEqual(expected.Item1, actual.Item1, desc);
                        CollectionAssert.AreEqual(expected.Item2, actual.Item2, desc);
                    }, (Type)null);
            };

            new[] {
                TestCase( 0, null , (false, null)),
                TestCase( 1, "@"  , (false, null)),
                TestCase(10, ""   , (true , Bin())),
                TestCase(11, "AA" , (true , Bin(0))),
                TestCase(12, "-g" , (true , Bin(250))),
                TestCase(13, "_wA", (true , Bin(255, 0))),
            }.Run();
        }

        #region Helper

        private static byte[] Bin(params byte[] bin) => bin;

        #endregion Helper
    }
}
