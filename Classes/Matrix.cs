using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

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
        T[,] Value { get; set; }

        public int XLength => Value.GetLength(0);
        public int YLength => Value.GetLength(1);

        public T this[Vector2Int position]
        {
            get
            {
                return Value[position.x, position.y];
            }
            set
            {
                Value[position.x, position.y] = value;
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
            T[,] newArray = new T[size.x, size.y];
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    newArray[x, y] = Value[index.x + x, index.y + y];
                }
            }
            return new Matrix<T>(newArray);
        }
    }
}
