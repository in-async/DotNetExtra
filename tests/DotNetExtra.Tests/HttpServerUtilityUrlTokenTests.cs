using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class HttpServerUtilityUrlTokenTests {

        [TestMethod]
        public void Encode() {
            Action TestCase(int testNumber, byte[] bytes, string expected, Type expectedExceptionType = null) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => HttpServerUtilityUrlToken.Encode(bytes))
                    .Verify(expected, expectedExceptionType);
            };

            new[]{
                TestCase( 0, null            , null   , typeof(ArgumentNullException)),
                TestCase(10, Bin()           , ""     ),
                TestCase(11, Bin(0)          , "AA2"  ),
                TestCase(12, Bin(0, 255)     , "AP81" ),
                TestCase(13, Bin(0, 255, 254), "AP_-0"),
            }.Run();
        }

        [TestMethod]
        public void Encode_ByteSegment() {
            Action TestCase(int testNumber, ArraySegment<byte> bytes, string expected, Type expectedExceptionType = null) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => HttpServerUtilityUrlToken.Encode(bytes))
                    .Verify(expected, expectedExceptionType);
            };

            new[]{
                TestCase(10, default           , ""     ),
                TestCase(11, ByteS()           , ""     ),
                TestCase(12, ByteS(0)          , "AA2"  ),
                TestCase(13, ByteS(0, 255)     , "AP81" ),
                TestCase(14, ByteS(0, 255, 254), "AP_-0"),
            }.Run();
        }

        [TestMethod]
        public void Decode() {
            Action TestCase(int testNumber, string encoded, byte[] expected, Type expectedExceptionType = null) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => HttpServerUtilityUrlToken.Decode(encoded))
                    .Verify((res, desc) => CollectionAssert.AreEqual(expected, res, desc), expectedExceptionType);
            };

            new[]{
                TestCase( 0, null   , null            , typeof(ArgumentNullException)),
                TestCase( 1, "4"    , null            , typeof(FormatException)),
                TestCase( 2, "AA1"  , null            , typeof(FormatException)),
                TestCase(10, ""     , Bin()           ),
                TestCase(11, "AA2"  , Bin(0)          ),
                TestCase(12, "AP81" , Bin(0, 255)     ),
                TestCase(13, "AP_-0", Bin(0, 255, 254)),
            }.Run();
        }

        [TestMethod]
        public void TryDecode() {
            Action TestCase(int testNumber, string encoded, (bool success, byte[] result) expected) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => (success: HttpServerUtilityUrlToken.TryDecode(encoded, out var result), result))
                    .Verify((actual, desc) => {
                        Assert.AreEqual(expected.success, actual.success, desc);
                        CollectionAssert.AreEqual(expected.result, actual.result, desc);
                    }, (Type)null);
            };

            new[] {
                TestCase( 0, null   , (false, null)),
                TestCase( 1, "4"    , (false, null)),
                TestCase( 2, "AA1"  , (false, null)),
                TestCase(10, ""     , (true , Bin())),
                TestCase(11, "AA2"  , (true , Bin(0))),
                TestCase(12, "AP81" , (true , Bin(0, 255))),
                TestCase(13, "AP_-0", (true , Bin(0, 255, 254))),
            }.Run();
        }

        #region Helper

        private static byte[] Bin(params byte[] bin) => bin;

        private static ArraySegment<byte> ByteS(params byte[] bytes) => new ArraySegment<byte>(bytes);

        #endregion Helper
    }
}
