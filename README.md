# 使い方

## 準備

1. ソースをダウンロード
2. Assetsフォルダ下に配置

## クラス / Classes

```csharp
// 1. クラスの使用先で下記ネームスペースを記述する
using HvcNeoria.Unity.Utils

public class Test : MonoBehaviour
{
    void Start()
    {
        // 2. new クラス名(引数)でインスタンスを生成する
        var hsv = new Hsv(0.5f, 0.5f, 0.5f);

        // 3. インスタンス.メソッド（またはプロパティ）と記述し、利用する
        Debug.Log(hsv.ToColor());
    }
}
```

## コンポーネント / Components

使用したいスクリプトコンポーネントをUnity上でゲームオブジェクトに追加する。

## エディタ拡張 / EditorExtensions

各クラスのコメントに記載された使い方に従う。

## 拡張メソッド / ExtensionMethods

1. メソッドの第一引数の型を確認する
`public static Vector3 ToXOZ(this Vector2 vector2) => new Vector3(vector2.x, 0, vector2.y);`

```csharp
using UnityEngine;
// 2. クラスの使用先で下記ネームスペースを記述する
using HvcNeoria.Unity.Utils

public class Test : MonoBehaviour
{
    void Start()
    {
        // 3. 1のインスタンス.メソッド（またはプロパティ）と記述し、利用する
        //    1がMonoBehaviourかつ使用先のクラスがMonoBehaviourを継承している場合、this.メソッド（またはプロパティ）と記述可能
        var vector3 = new Vector2(1f, 2f).ToXOZ();
        Debug.Log(vector3);
    }
}
```

## Staticメソッド / StaticMethods

```csharp
using UnityEngine;
// 1. クラスの使用先で下記ネームスペースを記述する
using HvcNeoria.Unity.Utils

public class Test : MonoBehaviour
{
    void Start()
    {
        // 2. クラス名.メソッド（またはプロパティ）と記述し、利用する
        Debug.Log(InputUtils.GetMousePositionIn3D());
    }
}
```
