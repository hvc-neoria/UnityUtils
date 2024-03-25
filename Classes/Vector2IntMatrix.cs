using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public class Vector2IntMatrix : Matrix<Vector2Int>
    {
        public Vector2IntMatrix(Vector2Int[,] value) : base(value)
        {
        }

        public static Vector2IntMatrix operator +(Vector2IntMatrix a, Vector2Int b)
        {
            Vector2Int[,] newArray = new Vector2Int[a.XLength, a.YLength];
            for (int x = 0; x < a.XLength; x++)
            {
                for (int y = 0; y < a.YLength; y++)
                {
                    newArray[x, y] = a[x, y] + b;
                }
            }
            return new Vector2IntMatrix(newArray);
        }
    }
}
