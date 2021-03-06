﻿using System;
using Inasync;

namespace Microsoft.VisualStudio.TestTools.UnitTesting {

    public static class TestActualExtensions {

        /// <summary>
        /// テスト対象コードの実行結果を検証します。
        /// </summary>
        /// <typeparam name="TResult">テスト対象コードの戻り値の型。</typeparam>
        /// <param name="actual">テスト対象コードの実行結果。</param>
        /// <param name="expectedResult">テスト対象コードの戻り値として期待される値。</param>
        /// <param name="expectedExceptionType">テスト対象コードによって生じる事が期待される例外の型。</param>
        /// <exception cref="ArgumentNullException"><paramref name="actual"/> is <c>null</c>.</exception>
        public static void Verify<TResult>(this ITestActual<TResult[]> actual, TResult[] expectedResult, Type expectedExceptionType) {
            if (actual == null) { throw new ArgumentNullException(nameof(actual)); }

            actual.Verify(
                (result, description) => CollectionAssert.AreEqual(expectedResult, result, description),
                expectedExceptionType
            );
        }
    }
}
