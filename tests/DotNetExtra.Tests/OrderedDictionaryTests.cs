using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Inasync;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Inasync.Tests {

    [TestClass]
    public class OrderedDictionaryTests {

        [TestMethod]
        public void OrderedDictionary() {
            new TestCaseRunner()
                .Run(() => new OrderedDictionary<string, int>())
                .Verify((actual, desc) => {
                    Assert.AreEqual(0, actual.Count, desc);
                    Assert.AreEqual(EqualityComparer<string>.Default, actual.Comparer, desc);
                }, (Type)null);
        }

        [TestMethod]
        public void OrderedDictionary_Comparer() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => new OrderedDictionary<string, int>(item.comparer))
                    .Verify((actual, desc) => {
                        Assert.AreEqual(0, actual.Count, desc);
                        Assert.AreEqual(item.expectedComparer, actual.Comparer, desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, IEqualityComparer<string> comparer, IEqualityComparer<string> expectedComparer, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, (IEqualityComparer<string>)null                            , (IEqualityComparer<string>)EqualityComparer<string>.Default, (Type)null),
                ( 1, (IEqualityComparer<string>)EqualityComparer<string>.Default, (IEqualityComparer<string>)EqualityComparer<string>.Default, (Type)null),
                ( 2, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase, (Type)null),
            };
        }

        [TestMethod]
        public void Keys() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.dic.Keys)
                    .Verify(item.expected, item.expectedExceptionType);

                // IReadOnlyDictionary.Keys
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((IReadOnlyDictionary<string, string>)item.dic).Keys)
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, string[] expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()                                     , new string[]{}               , (Type)null),
                (10, new OrderedDictionary<string, string>{{"foo",null }}                        , new string[]{"foo"}          , (Type)null),
                (11, new OrderedDictionary<string, string>{{"foo",""   }}                        , new string[]{"foo"}          , (Type)null),
                (12, new OrderedDictionary<string, string>{{"foo","bar"}}                        , new string[]{"foo"}          , (Type)null),
                (20, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f",null }}          , new string[]{"foo","%3f"}    , (Type)null),
                (21, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f",""   }}          , new string[]{"foo","%3f"}    , (Type)null),
                (22, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f","%26"}}          , new string[]{"foo","%3f"}    , (Type)null),
                (30, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f","%26"},{"a","b"}}, new string[]{"foo","%3f","a"}, (Type)null),
            };
        }

        [TestMethod]
        public void Values() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.dic.Values)
                    .Verify(item.expected, item.expectedExceptionType);

                // IReadOnlyDictionary.Values
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((IReadOnlyDictionary<string, string>)item.dic).Values)
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, string[] expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()                                     , new string[]{}               , (Type)null),
                (10, new OrderedDictionary<string, string>{{"foo",null }}                        , new string[]{null }          , (Type)null),
                (11, new OrderedDictionary<string, string>{{"foo",""   }}                        , new string[]{""   }          , (Type)null),
                (12, new OrderedDictionary<string, string>{{"foo","bar"}}                        , new string[]{"bar"}          , (Type)null),
                (20, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f",null }}          , new string[]{"bar",null }    , (Type)null),
                (21, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f",""   }}          , new string[]{"bar",""   }    , (Type)null),
                (22, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f","%26"}}          , new string[]{"bar","%26"}    , (Type)null),
                (30, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f","%26"},{"a","b"}}, new string[]{"bar","%26","b"}, (Type)null),
            };
        }

        [TestMethod]
        public void Count() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.dic.Count)
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, int expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()                                     , 0, (Type)null),
                (10, new OrderedDictionary<string, string>{{"foo",null }}                        , 1, (Type)null),
                (11, new OrderedDictionary<string, string>{{"foo",""   }}                        , 1, (Type)null),
                (12, new OrderedDictionary<string, string>{{"foo","bar"}}                        , 1, (Type)null),
                (20, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f",null }}          , 2, (Type)null),
                (21, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f",""   }}          , 2, (Type)null),
                (22, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f","%26"}}          , 2, (Type)null),
                (30, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f","%26"},{"a","b"}}, 3, (Type)null),
            };
        }

        [TestMethod]
        public void Collection_IsReadOnly() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((ICollection<KeyValuePair<string, string>>)item.dic).IsReadOnly)
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, bool expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()                           , false, (Type)null),
                ( 1, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f","%26"}}, false, (Type)null),
            };
        }

        [TestMethod]
        public void Add_Key_OValue() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.dic.Add(item.input.key, item.input.value))
                    .Verify(desc => {
                        CollectionAssert.AreEqual(item.expected.ToArray(), item.dic.Cast<KeyValuePair<string, object>>().ToArray(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, object> dic, (string key, object value) input, IEnumerable<KeyValuePair<string, object>> expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, object>()            , (null , (object)"/" ), default                   , (Type)typeof(ArgumentNullException)),
                (10, new OrderedDictionary<string, object>()            , (""   , (object)null), KVPs((""   ,null))        , (Type)null),
                (11, new OrderedDictionary<string, object>()            , ("1"  , (object)null), KVPs(("1"  ,null))        , (Type)null),
                (12, new OrderedDictionary<string, object>()            , ("+"  , (object)2   ), KVPs(("+"  ,2))           , (Type)null),
                (13, new OrderedDictionary<string, object>()            , ("%2B", (object)"/" ), KVPs(("%2B","/"))         , (Type)null),
                (20, new OrderedDictionary<string, object>{{"1"  ,null}}, ("+"  , (object)2   ), KVPs(("1"  ,null),("+",2)), (Type)null),
                (21, new OrderedDictionary<string, object>{{"+"  ,null}}, ("+"  , (object)2   ), default                   , (Type)typeof(ArgumentException)),
                (22, new OrderedDictionary<string, object>{{"%2B",null}}, ("+"  , (object)2   ), KVPs(("%2B",null),("+",2)), (Type)null),
                (23, new OrderedDictionary<string, object>{{"%2B",null}}, ("%2B", (object)2   ), default                   , (Type)typeof(ArgumentException)),
                // 順序保証。
                (30, new OrderedDictionary<string, object>{{"b"  ,null}}, ("a"  , (object)2   ), KVPs(("b"  ,null),("a",2)), (Type)null),
            };

            IEnumerable<KeyValuePair<string, object>> KVPs(params (string key, object value)[] items) => items.Select(item => KeyValuePair.Create(item.key, item.value));
        }

        [TestMethod]
        public void Add_Key_SValue() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.dic.Add(item.input.key, item.input.value))
                    .Verify(desc => {
                        CollectionAssert.AreEqual(item.expected.ToArray(), item.dic.Cast<KeyValuePair<string, string>>().ToArray(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, (string key, string value) input, IEnumerable<KeyValuePair<string, string>> expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()            , (null , "/" ), default                     , (Type)typeof(ArgumentNullException)),
                (10, new OrderedDictionary<string, string>()            , (""   , null), KVPs((""   ,null))          , (Type)null),
                (11, new OrderedDictionary<string, string>()            , ("1"  , null), KVPs(("1"  ,null))          , (Type)null),
                (12, new OrderedDictionary<string, string>()            , ("+"  , "2" ), KVPs(("+"  ,"2" ))          , (Type)null),
                (13, new OrderedDictionary<string, string>()            , ("%2B", "/" ), KVPs(("%2B","/" ))          , (Type)null),
                (20, new OrderedDictionary<string, string>{{"1"  ,null}}, ("+"  , "2" ), KVPs(("1"  ,null),("+","2")), (Type)null),
                (21, new OrderedDictionary<string, string>{{"+"  ,null}}, ("+"  , "2" ), default                     , (Type)typeof(ArgumentException)),
                (22, new OrderedDictionary<string, string>{{"%2B",null}}, ("+"  , "2" ), KVPs(("%2B",null),("+","2")), (Type)null),
                (23, new OrderedDictionary<string, string>{{"%2B",null}}, ("%2B", "2" ), default                     , (Type)typeof(ArgumentException)),
                // 順序保証。
                (30, new OrderedDictionary<string, string>{{"b"  ,null}}, ("a"  , "2") , KVPs(("b"  ,null),("a","2")), (Type)null),
            };

            IEnumerable<KeyValuePair<string, string>> KVPs(params (string key, string value)[] items) => items.Select(item => KeyValuePair.Create(item.key, item.value));
        }

        [TestMethod]
        public void Collection_Add() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((ICollection<KeyValuePair<string, string>>)item.dic).Add(item.input))
                    .Verify(desc => {
                        CollectionAssert.AreEqual(item.expected.ToArray(), item.dic.Cast<KeyValuePair<string, string>>().ToArray(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, KeyValuePair<string, string> input, IEnumerable<KeyValuePair<string, string>> expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()            , KVP(null , "/" ), default                     , (Type)typeof(ArgumentException)),
                (10, new OrderedDictionary<string, string>()            , KVP(""   , null), KVPs((""   ,null))          , (Type)null),
                (11, new OrderedDictionary<string, string>()            , KVP("1"  , null), KVPs(("1"  ,null))          , (Type)null),
                (12, new OrderedDictionary<string, string>()            , KVP("+"  , "2" ), KVPs(("+"  ,"2" ))          , (Type)null),
                (13, new OrderedDictionary<string, string>()            , KVP("%2B", "/" ), KVPs(("%2B","/" ))          , (Type)null),
                (20, new OrderedDictionary<string, string>{{"1"  ,null}}, KVP("+"  , "2" ), KVPs(("1"  ,null),("+","2")), (Type)null),
                (21, new OrderedDictionary<string, string>{{"+"  ,null}}, KVP("+"  , "2" ), default                     , (Type)typeof(ArgumentException)),
                (22, new OrderedDictionary<string, string>{{"%2B",null}}, KVP("+"  , "2" ), KVPs(("%2B",null),("+","2")), (Type)null),
                (23, new OrderedDictionary<string, string>{{"%2B",null}}, KVP("%2B", "2" ), default                     , (Type)typeof(ArgumentException)),
                // 順序保証。
                (30, new OrderedDictionary<string, string>{{"b"  ,null}}, KVP("a"  , "2" ), KVPs(("b"  ,null),("a","2")), (Type)null),
            };

            IEnumerable<KeyValuePair<string, string>> KVPs(params (string key, string value)[] items) => items.Select(item => KeyValuePair.Create(item.key, item.value));
            KeyValuePair<string, string> KVP(string key, string value) => KeyValuePair.Create(key, value);
        }

        [TestMethod]
        public void Remove() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.dic.Remove(item.key))
                    .Verify((actual, desc) => {
                        Assert.AreEqual(item.expected.result, actual, desc);
                        CollectionAssert.AreEqual(item.expected.keyValues.ToArray(), item.dic.Cast<KeyValuePair<string, string>>().ToArray(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, string key, (bool result, IEnumerable<KeyValuePair<string, string>> keyValues) expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()                                , (string)null , default                              , (Type)typeof(ArgumentNullException)),
                (10, new OrderedDictionary<string, string>()                                , (string)""   , (false, KVPs())                      , (Type)null),
                (11, new OrderedDictionary<string, string>()                                , (string)"1"  , (false, KVPs())                      , (Type)null),
                (20, new OrderedDictionary<string, string>{{"1"  ,null}}                    , (string)"1"  , (true , KVPs())                      , (Type)null),
                (21, new OrderedDictionary<string, string>{{"+"  ,null}}                    , (string)"+"  , (true , KVPs())                      , (Type)null),
                (22, new OrderedDictionary<string, string>{{"%2B",null}}                    , (string)"+"  , (false, KVPs(("%2B",null)))          , (Type)null),
                (23, new OrderedDictionary<string, string>{{"%2B",null}}                    , (string)"%2B", (true , KVPs())                      , (Type)null),
                // 順序保証。
                (30, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, (string)"a"  , (true , KVPs(("c"  ,null),("b","1"))), (Type)null),
            };

            IEnumerable<KeyValuePair<string, string>> KVPs(params (string key, string value)[] items) => items.Select(item => KeyValuePair.Create(item.key, item.value));
        }

        [TestMethod]
        public void Collection_Remove() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((ICollection<KeyValuePair<string, string>>)item.dic).Remove(item.input))
                    .Verify((actual, desc) => {
                        Assert.AreEqual(item.expected.result, actual, desc);
                        CollectionAssert.AreEqual(item.expected.keyValues.ToArray(), item.dic.Cast<KeyValuePair<string, string>>().ToArray(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, KeyValuePair<string, string> input, (bool result, IEnumerable<KeyValuePair<string, string>> keyValues) expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()                                , KVP(null,""  ), default                                        , (Type)typeof(ArgumentNullException)),
                (10, new OrderedDictionary<string, string>()                                , KVP(""  ,""  ), (false, KVPs())                                , (Type)null),
                (11, new OrderedDictionary<string, string>()                                , KVP("1" ,""  ), (false, KVPs())                                , (Type)null),
                (20, new OrderedDictionary<string, string>{{"1"  ,null}}                    , KVP("1" ,""  ), (false, KVPs(("1"  ,null)))                    , (Type)null),
                (21, new OrderedDictionary<string, string>{{"1"  ,null}}                    , KVP("1" ,null), (true , KVPs())                                , (Type)null),
                (22, new OrderedDictionary<string, string>{{"+"  ,null}}                    , KVP("+" ,""  ), (false, KVPs(("+"  ,null)))                    , (Type)null),
                (23, new OrderedDictionary<string, string>{{"+"  ,null}}                    , KVP("+" ,null), (true , KVPs())                                , (Type)null),
                (24, new OrderedDictionary<string, string>{{"%2B",null}}                    , KVP("+" ,""  ), (false, KVPs(("%2B",null)))                    , (Type)null),
                (25, new OrderedDictionary<string, string>{{"%2B",null}}                    , KVP("+" ,null), (false, KVPs(("%2B",null)))                    , (Type)null),
                // 順序保証。
                (30, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, KVP("a" ,""  ), (false, KVPs(("c"  ,null),("a","2"),("b","1"))), (Type)null),
                (31, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, KVP("a" ,null), (false, KVPs(("c"  ,null),("a","2"),("b","1"))), (Type)null),
                (32, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, KVP("a" ,"2" ), (true , KVPs(("c"  ,null),("b","1")))          , (Type)null),
            };

            IEnumerable<KeyValuePair<string, string>> KVPs(params (string key, string value)[] items) => items.Select(item => KeyValuePair.Create(item.key, item.value));
            KeyValuePair<string, string> KVP(string key, string value) => KeyValuePair.Create(key, value);
        }

        [TestMethod]
        public void Clear() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.dic.Clear())
                    .Verify(desc => {
                        Assert.AreEqual(0, item.dic.Count, desc);
                        CollectionAssert.AreEqual(Array.Empty<KeyValuePair<string, string>>(), item.dic.Cast<KeyValuePair<string, string>>().ToArray(), desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, Type expectedExceptionType)[] TestCases() => new[] {
                (10, new OrderedDictionary<string, string>()                           , (Type)null),
                (11, new OrderedDictionary<string, string>{{"foo",null }}              , (Type)null),
                (12, new OrderedDictionary<string, string>{{"foo","bar"}}              , (Type)null),
                (13, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f",null }}, (Type)null),
                (14, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f","%26"}}, (Type)null),
            };
        }

        [TestMethod]
        public void Collection_Contains() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((ICollection<KeyValuePair<string, string>>)item.dic).Contains(item.input))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, KeyValuePair<string, string> input, bool expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()                                , default      , false, (Type)typeof(ArgumentNullException)),
                (10, new OrderedDictionary<string, string>()                                , KVP("" ,null), false, (Type)null),
                (11, new OrderedDictionary<string, string>()                                , KVP("1",null), false, (Type)null),
                (20, new OrderedDictionary<string, string>{{"1"  ,null}}                    , KVP("1",null), true , (Type)null),
                (21, new OrderedDictionary<string, string>{{"1"  ,null}}                    , KVP("1",""  ), false, (Type)null),
                (22, new OrderedDictionary<string, string>{{"+"  ,null}}                    , KVP("+",null), true , (Type)null),
                (23, new OrderedDictionary<string, string>{{"+"  ,null}}                    , KVP("+",""  ), false, (Type)null),
                (24, new OrderedDictionary<string, string>{{"%2B",null}}                    , KVP("+",null), false, (Type)null),
                (25, new OrderedDictionary<string, string>{{"%2B",null}}                    , KVP("+",""  ), false, (Type)null),
                (30, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, KVP("a",null), false, (Type)null),
                (30, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, KVP("a",""  ), false, (Type)null),
                (31, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, KVP("a","2" ), true , (Type)null),
            };

            KeyValuePair<string, string> KVP(string key, string value) => KeyValuePair.Create(key, value);
        }

        [TestMethod]
        public void ContainsKey() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.dic.ContainsKey(item.key))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, string key, bool expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()                                , default      , false, (Type)typeof(ArgumentNullException)),
                (10, new OrderedDictionary<string, string>()                                , (string)""   , false, (Type)null),
                (11, new OrderedDictionary<string, string>()                                , (string)"1"  , false, (Type)null),
                (20, new OrderedDictionary<string, string>{{"1"  ,null}}                    , (string)"+"  , false, (Type)null),
                (21, new OrderedDictionary<string, string>{{"+"  ,null}}                    , (string)"+"  , true , (Type)null),
                (22, new OrderedDictionary<string, string>{{"%2B",null}}                    , (string)"+"  , false, (Type)null),
                (23, new OrderedDictionary<string, string>{{"%2B",null}}                    , (string)"%2B", true , (Type)null),
                (30, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, (string)"a"  , true , (Type)null),
                (31, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, (string)"b"  , true , (Type)null),
                (32, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, (string)"c"  , true , (Type)null),
                (33, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, (string)"d"  , false, (Type)null),
            };
        }

        [TestMethod]
        public void TryGetValue() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => (item.dic.TryGetValue(item.key, out var value), value))
                    .Verify(item.expected, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, string key, (bool result, string value) expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()                                , (string)null , default      , (Type)typeof(ArgumentNullException)),
                (10, new OrderedDictionary<string, string>()                                , (string)""   , (false, null), (Type)null),
                (11, new OrderedDictionary<string, string>()                                , (string)"1"  , (false, null), (Type)null),
                (20, new OrderedDictionary<string, string>{{"1"  ,null}}                    , (string)"+"  , (false, null), (Type)null),
                (21, new OrderedDictionary<string, string>{{"+"  ,null}}                    , (string)"+"  , (true , null), (Type)null),
                (22, new OrderedDictionary<string, string>{{"%2B",null}}                    , (string)"+"  , (false, null), (Type)null),
                (23, new OrderedDictionary<string, string>{{"%2B",null}}                    , (string)"%2B", (true , null), (Type)null),
                (30, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, (string)"a"  , (true , "2" ), (Type)null),
                (31, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, (string)"b"  , (true , "1" ), (Type)null),
                (32, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, (string)"c"  , (true , null), (Type)null),
                (33, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, (string)"d"  , (false, null), (Type)null),
            };
        }

        [TestMethod]
        public void Collection_CopyTo() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((ICollection<KeyValuePair<string, string>>)item.dic).CopyTo(item.input.array, item.input.arrayIndex))
                    .Verify(desc => {
                        CollectionAssert.AreEqual(item.expected, item.input.array, desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, (KeyValuePair<string, string>[] array, int arrayIndex) input, KeyValuePair<string, string>[] expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()                                , (null     , 0 ), default                                , (Type)typeof(ArgumentNullException)),
                ( 1, new OrderedDictionary<string, string>()                                , (KVPAry(0), -1), default                                , (Type)typeof(ArgumentOutOfRangeException)),
                ( 2, new OrderedDictionary<string, string>()                                , (KVPAry(0), 1 ), default                                , (Type)typeof(ArgumentOutOfRangeException)),
                (10, new OrderedDictionary<string, string>()                                , (KVPAry(0), 0 ), KVPs()                                 , (Type)null),
                (11, new OrderedDictionary<string, string>()                                , (KVPAry(1), 0 ), KVPAry(1)                              , (Type)null),
                (20, new OrderedDictionary<string, string>{{"1"  ,null}}                    , (KVPAry(0), 0 ), default                                , (Type)typeof(ArgumentException)),
                (21, new OrderedDictionary<string, string>{{"1"  ,null}}                    , (KVPAry(1), 0 ), KVPs(("1"  ,null))                     , (Type)null),
                (22, new OrderedDictionary<string, string>{{"1"  ,null}}                    , (KVPAry(2), 0 ), KVPs(("1"  ,null),default)             , (Type)null),
                (23, new OrderedDictionary<string, string>{{"1"  ,null}}                    , (KVPAry(2), 1 ), KVPs(default     ,("1",null))          , (Type)null),
                (24, new OrderedDictionary<string, string>{{"+"  ,null}}                    , (KVPAry(1), 0 ), KVPs(("+"  ,null))                     , (Type)null),
                (25, new OrderedDictionary<string, string>{{"%2B",null}}                    , (KVPAry(1), 0 ), KVPs(("%2B",null))                     , (Type)null),
                (30, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, (KVPAry(2), 0 ), default                                , (Type)typeof(ArgumentException)),
                (31, new OrderedDictionary<string, string>{{"c"  ,null},{"a","2"},{"b","1"}}, (KVPAry(3), 0 ), KVPs(("c"  ,null),("a","2" ),("b","1")), (Type)null),
            };

            KeyValuePair<string, string>[] KVPAry(int count) => new KeyValuePair<string, string>[count];
            KeyValuePair<string, string>[] KVPs(params (string key, string value)[] items) => items.Select(item => KeyValuePair.Create(item.key, item.value)).ToArray();
        }

        [TestMethod]
        public void GetEnumerator() {
            foreach (var item in TestCases()) {
                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => item.dic.GetEnumerator())
                    .Verify((actual, desc) => {
                        var actualList = new List<KeyValuePair<string, string>>();
                        while (actual.MoveNext()) {
                            actualList.Add(actual.Current);
                        }
                        CollectionAssert.AreEqual(item.expected, actualList, desc);
                    }, item.expectedExceptionType);

                new TestCaseRunner($"No.{item.testNumber}")
                    .Run(() => ((IEnumerable)item.dic).GetEnumerator())
                    .Verify((actual, desc) => {
                        var actualList = new ArrayList();
                        while (actual.MoveNext()) {
                            actualList.Add(actual.Current);
                        }
                        CollectionAssert.AreEqual(item.expected, actualList, desc);
                    }, item.expectedExceptionType);
            }

            (int testNumber, OrderedDictionary<string, string> dic, KeyValuePair<string, string>[] expected, Type expectedExceptionType)[] TestCases() => new[] {
                ( 0, new OrderedDictionary<string, string>()                                     , KVPs()                                     , (Type)null),
                (10, new OrderedDictionary<string, string>{{"foo",null }}                        , KVPs(("foo",null ))                        , (Type)null),
                (11, new OrderedDictionary<string, string>{{"foo",""   }}                        , KVPs(("foo",""   ))                        , (Type)null),
                (12, new OrderedDictionary<string, string>{{"foo","bar"}}                        , KVPs(("foo","bar"))                        , (Type)null),
                (20, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f",null }}          , KVPs(("foo","bar"),("%3f",null ))          , (Type)null),
                (21, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f",""   }}          , KVPs(("foo","bar"),("%3f",""   ))          , (Type)null),
                (22, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f","%26"}}          , KVPs(("foo","bar"),("%3f","%26"))          , (Type)null),
                (30, new OrderedDictionary<string, string>{{"foo","bar"},{"%3f","%26"},{"a","b"}}, KVPs(("foo","bar"),("%3f","%26"),("a","b")), (Type)null),
            };

            KeyValuePair<string, string>[] KVPs(params (string key, string value)[] items) => items.Select(item => KeyValuePair.Create(item.key, item.value)).ToArray();
        }
    }
}
