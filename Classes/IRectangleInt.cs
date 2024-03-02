using UnityEngine;

public interface IRectangleInt
{
    Vector2Int Position { get; }
    Vector2Int Size { get; }
    Vector2Int LocalPosition { get; }

    int Left { get; }
    int Right { get; }
    int Bottom { get; }
    int Top { get; }

    Vector2Int LeftBottom { get; }
    Vector2Int RightBottom { get; }
    Vector2Int LeftTop { get; }
    Vector2Int RightTop { get; }

    int LocalLeft { get; }
    int LocalRight { get; }
    int LocalBottom { get; }
    int LocalTop { get; }

    int Width { get; }
    int Height { get; }

    int Area { get; }
}
