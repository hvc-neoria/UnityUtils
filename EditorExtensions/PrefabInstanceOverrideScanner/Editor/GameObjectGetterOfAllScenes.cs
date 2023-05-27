//  GameObjectGetterOfAllScenes.cs
//  http://kan-kikuchi.hatenablog.com/entry/GameObjectGetterOfAllScenes
//
//  Created by kan.kikuchi on 2017.09.28.

using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace HvcNeoria.Unity.Utils
{
    /// <summary>
    /// 全シーンのGameObjectを取得するクラス
    /// </summary>
    public class GameObjectGetterOfAllScenes
    {

        //シーンの総数
        private static int _sceneNum = 0;

        //全シーンのパス
        private static List<string> _scenePathList = new List<string>();

        //シーン読み込み後に、GameObjectを渡して実行するアクション
        private static Action<string, List<GameObject>, Action> _action;

        //=================================================================================
        //取得
        //=================================================================================

        /// <summary>
        /// 指定されたパスにあるディレクトリに入っている全シーンを順に読み込み、シーンないの全GameObjectを取得し、渡されたActionを実行する
        /// </summary>
        public static void Get(string directoryPath, Action<string, List<GameObject>, Action> action)
        {
            _action = action;

            //指定ディレクトリ内の全ファイルのパスを取得(meta含む)
            string[] filePathArray = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

            //取得したファイルの中からシーンアセットだけリストに追加する
            _scenePathList.Clear();
            foreach (string filePath in filePathArray)
            {
                if (AssetDatabase.LoadAssetAtPath<SceneAsset>(filePath) != null)
                {
                    _scenePathList.Add(filePath);
                }
            }

            //シーンの総数を設定し、シーンの読み込み開始
            _sceneNum = _scenePathList.Count;
            Load();
        }

        //シーン読み込み
        private static void Load()
        {
            //全シーン見終わったら終了、プログレスバーも消す
            if (_scenePathList.Count == 0)
            {
                EditorUtility.ClearProgressBar();

                // 追記者：HVC
                Debug.Log("オーバーライドがあるプレハブインスタンスの走査を終了します。");

                return;
            }

            //シーンのパスと名前を取得
            string scenePath = _scenePathList[0];
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);
            _scenePathList.Remove(scenePath);

            //プログレスバー表示
            EditorUtility.DisplayProgressBar(
                sceneName + "読み込み中",
                (_sceneNum - _scenePathList.Count).ToString() + " / " + _sceneNum.ToString(),
                (_sceneNum - _scenePathList.Count) / (float)_sceneNum
            );

            //シーン読み込み
            EditorSceneManager.OpenScene(scenePath);

            //全GameObject取得(非アクティブのものも)
            List<GameObject> gameObjectList = new List<GameObject>();
            foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
                //シーン上に存在するGameObjectだけ取得
                if (AssetDatabase.GetAssetOrScenePath(gameObject).Contains(".unity"))
                {
                    gameObjectList.Add(gameObject);
                }
            }

            //シーン名と取得したGameObjectを渡してアクションを実行
            _action(sceneName, gameObjectList, Load);
        }

    }
}
