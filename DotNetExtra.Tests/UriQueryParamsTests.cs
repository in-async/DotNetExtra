using System;
using System.Collections.Generic;
using System.Linq;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetExtra.Tests {

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

        [TestMethod]
        public void Add_Key_SValue() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.query.Add(item.key, item.value))
                    .Verify(desc => {
                        Assert.AreEqual(item.expected, item.query.ToString(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, string key, string value, string expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new UriQueryParams()                                   , (string)null , (string)"/" , null          , (Type)typeof(ArgumentNullException)),
                (10, new UriQueryParams()                                   , (string)"1"  , (string)null, "1="          , (Type)null),
                (11, new UriQueryParams()                                   , (string)"1"  , (string)"2" , "1=2"         , (Type)null),
                (12, new UriQueryParams()                                   , (string)"1"  , (string)"/" , "1=%2F"       , (Type)null),
                (13, new UriQueryParams()                                   , (string)"+"  , (string)null, "%2B="        , (Type)null),
                (14, new UriQueryParams()                                   , (string)"+"  , (string)"2" , "%2B=2"       , (Type)null),
                (15, new UriQueryParams()                                   , (string)"+"  , (string)"/" , "%2B=%2F"     , (Type)null),
                (16, new UriQueryParams()                                   , (string)"%2B", (string)null, "%252B="      , (Type)null),
                (17, new UriQueryParams()                                   , (string)"%2B", (string)"2" , "%252B=2"     , (Type)null),
                (18, new UriQueryParams()                                   , (string)"%2B", (string)"/" , "%252B=%2F"   , (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?1="))  , (string)"+"  , (string)"2" , "1=&%2B=2"    , (Type)null),
                (21, new UriQueryParams(new Uri("http://example.com/?+="))  , (string)"+"  , (string)"2" , null          , (Type)typeof(ArgumentException)),
                (22, new UriQueryParams(new Uri("http://example.com/?%2B=")), (string)"+"  , (string)"2" , null          , (Type)typeof(ArgumentException)),
                (23, new UriQueryParams(new Uri("http://example.com/?%2B=")), (string)"%2B", (string)"2" , "%2B=&%252B=2", (Type)null),
                // 順序保証。
                (30, new UriQueryParams(new Uri("http://example.com/?b="))  , (string)"a"  , (string)"2" , "b=&a=2"      , (Type)null),
            };
        }

        [TestMethod]
        public void Add_Item() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((ICollection<KeyValuePair<string, string>>)item.query).Add(item.item))
                    .Verify(desc => {
                        Assert.AreEqual(item.expected, item.query.ToString(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, KeyValuePair<string, string> item, string expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new UriQueryParams()                                   , default(KeyValuePair<string, string>)         , null          , (Type)typeof(ArgumentException)),
                ( 1, new UriQueryParams()                                   , new KeyValuePair<string, string>(null ,  "/" ), null          , (Type)typeof(ArgumentException)),
                (10, new UriQueryParams()                                   , new KeyValuePair<string, string>("1"  ,  null), "1="          , (Type)null),
                (11, new UriQueryParams()                                   , new KeyValuePair<string, string>("1"  ,  "2" ), "1=2"         , (Type)null),
                (12, new UriQueryParams()                                   , new KeyValuePair<string, string>("1"  ,  "/" ), "1=%2F"       , (Type)null),
                (13, new UriQueryParams()                                   , new KeyValuePair<string, string>("+"  ,  null), "%2B="        , (Type)null),
                (14, new UriQueryParams()                                   , new KeyValuePair<string, string>("+"  ,  "2" ), "%2B=2"       , (Type)null),
                (15, new UriQueryParams()                                   , new KeyValuePair<string, string>("+"  ,  "/" ), "%2B=%2F"     , (Type)null),
                (16, new UriQueryParams()                                   , new KeyValuePair<string, string>("%2B",  null), "%252B="      , (Type)null),
                (17, new UriQueryParams()                                   , new KeyValuePair<string, string>("%2B",  "2" ), "%252B=2"     , (Type)null),
                (18, new UriQueryParams()                                   , new KeyValuePair<string, string>("%2B",  "/" ), "%252B=%2F"   , (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?1="))  , new KeyValuePair<string, string>("+"  ,  "2" ), "1=&%2B=2"    , (Type)null),
                (21, new UriQueryParams(new Uri("http://example.com/?+="))  , new KeyValuePair<string, string>("+"  ,  "2" ), null          , (Type)typeof(ArgumentException)),
                (22, new UriQueryParams(new Uri("http://example.com/?%2B=")), new KeyValuePair<string, string>("+"  ,  "2" ), null          , (Type)typeof(ArgumentException)),
                (23, new UriQueryParams(new Uri("http://example.com/?%2B=")), new KeyValuePair<string, string>("%2B",  "2" ), "%2B=&%252B=2", (Type)null),
                // 順序保証。
                (30, new UriQueryParams(new Uri("http://example.com/?b="))  , new KeyValuePair<string, string>("a"  ,  "2" ), "b=&a=2"      , (Type)null),
            };
        }

        [TestMethod]
        public void Remove_Key() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.query.Remove(item.key))
                    .Verify((actual, desc) => {
                        Assert.AreEqual(item.expected.result, actual, desc);
                        Assert.AreEqual(item.expected.query, item.query.ToString(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, string key, (bool result, string query) expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new UriQueryParams()                                         , (string)null , (false, null    ), (Type)typeof(ArgumentNullException)),
                (10, new UriQueryParams()                                         , (string)"1"  , (false, ""      ), (Type)null),
                (11, new UriQueryParams(new Uri("http://example.com/?1="))        , (string)"1"  , (true , ""      ), (Type)null),
                (12, new UriQueryParams(new Uri("http://example.com/?+="))        , (string)"+"  , (true , ""      ), (Type)null),
                (13, new UriQueryParams(new Uri("http://example.com/?%2B="))      , (string)"+"  , (true , ""      ), (Type)null),
                (14, new UriQueryParams(new Uri("http://example.com/?%2B="))      , (string)"%2B", (false, "%2B="  ), (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), (string)"a"  , (true , "c=&b=1"), (Type)null),
            };
        }

        [TestMethod]
        public void Remove_Item() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((ICollection<KeyValuePair<string, string>>)item.query).Remove(item.item))
                    .Verify((actual, desc) => {
                        Assert.AreEqual(item.expected.result, actual, desc);
                        Assert.AreEqual(item.expected.query, item.query.ToString(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, KeyValuePair<string, string> item, (bool result, string query) expected, Type expectedExceptionType)[] TestCases() => new[] {
                (10, new UriQueryParams()                                         , new KeyValuePair<string, string>(null, ""  ), (false, ""          ), (Type)null),
                (11, new UriQueryParams()                                         , new KeyValuePair<string, string>("1" , ""  ), (false, ""          ), (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?1="))        , new KeyValuePair<string, string>("1" , ""  ), (true , ""          ), (Type)null),
                (21, new UriQueryParams(new Uri("http://example.com/?1="))        , new KeyValuePair<string, string>("1" , null), (false, "1="        ), (Type)null),
                (22, new UriQueryParams(new Uri("http://example.com/?+="))        , new KeyValuePair<string, string>("+" , ""  ), (true , ""          ), (Type)null),
                (23, new UriQueryParams(new Uri("http://example.com/?+="))        , new KeyValuePair<string, string>("+" , null), (false, "%2B="      ), (Type)null),
                (24, new UriQueryParams(new Uri("http://example.com/?%2B="))      , new KeyValuePair<string, string>("+" , ""  ), (true , ""          ), (Type)null),
                (25, new UriQueryParams(new Uri("http://example.com/?%2B="))      , new KeyValuePair<string, string>("+" , null), (false, "%2B="      ), (Type)null),
                (30, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), new KeyValuePair<string, string>("a" , ""  ), (false, "c=&a=2&b=1"), (Type)null),
                (31, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), new KeyValuePair<string, string>("a" , null), (false, "c=&a=2&b=1"), (Type)null),
                (32, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), new KeyValuePair<string, string>("a" , "2" ), (true , "c=&b=1"    ), (Type)null),
            };
        }

        [TestMethod]
        public void Clear() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.query.Clear())
                    .Verify(desc => {
                        Assert.AreEqual(0, item.query.Count, desc);
                        Assert.AreEqual("", item.query.ToString(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, Type expectedExceptionType)[] TestCases() => new[] {
                (10, new UriQueryParams(new Uri("http://example.com/"))                    , (Type)null),
                (11, new UriQueryParams(new Uri("http://example.com/?"))                   , (Type)null),
                (12, new UriQueryParams(new Uri("http://example.com/?foo"))                , (Type)null),
                (13, new UriQueryParams(new Uri("http://example.com/?foo="))               , (Type)null),
                (14, new UriQueryParams(new Uri("http://example.com/?foo=bar"))            , (Type)null),
                (15, new UriQueryParams(new Uri("http://example.com/?foo=bar&"))           , (Type)null),
                (16, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f"))        , (Type)null),
                (17, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f="))       , (Type)null),
                (18, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f=%26"))    , (Type)null),
                (19, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f=%26#"))   , (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f=%26#%2b")), (Type)null),
            };
        }

        [TestMethod]
        public void Contains() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.query.Contains(item.item))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, KeyValuePair<string, string> item, bool expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new UriQueryParams()                                         , default(KeyValuePair<string, string>)       , false, (Type)null),
                (10, new UriQueryParams()                                         , new KeyValuePair<string, string>("1" , null), false, (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?1="))        , new KeyValuePair<string, string>("1" , null), false, (Type)null),
                (21, new UriQueryParams(new Uri("http://example.com/?1="))        , new KeyValuePair<string, string>("1" , ""  ), true , (Type)null),
                (22, new UriQueryParams(new Uri("http://example.com/?+="))        , new KeyValuePair<string, string>("+" , null), false, (Type)null),
                (23, new UriQueryParams(new Uri("http://example.com/?+="))        , new KeyValuePair<string, string>("+" , ""  ), true , (Type)null),
                (24, new UriQueryParams(new Uri("http://example.com/?%2B="))      , new KeyValuePair<string, string>("+" , null), false, (Type)null),
                (25, new UriQueryParams(new Uri("http://example.com/?%2B="))      , new KeyValuePair<string, string>("+" , ""  ), true , (Type)null),
                (30, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), new KeyValuePair<string, string>("a" , null), false, (Type)null),
                (30, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), new KeyValuePair<string, string>("a" , ""  ), false, (Type)null),
                (31, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), new KeyValuePair<string, string>("a" , "2" ), true , (Type)null),
            };
        }

        [TestMethod]
        public void ContainsKey() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.query.ContainsKey(item.key))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, string key, bool expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new UriQueryParams()                                         , (string)null , false, (Type)typeof(ArgumentNullException)),
                (10, new UriQueryParams()                                         , (string)"1"  , false, (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?1="))        , (string)"+"  , false, (Type)null),
                (21, new UriQueryParams(new Uri("http://example.com/?+="))        , (string)"+"  , true , (Type)null),
                (22, new UriQueryParams(new Uri("http://example.com/?%2B="))      , (string)"+"  , true , (Type)null),
                (23, new UriQueryParams(new Uri("http://example.com/?%2B="))      , (string)"%2B", false, (Type)null),
                (30, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), (string)"a"  , true , (Type)null),
                (31, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), (string)"b"  , true , (Type)null),
                (32, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), (string)"c"  , true , (Type)null),
                (33, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), (string)"d"  , false, (Type)null),
            };
        }

        [TestMethod]
        public void TryGetValue() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => (item.query.TryGetValue(item.key, out var value), value))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, string key, (bool result, string value) expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new UriQueryParams()                                         , (string)null , (false, null), (Type)typeof(ArgumentNullException)),
                (10, new UriQueryParams()                                         , (string)"1"  , (false, null), (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?1="))        , (string)"+"  , (false, null), (Type)null),
                (21, new UriQueryParams(new Uri("http://example.com/?+="))        , (string)"+"  , (true , ""  ), (Type)null),
                (22, new UriQueryParams(new Uri("http://example.com/?%2B="))      , (string)"+"  , (true , ""  ), (Type)null),
                (23, new UriQueryParams(new Uri("http://example.com/?%2B="))      , (string)"%2B", (false, null), (Type)null),
                (30, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), (string)"a"  , (true , "2" ), (Type)null),
                (31, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), (string)"b"  , (true , "1" ), (Type)null),
                (32, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), (string)"c"  , (true , ""  ), (Type)null),
                (33, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), (string)"d"  , (false, null), (Type)null),
            };
        }

        [TestMethod]
        public void CopyTo() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((ICollection<KeyValuePair<string, string>>)item.query).CopyTo(item.array, item.arrayIndex))
                    .Verify(desc => {
                        CollectionAssert.AreEqual(item.expected, item.array, desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, KeyValuePair<string, string>[] array, int arrayIndex, KeyValuePair<string, string>[] expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new UriQueryParams()                                         , null     , 0 , null                                   , (Type)typeof(ArgumentNullException)),
                ( 1, new UriQueryParams()                                         , ItemsB(0), -1, null                                   , (Type)typeof(ArgumentOutOfRangeException)),
                ( 2, new UriQueryParams()                                         , ItemsB(0), 1 , null                                   , (Type)typeof(ArgumentException)),
                (10, new UriQueryParams()                                         , ItemsB(0), 0 , ItemsN()                               , (Type)null),
                (11, new UriQueryParams()                                         , ItemsB(1), 0 , ItemsB(1)                              , (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?1="))        , ItemsB(0), 0 , null                                   , (Type)typeof(ArgumentException)),
                (21, new UriQueryParams(new Uri("http://example.com/?1="))        , ItemsB(1), 0 , ItemsN(("1", ""))                      , (Type)null),
                (22, new UriQueryParams(new Uri("http://example.com/?1="))        , ItemsB(2), 0 , ItemsN(("1", ""), default)             , (Type)null),
                (23, new UriQueryParams(new Uri("http://example.com/?1="))        , ItemsB(2), 1 , ItemsN(default, ("1", ""))             , (Type)null),
                (24, new UriQueryParams(new Uri("http://example.com/?+="))        , ItemsB(1), 0 , ItemsN(("+", ""))                      , (Type)null),
                (25, new UriQueryParams(new Uri("http://example.com/?%2B="))      , ItemsB(1), 0 , ItemsN(("+", ""))                      , (Type)null),
                (30, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), ItemsB(2), 0 , null                                   , (Type)typeof(ArgumentException)),
                (31, new UriQueryParams(new Uri("http://example.com/?c=&a=2&b=1")), ItemsB(3), 0 , ItemsN(("c", ""),("a", "2"),("b", "1")), (Type)null),
            };

            KeyValuePair<string, string>[] ItemsB(int count) => new KeyValuePair<string, string>[count];
            KeyValuePair<string, string>[] ItemsN(params (string key, string value)[] items) => items.Select(item => KeyValuePair.Create(item.key, item.value)).ToArray();
        }

        [TestMethod]
        public void GetEnumerator() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.query.GetEnumerator())
                    .Verify((actual, desc) => {
                        var list = new List<KeyValuePair<string, string>>();
                        while (actual.MoveNext()) {
                            list.Add(actual.Current);
                        }
                        CollectionAssert.AreEqual(item.expected, list, desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, KeyValuePair<string, string>[] expected, Type expectedExceptionType)[] TestCases() => new[] {
                (10, new UriQueryParams(new Uri("http://example.com/"))                    , Items()                                   , (Type)null),
                (11, new UriQueryParams(new Uri("http://example.com/?"))                   , Items()                                   , (Type)null),
                (12, new UriQueryParams(new Uri("http://example.com/?foo"))                , Items(("foo",""))                         , (Type)null),
                (13, new UriQueryParams(new Uri("http://example.com/?foo="))               , Items(("foo",""))                         , (Type)null),
                (14, new UriQueryParams(new Uri("http://example.com/?foo=bar"))            , Items(("foo","bar"))                      , (Type)null),
                (15, new UriQueryParams(new Uri("http://example.com/?foo=bar&"))           , Items(("foo","bar"))                      , (Type)null),
                (16, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f"))        , Items(("foo","bar"), ("?",""))            , (Type)null),
                (17, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f="))       , Items(("foo","bar"), ("?",""))            , (Type)null),
                (18, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f=%26"))    , Items(("foo","bar"), ("?","&"))           , (Type)null),
                (19, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f=%26&a=b")), Items(("foo","bar"), ("?","&"), ("a","b")), (Type)null),
            };

            KeyValuePair<string, string>[] Items(params (string key, string value)[] items) => items.Select(item => KeyValuePair.Create(item.key, item.value)).ToArray();
        }

        [TestMethod]
        public void IsReadOnly() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((ICollection<KeyValuePair<string, string>>)item.query).IsReadOnly)
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, UriQueryParams query, bool expected, Type expectedExceptionType)[] TestCases() => new[] {
                (10, new UriQueryParams()                                                  , false, (Type)null),
                (20, new UriQueryParams(new Uri("http://example.com/?foo=bar&%3f=%26#%2b")), false, (Type)null),
            };
        }

        [TestMethod]
        public void GetItem() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SetItem() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Keys() {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Values() {
            throw new NotImplementedException();
        }
    }
}