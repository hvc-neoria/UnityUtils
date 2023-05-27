using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace HvcNeoria.Unity.Utils
{
    /// <summary>
    /// Input.GetAxisRawを押し下げ・押し上げ入力に変換するクラス。
    /// </summary>
    public class AxisToButtons
    {
        public bool NegativeButtonUp { get; private set; }
        public bool PositiveButtonUp { get; private set; }
        public bool NegativeButtonDown { get; private set; }
        public bool PositiveButtonDown { get; private set; }
        string axisName;
        float axisRawBefore1Frame;

        /// <summary>
        /// Axisを押し下げ、押し上げ入力に変換するクラスのコンストラクター
        /// </summary>
        /// <param name="axisName">axisの名前</param>
        public AxisToButtons(string axisName)
        {
            this.axisName = axisName;
        }

        /// <summary>
        /// Update関数内で呼び出す必要があるメソッド
        /// </summary>
        public void OnUpdate()
        {
            float currentAxisRaw = Input.GetAxisRaw(axisName);
            PositiveButtonDown = axisRawBefore1Frame != 1f && currentAxisRaw == 1f;
            PositiveButtonUp = axisRawBefore1Frame == 1f && currentAxisRaw != 1f;
            NegativeButtonDown = axisRawBefore1Frame != -1f && currentAxisRaw == -1f;
            NegativeButtonUp = axisRawBefore1Frame == -1f && currentAxisRaw != -1f;

            axisRawBefore1Frame = currentAxisRaw;
        }
    }
}
