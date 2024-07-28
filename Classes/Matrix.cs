using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using System.Linq;

namespace HvcNeoria.Unity.Utils
{
    /// <summary>
    /// 行列。
    /// </summary>
    /// <remarks>
    /// Math.NET Numericsを使うともっと便利な行列が使えるが、
    /// UnityのVector2Intと組み合わせて使うために、このクラスを作成した。
    /// </remarks>
    /// <typeparam name="T">任意の型</typeparam>
    public class Matrix<T>
    {
        public readonly T[,] Value;

        public int Length => XLength * YLength;
        public int XLength => Value.GetLength(0);
        public int YLength => Value.GetLength(1);

        public T this[int x, int y]
        {
            get
            {
                return Value[x, y];
            }
            set
            {
                Value[x, y] = value;
            }
        }

        public T this[Vector2Int index]
        {
            get
            {
                return Value[index.x, index.y];
            }
            set
            {
                Value[index.x, index.y] = value;
            }
        }

        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="value"></param>
        public Matrix(T[,] value)
        {
            Value = value;
        }

        /// <summary>
        /// 部分行列を取得する。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public Matrix<T> SubMatrix(Vector2Int index, Vector2Int size)
        {
            var xLength = size.x + Mathf.Min(index.x, 0);
            var yLength = size.y + Mathf.Min(index.y, 0);
            T[,] newArray = new T[xLength, yLength];

            var xOrigin = Mathf.Max(index.x, 0);
            var yOrigin = Mathf.Max(index.y, 0);

            for (int x = 0; x < xLength; x++)
            {
                for (int y = 0; y < yLength; y++)
                {
                    newArray[x, y] = Value[xOrigin + x, yOrigin + y];
                }
            }
            return new Matrix<T>(newArray);
        }

        /// <summary>
        /// 置き換える。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rectangleInt"></param>
        /// <returns></returns>
        public Matrix<T> Replace(T value, RectangleInt rectangleInt)
        {
            T[,] newArray = new T[XLength, YLength];
            for (int x = 0; x < XLength; x++)
            {
                for (int y = 0; y < YLength; y++)
                {
                    if (x >= rectangleInt.LocalLeft && x <= rectangleInt.LocalRight && y >= rectangleInt.LocalBottom && y <= rectangleInt.LocalTop)
                    {
                        newArray[x, y] = value;
                    }
                    else
                    {
                        newArray[x, y] = Value[x, y];
                    }
                }
            }
            return new Matrix<T>(newArray);
        }

        /// <summary>
        /// IEnumerableを取得する。
        /// </summary>
        /// <returns>IEnumerable<T></returns>
        public IEnumerable<T> GetIEnumerable()
        {
            for (int x = 0; x < XLength; x++)
            {
                for (int y = 0; y < YLength; y++)
                {
                    yield return Value[x, y];
                }
            }
        }

        /// <summary>
        /// 引数と同じ値を持つ要素のインデックスを取得する。
        /// </summary>
        /// <param name="value">値</param>
        /// <returns>Vector2Int</returns>
        public Vector2Int IndexOf(T value)
        {
            for (int x = 0; x < XLength; x++)
            {
                for (int y = 0; y < YLength; y++)
                {
                    if (value != null && Value[x, y] == null) continue;
                    if (Value[x, y].Equals(value))
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
            throw new ArgumentException("Not found.");
        }
    }
}
