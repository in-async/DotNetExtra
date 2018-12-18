using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetExtra.Tests {

    [TestClass]
    public class SemanticVersionTests {

        [TestMethod]
        public void Empty() {
            new TestCaseRunner()
                .Run(() => SemanticVersion.Empty)
                .Verify((actual, desc) => {
                    Assert.AreEqual(0, actual.Major, desc);
                    Assert.AreEqual(0, actual.Minor, desc);
                    Assert.AreEqual(0, actual.Patch, desc);
                    Assert.AreEqual(null, actual.PreReleaseId, desc);
                    Assert.AreEqual(null, actual.BuildMetadata, desc);
                    Assert.AreEqual("0.0.0", actual.ToString(), desc);
                    Assert.AreEqual(new SemanticVersion(0, 0, 0, null, null), actual, desc);
                }, (Type)null);
        }

        [TestMethod]
        public void SemanticVersion_BBB() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => new SemanticVersion(item.major, item.minor, item.patch))
                    .Verify((res, desc) => {
                        Assert.AreEqual(item.expected, (res.Major, res.Minor, res.Patch), desc);
                        Assert.IsNull(res.PreReleaseId, desc);
                        Assert.IsNull(res.BuildMetadata, desc);
                    }, item.expectedExceptionType);
            }

            // テストケース定義。
            (int testNumber, byte major, byte minor, byte patch, (byte major, byte minor, byte patch) expected, Type expectedExceptionType)[] TestCases() => new[]{
                ( 0, (byte)0, (byte)0, (byte)0, ((byte)0, (byte)0, (byte)0), (Type)null),
                ( 1, (byte)1, (byte)0, (byte)0, ((byte)1, (byte)0, (byte)0), (Type)null),
                ( 2, (byte)0, (byte)1, (byte)0, ((byte)0, (byte)1, (byte)0), (Type)null),
                ( 3, (byte)0, (byte)0, (byte)1, ((byte)0, (byte)0, (byte)1), (Type)null),

                (10, (byte)1, (byte)2, (byte)3, ((byte)1, (byte)2, (byte)3), (Type)null),
            };
        }

        [TestMethod]
        public void SemanticVersion_BBBSS() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => new SemanticVersion(item.major, item.minor, item.patch, item.preReleaseId, item.buildMetadata))
                    .Verify((res, desc) => {
                        Assert.AreEqual(item.expected, (res.Major, res.Minor, res.Patch, res.PreReleaseId, res.BuildMetadata), desc);
                    }, item.expectedExceptionType);
            }

            // テストケース定義。
            (int testNumber, byte major, byte minor, byte patch, string preReleaseId, string buildMetadata, (byte major, byte minor, byte patch, string preReleaseId, string buildMetadata) expected, Type expectedExceptionType)[] TestCases() => new[]{
                ( 0, (byte)1, (byte)0, (byte)0, null  , null             , ((byte)1, (byte)0, (byte)0, null  , null             ), (Type)null),
                ( 1, (byte)0, (byte)1, (byte)0, null  , null             , ((byte)0, (byte)1, (byte)0, null  , null             ), (Type)null),
                ( 2, (byte)0, (byte)0, (byte)1, null  , null             , ((byte)0, (byte)0, (byte)1, null  , null             ), (Type)null),
                ( 3, (byte)0, (byte)0, (byte)0, ""    , null             , ((byte)0, (byte)0, (byte)0, null  , null             ), (Type)null),
                ( 4, (byte)0, (byte)0, (byte)0, null  , ""               , ((byte)0, (byte)0, (byte)0, null  , null             ), (Type)null),

                (10, (byte)1, (byte)2, (byte)3, "beta", null             , ((byte)1, (byte)2, (byte)3, "beta", null             ), (Type)null),
                (11, (byte)1, (byte)2, (byte)3, null  , "exp.sha.5114f85", ((byte)1, (byte)2, (byte)3, null  , "exp.sha.5114f85"), (Type)null),
                (12, (byte)1, (byte)2, (byte)3, "beta", "exp.sha.5114f85", ((byte)1, (byte)2, (byte)3, "beta", "exp.sha.5114f85"), (Type)null),
            };
        }

        [TestMethod]
        public new void ToString() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.version.ToString())
                    .Verify(item.expected, item.expectedExceptionType);
            }

            // テストケース定義。
            (int testNumber, SemanticVersion version, string expected, Type expectedExceptionType)[] TestCases() => new[]{
                ( 0, new SemanticVersion(1, 0, 0, null  , null             ), "1.0.0"                     , (Type)null),
                ( 1, new SemanticVersion(0, 1, 0, null  , null             ), "0.1.0"                     , (Type)null),
                ( 2, new SemanticVersion(0, 0, 1, null  , null             ), "0.0.1"                     , (Type)null),
                ( 3, new SemanticVersion(0, 0, 0, ""    , null             ), "0.0.0"                     , (Type)null),
                ( 4, new SemanticVersion(0, 0, 0, null  , ""               ), "0.0.0"                     , (Type)null),

                (10, new SemanticVersion(1, 2, 3, "beta", null             ), "1.2.3-beta"                , (Type)null),
                (11, new SemanticVersion(1, 2, 3, null  , "exp.sha.5114f85"), "1.2.3+exp.sha.5114f85"     , (Type)null),
                (12, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), "1.2.3-beta+exp.sha.5114f85", (Type)null),
            };
        }

        [TestMethod]
        public void CompareTo() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => Math.Sign(item.version.CompareTo(item.other)))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            // テストケース定義。
            (int testNumber, SemanticVersion version, SemanticVersion other, int expected, Type expectedExceptionType)[] TestCases() => new[]{
                ( 0, new SemanticVersion(0, 0, 0, null  , null             ), null                                                   , 1 , (Type)null),
                ( 1, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 0, null  , null             ), 0 , (Type)null),
                ( 2, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(1, 0, 0, null  , null             ), -1, (Type)null),
                ( 3, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 1, 0, null  , null             ), -1, (Type)null),
                ( 4, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 1, null  , null             ), -1, (Type)null),
                ( 5, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 0, ""    , null             ), 0 , (Type)null),
                ( 6, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 0, null  , ""               ), 0 , (Type)null),
                ( 7, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 0, "beta", null             ), 1 , (Type)null),
                ( 8, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 0, null  , "exp.sha.5114f85"), 0 , (Type)null),

                (10, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), 0 , (Type)null),
                (11, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(0, 2, 3, "beta", "exp.sha.5114f85"), 1 , (Type)null),
                (12, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(1, 0, 3, "beta", "exp.sha.5114f85"), 1 , (Type)null),
                (13, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(1, 2, 0, "beta", "exp.sha.5114f85"), 1 , (Type)null),
                (14, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(1, 2, 3, null  , "exp.sha.5114f85"), -1, (Type)null),
                (15, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(1, 2, 3, "beta", null             ), 0 , (Type)null),
            };
        }

        [TestMethod]
        public void Equals_GetHashCode() {
            // Equals(SemanticVersion)
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.version.Equals(item.other))
                    .Verify(item.expected, item.expectedExceptionType);
            }
            // Equals(object)
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.version.Equals((object)item.other))
                    .Verify(item.expected, item.expectedExceptionType);
            }
            // GetHashCode
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.version.GetHashCode() == item.other?.GetHashCode())
                    .Verify(item.expected, item.expectedExceptionType);
            }

            // テストケース定義。
            (int testNumber, SemanticVersion version, SemanticVersion other, bool expected, Type expectedExceptionType)[] TestCases() => new[]{
                ( 0, new SemanticVersion(0, 0, 0, null  , null             ), null                                                   , false, (Type)null),

                ( 1, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 0, null  , null             ), true , (Type)null),
                ( 2, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(1, 0, 0, null  , null             ), false, (Type)null),
                ( 3, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 1, 0, null  , null             ), false, (Type)null),
                ( 4, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 1, null  , null             ), false, (Type)null),
                ( 5, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 0, ""    , null             ), true , (Type)null),
                ( 6, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 0, null  , ""               ), true , (Type)null),
                ( 7, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 0, "beta", null             ), false, (Type)null),
                ( 8, new SemanticVersion(0, 0, 0, null  , null             ), new SemanticVersion(0, 0, 0, null  , "exp.sha.5114f85"), false, (Type)null),

                ( 9, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), true , (Type)null),
                (10, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(0, 2, 3, "beta", "exp.sha.5114f85"), false, (Type)null),
                (11, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(1, 0, 3, "beta", "exp.sha.5114f85"), false, (Type)null),
                (12, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(1, 2, 0, "beta", "exp.sha.5114f85"), false, (Type)null),
                (13, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(1, 2, 3, null  , "exp.sha.5114f85"), false, (Type)null),
                (14, new SemanticVersion(1, 2, 3, "beta", "exp.sha.5114f85"), new SemanticVersion(1, 2, 3, "beta", null             ), false, (Type)null),
            };
        }
    }
}