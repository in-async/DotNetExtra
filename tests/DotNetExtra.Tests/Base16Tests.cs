using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class Base16Tests {

        [TestMethod]
        public void Encode() {
            Action TestCase(int testNumber, byte[] bytes, bool toUpper, string expected, Type expectedExceptionType = null) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => Base16.Encode(bytes, toUpper))
                    .Verify(expected, expectedExceptionType);
            };

            var rndBytes = Rand.Bytes();
            new[]{
                TestCase( 0, null                                          , false, null              , typeof(ArgumentNullException)),
                TestCase( 1, Bytes()                                       , false, ""                ),
                TestCase( 2, Bytes(0x0f)                                   , false, "0f"              ),
                TestCase( 3, Bytes(0x0f,0xf0)                              , false, "0ff0"            ),
                TestCase( 4, Bytes(0x0f,0xf0)                              , true , "0FF0"            ),
                TestCase(10, Bytes(0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef), false, "0123456789abcdef"),
                TestCase(11, Bytes(0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef), true , "0123456789ABCDEF"),
                TestCase(50, rndBytes                                      , false, BitConverter.ToString(rndBytes).Replace("-", "").ToLowerInvariant()),
                TestCase(51, rndBytes                                      , true , BitConverter.ToString(rndBytes).Replace("-", "").ToUpperInvariant()),
            }.Run();
        }

        [TestMethod]
        public void Decode() {
            Action TestCase(int testNumber, string hexString, byte[] expected, Type expectedExceptionType = null) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => Base16.Decode(hexString))
                    .Verify(expected, expectedExceptionType);
            };

            new[]{
                TestCase( 0, null              , null                                          , typeof(ArgumentNullException)),
                TestCase( 1, ""                , Bytes()                                       ),
                TestCase( 2, " "               , null                                          , typeof(FormatException)),
                TestCase( 3, "0"               , null                                          , typeof(FormatException)),
                TestCase( 4, "0g"              , null                                          , typeof(FormatException)),
                TestCase( 5, "0f"              , Bytes(0x0f)                                   ),
                TestCase( 6, "0fF0"            , Bytes(0x0f,0xf0)                              ),
                TestCase(10, "0123456789abcDEF", Bytes(0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef)),
            }.Run();
        }

        [TestMethod]
        public void TryDecode() {
            Action TestCase(int testNumber, string hexString, (bool success, byte[] result) expected) => () => {
                new TestCaseRunner($"No.{testNumber}")
                    .Run(() => (success: Base16.TryDecode(hexString, out var result), result))
                    .Verify((actual, desc) => {
                        if (actual.result != null) {
                            Console.WriteLine(Base16.Encode(actual.result));
                        }
                        Assert.AreEqual(expected.success, actual.success, desc);
                        CollectionAssert.AreEqual(expected.result, actual.result, desc);
                    }, (Type)null);
            };

            new[] {
                TestCase( 0, null              , (false, null                                          )),
                TestCase( 1, ""                , (true , Bytes()                                       )),
                TestCase( 2, " "               , (false, null                                          )),
                TestCase( 3, "0"               , (false, null                                          )),
                TestCase( 4, "0g"              , (false, null                                          )),
                TestCase( 5, "0f"              , (true , Bytes(0x0f)                                   )),
                TestCase( 6, "0fF0"            , (true , Bytes(0x0f,0xf0)                              )),
                TestCase(10, "0123456789abcDEF", (true , Bytes(0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef))),
            }.Run();
        }

        #region Helpers

        private static byte[] Bytes(params byte[] bytes) => bytes;

        #endregion Helpers
    }
}
