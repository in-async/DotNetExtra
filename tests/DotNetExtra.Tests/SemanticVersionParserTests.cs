using System;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class SemanticVersionParserTests {

        [TestMethod]
        public void Parse() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => SemanticVersionParser.Default.Parse(item.value))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            // テストケース定義。
            (int testNumber, string value, SemanticVersion expected, Type expectedExceptionType)[] TestCases() => new[]{
                ( 0, null                        , null                                                         , (Type)typeof(ArgumentNullException)),
                ( 1, ""                          , null                                                         , (Type)typeof(FormatException)),
                ( 2, "   "                       , null                                                         , (Type)typeof(FormatException)),
                ( 3, "0"                         , null                                                         , (Type)typeof(FormatException)),
                ( 4, "1"                         , null                                                         , (Type)typeof(FormatException)),
                ( 5, "1.2"                       , null                                                         , (Type)typeof(FormatException)),
                ( 6, "1.2.3"                     , new SemanticVersion(1, 2, 3, null        , null             ), (Type)null),
                ( 7, "1.2.3-"                    , null                                                         , (Type)typeof(FormatException)),
                ( 8, "1.2.3-123"                 , new SemanticVersion(1, 2, 3, "123"       , null             ), (Type)null),
                ( 9, "1.2.3+"                    , null                                                         , (Type)typeof(FormatException)),
                (10, "1.2.3+456"                 , new SemanticVersion(1, 2, 3, null        , "456"            ), (Type)null),
                (11, "1.2.3-123+456"             , new SemanticVersion(1, 2, 3, "123"       , "456"            ), (Type)null),
                (12, "  1.2.3-123+456  "         , new SemanticVersion(1, 2, 3, "123"       , "456"            ), (Type)null),

                (13, "1.0.0-alpha"               , new SemanticVersion(1, 0, 0, "alpha"     , null             ), (Type)null),
                (14, "1.0.0-alpha.1"             , new SemanticVersion(1, 0, 0, "alpha.1"   , null             ), (Type)null),
                (15, "1.0.0-0.3.7"               , new SemanticVersion(1, 0, 0, "0.3.7"     , null             ), (Type)null),
                (16, "1.0.0-x.7.z.92"            , new SemanticVersion(1, 0, 0, "x.7.z.92"  , null             ), (Type)null),

                (17, "1.0.0-alpha+001"           , new SemanticVersion(1, 0, 0, "alpha"     , "001"            ), (Type)null),
                (18, "1.0.0+20130313144700"      , new SemanticVersion(1, 0, 0, null        , "20130313144700" ), (Type)null),
                (19, "1.0.0-beta+exp.sha.5114f85", new SemanticVersion(1, 0, 0, "beta"      , "exp.sha.5114f85"), (Type)null),

                (20, "1.0.0-alpha.beta"          , new SemanticVersion(1, 0, 0, "alpha.beta", null             ), (Type)null),
                (21, "1.0.0-beta"                , new SemanticVersion(1, 0, 0, "beta"      , null             ), (Type)null),
                (22, "1.0.0-beta.2"              , new SemanticVersion(1, 0, 0, "beta.2"    , null             ), (Type)null),
                (23, "1.0.0-beta.11"             , new SemanticVersion(1, 0, 0, "beta.11"   , null             ), (Type)null),
                (24, "1.0.0-rc.1"                , new SemanticVersion(1, 0, 0, "rc.1"      , null             ), (Type)null),
                (25, "1.0.0"                     , new SemanticVersion(1, 0, 0, null        , null             ), (Type)null),

                (50, "1.0.4:beta"                , null                                                         , (Type)typeof(FormatException)),
                (51, "1.0.9.6"                   , null                                                         , (Type)typeof(FormatException)),
                (52, "a.0.0"                     , null                                                         , (Type)typeof(FormatException)),
                (53, "1.a.0"                     , null                                                         , (Type)typeof(FormatException)),
                (54, "1.0.a"                     , null                                                         , (Type)typeof(FormatException)),
            };
        }

        [TestMethod]
        public void TryParse() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => (SemanticVersionParser.Default.TryParse(item.value, out var actualVersion), actualVersion))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            // テストケース定義。
            (int testNumber, string value, (bool, SemanticVersion) expected, Type expectedExceptionType)[] TestCases() => new[]{
                ( 0, null                        , (false, null                                                         ), (Type)null),
                ( 1, ""                          , (false, null                                                         ), (Type)null),
                ( 2, "   "                       , (false, null                                                         ), (Type)null),
                ( 3, "0"                         , (false, null                                                         ), (Type)null),
                ( 4, "1"                         , (false, null                                                         ), (Type)null),
                ( 5, "1.2"                       , (false, null                                                         ), (Type)null),
                ( 6, "1.2.3"                     , (true , new SemanticVersion(1, 2, 3, null        , null             )), (Type)null),
                ( 7, "1.2.3-"                    , (false, null                                                         ), (Type)null),
                ( 8, "1.2.3-123"                 , (true , new SemanticVersion(1, 2, 3, "123"       , null             )), (Type)null),
                ( 9, "1.2.3+"                    , (false, null                                                         ), (Type)null),
                (10, "1.2.3+456"                 , (true , new SemanticVersion(1, 2, 3, null        , "456"            )), (Type)null),
                (11, "1.2.3-123+456"             , (true , new SemanticVersion(1, 2, 3, "123"       , "456"            )), (Type)null),
                (12, "  1.2.3-123+456  "         , (true , new SemanticVersion(1, 2, 3, "123"       , "456"            )), (Type)null),

                (13, "1.0.0-alpha"               , (true , new SemanticVersion(1, 0, 0, "alpha"     , null             )), (Type)null),
                (14, "1.0.0-alpha.1"             , (true , new SemanticVersion(1, 0, 0, "alpha.1"   , null             )), (Type)null),
                (15, "1.0.0-0.3.7"               , (true , new SemanticVersion(1, 0, 0, "0.3.7"     , null             )), (Type)null),
                (16, "1.0.0-x.7.z.92"            , (true , new SemanticVersion(1, 0, 0, "x.7.z.92"  , null             )), (Type)null),

                (17, "1.0.0-alpha+001"           , (true , new SemanticVersion(1, 0, 0, "alpha"     , "001"            )), (Type)null),
                (18, "1.0.0+20130313144700"      , (true , new SemanticVersion(1, 0, 0, null        , "20130313144700" )), (Type)null),
                (19, "1.0.0-beta+exp.sha.5114f85", (true , new SemanticVersion(1, 0, 0, "beta"      , "exp.sha.5114f85")), (Type)null),

                (20, "1.0.0-alpha.beta"          , (true , new SemanticVersion(1, 0, 0, "alpha.beta", null             )), (Type)null),
                (21, "1.0.0-beta"                , (true , new SemanticVersion(1, 0, 0, "beta"      , null             )), (Type)null),
                (22, "1.0.0-beta.2"              , (true , new SemanticVersion(1, 0, 0, "beta.2"    , null             )), (Type)null),
                (23, "1.0.0-beta.11"             , (true , new SemanticVersion(1, 0, 0, "beta.11"   , null             )), (Type)null),
                (24, "1.0.0-rc.1"                , (true , new SemanticVersion(1, 0, 0, "rc.1"      , null             )), (Type)null),
                (25, "1.0.0"                     , (true , new SemanticVersion(1, 0, 0, null        , null             )), (Type)null),

                (50, "1.0.4:beta"                , (false, null                                                         ), (Type)null),
                (51, "1.0.9.6"                   , (false, null                                                         ), (Type)null),
                (52, "a.0.0"                     , (false, null                                                         ), (Type)null),
                (53, "1.a.0"                     , (false, null                                                         ), (Type)null),
                (54, "1.0.a"                     , (false, null                                                         ), (Type)null),
            };
        }
    }
}
