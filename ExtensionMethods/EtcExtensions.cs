using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

namespace HvcNeoria.Unity.Utils
{
    public static class EtcExtensions
    {
        /// <summary>
        /// 反射ベクトル。
        /// 返り値のベクトルの始点を、衝突元のコライダーの中心とすると、
        /// 衝突元のコライダーの軌道予測線となる。
        /// </summary>
        /// <param name="targetCollision">衝突先のコリジョン</param>
        /// <returns>反射ベクトル</returns>
        public static Vector3 ReflectionVector(this Collision targetCollision)
        {
            Vector3 inVector = targetCollision.relativeVelocity;
            Vector3 normalizedNormal = targetCollision.impulse.normalized;
            Vector3 outVector = Vector3.Reflect(-inVector, normalizedNormal);
            return outVector;
        }

        /// <summary>
        /// GetPositions()のワンライナー。
        /// </summary>
        /// <param name="lineRenderer">LineRenderer</param>
        /// <returns>Vector3の配列</returns>
        public static Vector3[] GetPositions(this LineRenderer lineRenderer)
        {
            var positions = new Vector3[lineRenderer.positionCount];
            // positionsに値が返されるのだが、outの記載が不要なため紛らわしい。
            lineRenderer.GetPositions(positions);
            return positions;
        }

        /// <summary>
        /// CSVを文字列の2次元配列に変換する。
        /// </summary>
        /// <param name="csvFile">CSVファイル</param>
        /// <returns>文字列の2次元配列</returns>
        public static string[][] ToStrings(this TextAsset csvFile)
        {
            var csvDatas = new List<string[]>();

            using (var reader = new StringReader(csvFile.text))
            {
                // 次の文字がなくなるまで繰り返す
                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine();
                    string[] strings = line.Split(',');
                    csvDatas.Add(strings);
                }
            }

            return csvDatas.ToArray();
        }

        /// <summary>
        /// CSVをDictionaryの配列に変換する。
        /// 1行目の値がキーとなる。
        /// </summary>
        /// <param name="csvFile">CSVファイル</param>
        /// <returns>Dictionaryの配列</returns>
        public static Dictionary<string, string>[] ToDictionaries(this TextAsset csvFile)
        {
            var csvDatas = new List<Dictionary<string, string>>();
            string[] keys = null;

            using (var reader = new StringReader(csvFile.text))
            {
                // 次の文字がなくなるまで繰り返す
                while (reader.Peek() != -1)
                {
                    string line = reader.ReadLine();
                    string[] strings = line.Split(',');

                    if (keys == null)
                    {
                        keys = strings;
                        continue;
                    }

                    var dictionary = Enumerable.Range(0, keys.Length).ToDictionary(i => keys[i], i => strings[i]);
                    csvDatas.Add(dictionary);
                }
            }

            return csvDatas.ToArray();
        }

        /// <summary>
        /// メソッドチェーンが可能なDebug.Log()。
        /// </summary>
        /// <param name="obj">オブジェクト</param>
        /// <returns>オブジェクト</returns>
        public static object DebugLog(this object obj)
        {
            Debug.Log(obj);
            return obj;
        }

        /// <summary>
        /// カラーコードの文字列をカラーに変換する。
        /// 「#」の有無は問わない。
        /// </summary>
        /// <param name="colorCode">カラーコード</param>
        /// <returns>カラー</returns>
        public static Color ToColor(this string colorCode)
        {
            string colorCodeAddedSharp = "";
            if (colorCode[0] != '#') colorCodeAddedSharp = "#";
            colorCodeAddedSharp += colorCode;

            Color result;
            if (ColorUtility.TryParseHtmlString(colorCodeAddedSharp, out result))
            {
                return result;
            }
            else
            {
                throw new InvalidCastException(colorCode + "からカラー構造体への変換に失敗しました。正しいカラーコードを指定してください。");
            }
        }

        /// <summary>
        /// カラーをHSVに変換する。
        /// </summary>
        /// <param name="color">カラー</param>
        /// <returns>Hsv</returns>
        public static Hsv ToHsv(this Color color)
        {
            return new Hsv(color);
        }
    }
}
