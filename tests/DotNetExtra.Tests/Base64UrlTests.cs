using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class Base64UrlTests {

        [TestMethod]
        public void Encode_Obsoleted() {
            Action TestCase(int testNumber, byte[] bytes, string expected, Type expectedExceptionType = null) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => Base64Url.Encode(bytes))
                    .Verify(expected, expectedExceptionType);
            };

            new[]{
                TestCase( 0, null       , null  , typeof(ArgumentNullException)),
                TestCase(10, Bin()      , ""    ),
                TestCase(11, Bin(0)     , "AA"  ),
                TestCase(12, Bin(250)   , "-g"  ),
                TestCase(13, Bin(255, 0), "_wA" ),
            }.Run();
        }

        [TestMethod]
        public void Encode() {
            Action TestCase(int testNumber, byte[] bytes, bool padding, string expected, Type expectedExceptionType = null) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => Base64Url.Encode(bytes, padding))
                    .Verify(expected, expectedExceptionType);
            };

            new[]{
                TestCase( 0, null       , false, null  , typeof(ArgumentNullException)),
                TestCase(10, Bin()      , false, ""    ),
                TestCase(11, Bin(0)     , false, "AA"  ),
                TestCase(12, Bin(250)   , false, "-g"  ),
                TestCase(13, Bin(255, 0), false, "_wA" ),
                TestCase(20, Bin()      , true , ""    ),
                TestCase(21, Bin(0)     , true , "AA=="),
                TestCase(22, Bin(250)   , true , "-g=="),
                TestCase(23, Bin(255, 0), true , "_wA="),
            }.Run();
        }

        [TestMethod]
        public void Encode_Range() {
            Action TestCase(int testNumber, byte[] bin, int offset, int length, bool padding, string expected, Type expectedExceptionType = null) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => Base64Url.Encode(bin, offset, length, padding))
                    .Verify(expected, expectedExceptionType);
            };

            new[]{
                TestCase( 0, null       , 0 , 0 , false, null  , typeof(ArgumentNullException)),
                TestCase(10, Bin()      , 0 , 0 , false, ""    ),
                TestCase(11, Bin(0)     , 0 , 1 , false, "AA"  ),
                TestCase(12, Bin(250)   , 0 , 1 , false, "-g"  ),
                TestCase(13, Bin(255, 0), 0 , 2 , false, "_wA" ),
                TestCase(20, Bin()      , 0 , 0 , true , ""    ),
                TestCase(21, Bin(0)     , 0 , 1 , true , "AA=="),
                TestCase(22, Bin(250)   , 0 , 1 , true , "-g=="),
                TestCase(23, Bin(255, 0), 0 , 2 , true , "_wA="),
                TestCase(30, Bin(255, 9), -1, 0 , false, null, typeof(ArgumentOutOfRangeException)),
                TestCase(31, Bin(255, 9), 0 , -1, false, null, typeof(ArgumentOutOfRangeException)),
                TestCase(32, Bin(255, 9), 2 , 0 , false, ""  ),
                TestCase(33, Bin(255, 9), 1 , 1 , false, "CQ"),
                TestCase(34, Bin(255, 9), 1 , 2 , false, null, typeof(ArgumentOutOfRangeException)),
                TestCase(35, Bin(255, 9), 1 , 1 , true , "CQ=="),
            }.Run();
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
            Action TestCase(int testNumber, string encoded, (bool success, byte[] result) expected) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => (success: Base64Url.TryDecode(encoded, out var result), result))
                    .Verify((actual, desc) => {
                        Assert.AreEqual(expected.success, actual.success, desc);
                        CollectionAssert.AreEqual(expected.result, actual.result, desc);
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
