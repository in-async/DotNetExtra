using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetExtra.Tests {

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

        #region Helper

        private static byte[] Bin(params byte[] bin) => bin;

        #endregion Helper
    }
}