using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class Vector2Extensions
    {
        public static Vector3 ToXOZ(this Vector2 vector2) => new Vector3(vector2.x, 0, vector2.y);

        /// <summary>
        /// ゲームコントローラーの浅い斜め入力に対応したノーマライズ。
        /// XとYに0.5fを渡すと、XとYに約0.35fを返す。
        /// </summary>
        /// <param name="vector2">Vector2</param>
        /// <returns>Vector2</returns>
        public static Vector2 NormalizeForShallowInput(this Vector2 vector2)
        {
            float xLength = Mathf.Abs(vector2.x);
            float yLength = Mathf.Abs(vector2.y);
            float scale = Mathf.Max(xLength, yLength);
            return vector2.normalized * scale;
        }

        /// <summary>
        /// 角度を球座標に変換する。
        /// 緯度・経度のように角度を与える。
        /// 特徴として、Transform.RotateAroundでは困難な角度制限が容易。
        /// x=0, y=0, radiusが1のとき、x=0, y=0, z=-1
        /// x=0, y=90, radiusが1のとき、x=0, y=1, z=0
        /// x=90, y=90, radiusが1のとき、x=0.71, y=0.71, z=0
        /// </summary>
        /// <param name="angle">角度</param>
        /// <param name="radius">半径</param>
        /// <returns>球座標</returns>
        public static Vector3 ToSphericalCoordinate(this Vector2 angle, float radius)
        {
            // 緯度・経度のように入力できるよう、値を加工する。
            float x = -angle.x - 180f;
            float y = -angle.y + 90f;

            Vector3 result = ToSphericalCoordinateOriginally(new Vector2(x, y), radius);
            return result;
        }

        /// <summary>
        /// 角度を球座標に変換する。
        /// x=0, y=0, radiusが1のとき、x=0, y=1, z=0
        /// x=0, y=90, radiusが1のとき、x=0, y=0, z=1
        /// x=90, y=90, radiusが1のとき、x=1, y=0, z=0
        /// </summary>
        /// <param name="radius">半径</param>
        /// <returns>球座標</returns>
        public static Vector3 ToSphericalCoordinateOriginally(this Vector2 angle, float radius)
        {
            float xRad = angle.x * Mathf.Deg2Rad;
            float yRad = angle.y * Mathf.Deg2Rad;
            Vector3 result = new Vector3(
                radius * Mathf.Sin(yRad) * Mathf.Sin(xRad),
                radius * Mathf.Cos(yRad),
                radius * Mathf.Sin(yRad) * Mathf.Cos(xRad)
            );
            return result;
        }
    }
}
