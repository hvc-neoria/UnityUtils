using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class InputUtils
    {
        /// <summary>
        /// マウスカーソルを3D空間に投影した時の座標を取得する。
        /// </summary>
        /// <returns>3次元空間に投影したマウスカーソルの座標</returns>
        public static Vector3 GetMousePositionIn3D()
        {
            return GetMouseHitInfoIn3D().point;
        }

        /// <summary>
        /// マウスカーソルを3D空間に投影した時のhitinfoを取得する。
        /// </summary>
        /// <returns>マウスカーソルを3D空間に投影した時のhitinfo</returns>
        public static RaycastHit GetMouseHitInfoIn3D()
        {
            return GetScreenPointHitInfoIn3D(Input.mousePosition);
        }

        /// <summary>
        /// スクリーン座標を3D空間に投影した時のhitinfoを取得する。
        /// </summary>
        /// <param name="screenPoint">スクリーン座標</param>
        /// <returns>スクリーン座標を3D空間に投影した時のhitinfo</returns>
        static RaycastHit GetScreenPointHitInfoIn3D(Vector3 screenPoint)
        {
            Ray mousePositionToRay = Camera.main.ScreenPointToRay(screenPoint);
            RaycastHit hitInfo = new RaycastHit();
            Physics.Raycast(mousePositionToRay, out hitInfo);
            return hitInfo;
        }
    }
}
