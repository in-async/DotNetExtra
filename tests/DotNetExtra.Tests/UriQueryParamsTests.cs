using System;
using System.Linq;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class UriQueryParamsTests {

        [TestMethod]
        public void UriQueryParams() {
            new TestCaseRunner()
                .Run(() => new UriQueryParams())
                .Verify(new UriQueryParams(), (Type)null);
        }

        [TestMethod]
        public void UriQueryParams_Uri() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => new UriQueryParams(item.uri))
                    .Verify((actual, desc) => {
                        CollectionAssert.AreEqual(new UriQueryParams(item.uri).Cast<object>().ToArray(), actual?.Cast<object>().ToArray(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, Uri uri, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, null                                              , (Type)typeof(ArgumentNullException)),
                (10, new Uri("http://example.com/")                    , (Type)null),
                (11, new Uri("http://example.com/?")                   , (Type)null),
                (12, new Uri("http://example.com/?foo")                , (Type)null),
                (13, new Uri("http://example.com/?foo=")               , (Type)null),
                (14, new Uri("http://example.com/?foo=bar")            , (Type)null),
                (15, new Uri("http://example.com/?foo=bar&")           , (Type)null),
                (16, new Uri("http://example.com/?foo=bar&%3f")        , (Type)null),
                (17, new Uri("http://example.com/?foo=bar&%3f=")       , (Type)null),
                (18, new Uri("http://example.com/?foo=bar&%3f=%26")    , (Type)null),
                (19, new Uri("http://example.com/?foo=bar&%3f=%26#")   , (Type)null),
                (20, new Uri("http://example.com/?foo=bar&%3f=%26#%2b"), (Type)null),
            };
        }

        [TestMethod]
        public new void ToString() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.query.ToString())
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, string expected, Type expectedExceptionType)[] TestCases() => new[] {
                (10, new UriQueryParams(                                                  ), ""                   , (Type)null),
                (11, new UriQueryParams(new Uri("http://example.com/")                    ), ""                   , (Type)null),
                (12, new UriQueryParams(new Uri("http://example.com/?")                   ), ""                   , (Type)null),
                (13, new UriQueryParams(new Uri("http://example.com/?foo")                ), "foo="               , (Type)null),
                (14, new UriQueryParams(new Uri("http://example.com/?foo=")               ), "foo="               , (Type)null),
                (15, new UriQueryParams(new Uri("http://example.com/?foo=bar")            ), "foo=bar"            , (Type)null),
                (16, new UriQueryParams(new Uri("http://example.com/?foo=bar&")           ), "foo=bar"            , (Type)null),
                (17, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f")        ), "foo=bar&%3F="       , (Type)null),
                (18, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f=")       ), "foo=bar&%3F="       , (Type)null),
                (19, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f=%26")    ), "foo=bar&%3F=%26"    , (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f=%26#%2b")), "foo=bar&%3F=%26"    , (Type)null),
                (21, new UriQueryParams(new Uri("http://example.com/?ほ=げ")              ), "%E3%81%BB=%E3%81%92", (Type)null),
            };
        }

        [TestMethod]
        public void Add_Key_OValue() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.query.Add(item.key, item.value))
                    .Verify(desc => {
                        Assert.AreEqual(item.expected, item.query.ToString(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, string key, object value, string expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new UriQueryParams()                                   , (string)null , (object)"/" , null          , (Type)typeof(ArgumentNullException)),
                (10, new UriQueryParams()                                   , (string)"1"  , (object)null, "1="          , (Type)null),
                (11, new UriQueryParams()                                   , (string)"1"  , (object)2   , "1=2"         , (Type)null),
                (12, new UriQueryParams()                                   , (string)"1"  , (object)"/" , "1=%2F"       , (Type)null),
                (13, new UriQueryParams()                                   , (string)"+"  , (object)null, "%2B="        , (Type)null),
                (14, new UriQueryParams()                                   , (string)"+"  , (object)2   , "%2B=2"       , (Type)null),
                (15, new UriQueryParams()                                   , (string)"+"  , (object)"/" , "%2B=%2F"     , (Type)null),
                (16, new UriQueryParams()                                   , (string)"%2B", (object)null, "%252B="      , (Type)null),
                (17, new UriQueryParams()                                   , (string)"%2B", (object)2   , "%252B=2"     , (Type)null),
                (18, new UriQueryParams()                                   , (string)"%2B", (object)"/" , "%252B=%2F"   , (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?1="))  , (string)"+"  , (object)2   , "1=&%2B=2"    , (Type)null),
                (21, new UriQueryParams(new Uri("http://example.com/?+="))  , (string)"+"  , (object)2   , null          , (Type)typeof(ArgumentException)),
                (22, new UriQueryParams(new Uri("http://example.com/?%2B=")), (string)"+"  , (object)2   , null          , (Type)typeof(ArgumentException)),
                (23, new UriQueryParams(new Uri("http://example.com/?%2B=")), (string)"%2B", (object)2   , "%2B=&%252B=2", (Type)null),
                // 順序保証。
                (30, new UriQueryParams(new Uri("http://example.com/?b="))  , (string)"a"  ,  (object)2  , "b=&a=2"      , (Type)null),
            };
        }
    }
}
