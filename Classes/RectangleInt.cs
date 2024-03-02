using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public struct RectangleInt : IRectangleInt
    {
        /// <summary>
        /// 左下隅の位置。
        /// </summary>
        public Vector2Int Position { get; }
        public Vector2Int Size { get; }
        public Vector2Int LocalPosition { get; }

        public int Left => Position.x;
        public int Right => Position.x + Size.x - 1;
        public int Bottom => Position.y;
        public int Top => Position.y + Size.y - 1;

        public Vector2Int LeftBottom => new Vector2Int(Left, Bottom);
        public Vector2Int RightBottom => new Vector2Int(Right, Bottom);
        public Vector2Int LeftTop => new Vector2Int(Left, Top);
        public Vector2Int RightTop => new Vector2Int(Right, Top);

        public int LocalLeft => LocalPosition.x;
        public int LocalRight => LocalPosition.x + Size.x - 1;
        public int LocalBottom => LocalPosition.y;
        public int LocalTop => LocalPosition.y + Size.y - 1;

        public int Width => Size.x;
        public int Height => Size.y;

        public int Area => Size.x * Size.y;

        /// <summary>
        /// コンストラクター。
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="size">Size</param>
        /// <param name="parentPosition">親のPosition。LocalPositionに使用。</param>
        public RectangleInt(Vector2Int position, Vector2Int size, Vector2Int parentPosition = default(Vector2Int))
        {
            Position = position;
            Size = size;
            LocalPosition = position - parentPosition;
        }
    }
}
