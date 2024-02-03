namespace HvcNeoria.Unity.Utils
{
    public static class StringExtensions
    {
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
    }
}
