using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
            StringReader reader = new StringReader(csvFile.text);

            List<string[]> csvDatas = new List<string[]>();
            while (reader.Peek() != -1) // reader.Peekが-1になるまで
            {
                string line = reader.ReadLine(); // 一行ずつ読み込み
                csvDatas.Add(line.Split(',')); // カンマ（,）区切りでリストに追加
            }
            string[][] result = csvDatas.ToArray();
            return result;
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
            string colorCodeAddedSharp = colorCode;
            if (colorCodeAddedSharp[0] != '#') colorCodeAddedSharp = '#' + colorCodeAddedSharp;

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
