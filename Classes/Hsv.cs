using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public struct Hsv
    {
        public readonly float h, s, v;
        const float min = 0;
        const float max = 1f;

        /// <summary>
        /// コンストラクター。
        /// いずれも0～1fの範囲で指定すること。
        /// </summary>
        /// <param name="h">色相</param>
        /// <param name="s">彩度</param>
        /// <param name="v">明度</param>
        public Hsv(float h, float s, float v)
        {
            if (h < min) throw new ArgumentOutOfRangeException($"{nameof(h)}が{min}未満です。{min}～{max}の範囲で指定してください。");
            if (h > max) throw new ArgumentOutOfRangeException($"{nameof(h)}が{max}を超えています。{min}～{max}の範囲で指定してください。");
            if (s < min) throw new ArgumentOutOfRangeException($"{nameof(s)}が{min}未満です。{min}～{max}の範囲で指定してください。");
            if (s > max) throw new ArgumentOutOfRangeException($"{nameof(s)}が{max}を超えています。{min}～{max}の範囲で指定してください。");
            if (v < min) throw new ArgumentOutOfRangeException($"{nameof(v)}が{min}未満です。{min}～{max}の範囲で指定してください。");
            if (v > max) throw new ArgumentOutOfRangeException($"{nameof(v)}が{max}を超えています。{min}～{max}の範囲で指定してください。");
            this.h = h;
            this.s = s;
            this.v = v;
        }

        public Hsv(Color color)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            this.h = h;
            this.s = s;
            this.v = v;
        }

        /// <summary>
        /// 明度を設定する。
        /// </summary>
        /// <param name="v"></param>
        /// <returns>新たなHsvインスタンス</returns>
        public Hsv SetValue(float v)
        {
            if (v < min) throw new ArgumentOutOfRangeException($"{nameof(v)}が{min}未満です。{min}～{max}の範囲で指定してください。");
            if (v > max) throw new ArgumentOutOfRangeException($"{nameof(v)}が{max}を超えています。{min}～{max}の範囲で指定してください。");
            return new Hsv(h, s, v);
        }

        public Color ToColor()
        {
            return Color.HSVToRGB(h, s, v);
        }
    }
}
