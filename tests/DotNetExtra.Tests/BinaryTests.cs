using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class BinaryTests {

        [TestMethod]
        public void ToHexString() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.data.ToHexString(item.toUpper))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, byte[] data, bool toUpper, string expected, Type expectedExceptionType)[] TestCases() => new[]{
                ( 0, null                                    , false, null              , (Type)typeof(ArgumentNullException)),
                (10, Bin(0x01,0x23)                          , false, "0123"            , (Type)null),
                (11, BitConverter.GetBytes(0x1)              , false, "01000000"        , (Type)null),
                (12, BitConverter.GetBytes(-1)               , false, "ffffffff"        , (Type)null),
                (13, BitConverter.GetBytes(0x12)             , false, "12000000"        , (Type)null),
                (14, BitConverter.GetBytes(0x123)            , false, "23010000"        , (Type)null),
                (15, BitConverter.GetBytes(0x123456789abcdef), false, "efcdab8967452301", (Type)null),
                (16, BitConverter.GetBytes(0x123)            , true , "23010000"        , (Type)null),
                (17, BitConverter.GetBytes(0x123456789abcdef), true , "EFCDAB8967452301", (Type)null),
                (50, Bin(0xac,0xbd,0x18,0xdb,0x4c,0xc2,0xf8,0x5c,0xed,0xef,0x65,0x4f,0xcc,0xc4,0xa4,0xd8), false, "acbd18db4cc2f85cedef654fccc4a4d8", (Type)null),
                (51, Bin(0xac,0xbd,0x18,0xdb,0x4c,0xc2,0xf8,0x5c,0xed,0xef,0x65,0x4f,0xcc,0xc4,0xa4,0xd8), true , "ACBD18DB4CC2F85CEDEF654FCCC4A4D8", (Type)null),
            };
        }

        [TestMethod]
        public void Parse() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => Binary.Parse(item.hexString))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, string hexString, byte[] expected, Type expectedExceptionType)[] TestCases() => new[]{
                ( 0, null              , null                                    , (Type)typeof(ArgumentNullException)),
                ( 1, "123"             , null                                    , (Type)typeof(ArgumentOutOfRangeException)),
                ( 2, "wxyz"            , null                                    , (Type)typeof(FormatException)),
                (10, "0123"            , Bin(0x01,0x23)                          , (Type)null),
                (11, "01000000"        , BitConverter.GetBytes(0x1)              , (Type)null),
                (12, "ffffffff"        , BitConverter.GetBytes(-1)               , (Type)null),
                (13, "12000000"        , BitConverter.GetBytes(0x12)             , (Type)null),
                (14, "23010000"        , BitConverter.GetBytes(0x123)            , (Type)null),
                (15, "efcdab8967452301", BitConverter.GetBytes(0x123456789abcdef), (Type)null),
                (16, "23010000"        , BitConverter.GetBytes(0x123)            , (Type)null),
                (17, "EFCDAB8967452301", BitConverter.GetBytes(0x123456789abcdef), (Type)null),
                (50, "acbd18db4cc2f85cedef654fccc4a4d8", Bin(0xac,0xbd,0x18,0xdb,0x4c,0xc2,0xf8,0x5c,0xed,0xef,0x65,0x4f,0xcc,0xc4,0xa4,0xd8), (Type)null),
                (51, "ACBD18DB4CC2F85CEDEF654FCCC4A4D8", Bin(0xac,0xbd,0x18,0xdb,0x4c,0xc2,0xf8,0x5c,0xed,0xef,0x65,0x4f,0xcc,0xc4,0xa4,0xd8), (Type)null),
            };
        }

        #region Helpers

        private static byte[] Bin(params byte[] bin) => bin;

        #endregion Helpers
    }
}
