using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class TransformExtensions
    {
        public static Transform SetPosX(this Transform tr, float x)
        {
            tr.position = new Vector3(x, tr.position.y, tr.position.z);
            return tr;
        }

        public static Transform SetPosY(this Transform tr, float y)
        {
            tr.position = new Vector3(tr.position.x, y, tr.position.z);
            return tr;
        }

        public static Transform SetPosZ(this Transform tr, float z)
        {
            tr.position = new Vector3(tr.position.x, tr.position.y, z);
            return tr;
        }
    }
}
