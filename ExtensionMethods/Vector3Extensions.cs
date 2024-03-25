using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class Vector3Extensions
    {
        public static Vector2 ToXZ(this Vector3 vector3) => new Vector2(vector3.x, vector3.z);
    }
}
