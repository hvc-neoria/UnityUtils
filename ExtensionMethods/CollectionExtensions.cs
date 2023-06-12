using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HvcNeoria.Unity.Utils
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// リストをシャッフルする。
        /// フィッシャー–イェーツのシャッフル。改良バージョン。
        /// 参考：https://ja.wikipedia.org/wiki/%E3%83%95%E3%82%A3%E3%83%83%E3%82%B7%E3%83%A3%E3%83%BC%E2%80%93%E3%82%A4%E3%82%A7%E3%83%BC%E3%83%84%E3%81%AE%E3%82%B7%E3%83%A3%E3%83%83%E3%83%95%E3%83%AB
        /// </summary>
        /// <param name="iEnumerable">コレクション</param>
        /// <typeparam name="T">コレクションの要素の型</typeparam>
        /// <returns>シャッフルされた配列</returns>
        public static T[] Shuffle<T>(this IEnumerable<T> iEnumerable)
        {
            var array = iEnumerable.ToArray();

            for (int i = 0; i < array.Length - 2; i++)
            {
                var j = Random.Range(i, array.Length);

                // i番目とj番目の要素を交換する
                var tmp = array[i];
                array[i] = array[j];
                array[j] = tmp;
            }

            return array;
        }

        /// <summary>
        /// floatリストの重複削除。
        /// floatの同値判定は性質上うまくいかないことが多いので、
        /// ほぼ同じ値であることを判定するMathf.Approximately()を使用している。
        /// </summary>
        /// <param name="floats">floatのIEnumerable</param>
        /// <returns>重複削除されたfloatのList</returns>
        public static List<float> Distinct(this IEnumerable<float> floats)
        {
            var result = new List<float>();

            foreach (var item in floats)
            {
                if (result.Count == 0)
                {
                    result.Add(item);
                    continue;
                }
                if (!Mathf.Approximately(result.Last(), item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// ForEachのワンライナー。
        /// 配列に対して使用したかったため作成。
        /// </summary>
        /// <param name="iEnumerable">コレクション</param>
        /// <param name="action">各要素に実行したいアクション</param>
        /// <typeparam name="T">コレクションの要素の型</typeparam>
        public static void ForEach<T>(this IEnumerable<T> iEnumerable, Action<T> action)
        {
            foreach (T item in iEnumerable)
            {
                action(item);
            }
        }

        /// <summary>
        /// コレクションの要素をカンマ区切りでデバッグログに出力する。
        /// </summary>
        /// <param name="iEnumerable">コレクション</param>
        /// <typeparam name="T">コレクションの要素の型</typeparam>
        /// <returns>無加工のコレクション</returns>
        public static IEnumerable<T> DebugLog<T>(this IEnumerable<T> iEnumerable)
        {
            Debug.Log(string.Join(", ", iEnumerable));
            return iEnumerable;
        }
    }
}
