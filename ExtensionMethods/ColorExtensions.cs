using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class ColorExtensions
    {
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
