using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HvcNeoria.Unity.Utils
{
    public class PrefabInstanceOverrideScanner : MonoBehaviour
    {
        [MenuItem("Tools/Prefab Instance Override Scan")]
        public static void PrefabInstanceOverrideScan()
        {
            Debug.Log("オーバーライドがあるプレハブインスタンスの走査を開始します。");

            //Scenesディレクトリにある全シーンの全GameObjectを取得し、PrefabInstanceOverrideScanを実行する
            GameObjectGetterOfAllScenes.Get("Assets", PrefabInstanceOverrideScan);
        }

        //全GameObjectから、オーバーライドがあるプレハブインスタンスが見つかったらログで表示
        //sceneNameがシーンの名前、gameObjectListがそのシーンにあるGameObject(非アクティブのものも含む)
        //callbackが次のシーン読み込み、または処理を終了するためのコールバック
        private static void PrefabInstanceOverrideScan(string sceneName, List<GameObject> gameObjectList, Action callback)
        {
            foreach (GameObject g in gameObjectList)
            {
                // 最上位の親のプレハブインスタンスであることを判定する
                // オーバーライドを取得するメソッドが、プレハブインスタンスの先祖や子孫も走査するため
                if (PrefabUtility.IsOutermostPrefabInstanceRoot(g))
                {
                    // インスタンスのオーバーライドは以下の4種類がある
                    // 参考：https://docs.unity3d.com/ja/2018.4/Manual/PrefabInstanceOverrides.html

                    foreach (var item in PrefabUtility.GetObjectOverrides(g, false))
                    {
                        Debug.Log($"{sceneName}：{g.name}の{item.instanceObject}からプロパティの値のオーバーライドを検出しました。");
                    }

                    foreach (var item in PrefabUtility.GetAddedComponents(g))
                    {
                        Debug.Log($"{sceneName}：{g.name}の{item.instanceComponent}からコンポーネント追加のオーバーライドを検出しました。");
                    }

                    foreach (var item in PrefabUtility.GetRemovedComponents(g))
                    {
                        Debug.Log($"{sceneName}：{g.name}の{item.containingInstanceGameObject}からコンポーネント削除のオーバーライドを検出しました。");
                    }

                    foreach (var item in PrefabUtility.GetAddedGameObjects(g))
                    {
                        Debug.Log($"{sceneName}：{g.name}の{item.instanceGameObject}から子ゲームオブジェクト追加のオーバーライドを検出しました。");
                    }
                }
            }

            //処理が終わったらコールバック実行(次のシーン読み込みへ)
            callback();
        }
    }
}
