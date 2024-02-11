using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class Vector2IntExtensions
    {
        public static Vector3Int ToXOZ(this Vector2Int vector2Int) => new Vector3Int(vector2Int.x, 0, vector2Int.y);
    }
}
