using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class TextAssetExtensions
    {
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
        /// CSVで1行目の値がキーとなる。
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
    }
}
