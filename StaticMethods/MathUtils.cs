using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HvcNeoria.Unity.Utils
{
    public static class MathUtils
    {
        /// <summary>
        /// 正規分布のランダム。
        /// </summary>
        /// <returns>正規分布のランダム</returns>
        public static float NormalDistribution()
        {
            float x = Random.value;
            float y = Random.value;
            float v = Mathf.Sqrt(-2f * Mathf.Log(x)) * Mathf.Cos(2f * Mathf.PI * y);
            return v;
        }
    }
}
