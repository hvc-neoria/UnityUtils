using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    /// <summary>
    /// RectTransform 型の拡張メソッドを管理するクラス。
    /// https://baba-s.hatenablog.com/entry/2017/09/21/100000
    /// </summary>
    public static class RectTransformExtensions
    {
        /// <summary>
        /// 幅を返します。
        /// </summary>
        public static float GetWidth(this RectTransform self)
        {
            return self.sizeDelta.x;
        }

        /// <summary>
        /// 高さを返します。
        /// </summary>
        public static float GetHeight(this RectTransform self)
        {
            return self.sizeDelta.y;
        }

        /// <summary>
        /// 幅を設定します。
        /// </summary>
        public static void SetWidth(this RectTransform self, float width)
        {
            var size = self.sizeDelta;
            size.x = width;
            self.sizeDelta = size;
        }

        /// <summary>
        /// 高さを設定します。
        /// </summary>
        public static void SetHeight(this RectTransform self, float height)
        {
            var size = self.sizeDelta;
            size.y = height;
            self.sizeDelta = size;
        }

        /// <summary>
        /// サイズを設定します。
        /// </summary>
        public static void SetSize(this RectTransform self, float width, float height)
        {
            self.sizeDelta = new Vector2(width, height);
        }
    }
}
