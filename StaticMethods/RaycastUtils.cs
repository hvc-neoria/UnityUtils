using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class RaycastUtils
    {
        /// <summary>
        /// レイキャストのワンライナー。
        /// 衝突していない時は、RaycastHit.colliderがnull。
        /// </summary>
        /// <param name="origin">起点</param>
        /// <param name="direction">向き</param>
        /// <param name="maxDistance">最大距離</param>
        /// <param name="layerMask">レイヤーマスク</param>
        /// <param name="interaction">インターアクション</param>
        /// <returns>RaycastHit</returns>
        public static RaycastHit Raycast(Vector3 origin, Vector3 direction, float maxDistance, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction interaction = QueryTriggerInteraction.UseGlobal)
        {
            RaycastHit hitInfo;
            Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask, interaction);
            return hitInfo;
        }

        /// <summary>
        /// レイを描画する。
        /// </summary>
        /// <param name="origin">起点</param>
        /// <param name="hitInfo">HitInfo。衝突したコライダーがnullの時はmaxDistanceまで描画する。</param>
        /// <param name="direction">向き</param>
        /// <param name="maxDistance">最大距離</param>
        /// <param name="duration">描画時間</param>
        public static void DrawRay(Vector3 origin, RaycastHit hitInfo, Vector3 direction, float maxDistance, float duration = 1f)
        {
            if (hitInfo.collider == null)
            {
                Debug.DrawRay(origin, direction * maxDistance, Color.red, duration);
            }
            else
            {
                Debug.DrawRay(origin, hitInfo.point - origin, Color.green, duration);
            }
        }
    }
}
