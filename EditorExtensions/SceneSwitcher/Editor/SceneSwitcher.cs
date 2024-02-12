using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    /// <summary>
    /// Unityの上部メニューバーの Tools に Scene Switcher を追加します。
    /// </summary>
    /// <remarks>
    /// 使い方
    /// ・Unityの上部メニューバーの Tools > Scene Switcher をクリックする
    /// ・切り替えたいシーンのボタンをクリックする
    /// </remarks>
    public class SceneSwitcher : EditorWindow
    {
        /// <Summary>
        /// シーン切り替えウィンドウを表示します。
        /// </Summary>
        [MenuItem("Tools/Scene Switcher")]
        static void Open()
        {
            var window = GetWindow<SceneSwitcher>();
            window.titleContent = new GUIContent("Scene Switcher");
        }

        /// <summary>
        /// Scene In Build に登録されているシーンをボタンとして表示します。
        /// </summary>
        void OnGUI()
        {
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (GUILayout.Button(EditorBuildSettings.scenes[i].path.Replace("Assets/", "").Replace(".unity", "")))
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(EditorBuildSettings.scenes[i].path);
                }
            }
        }
    }
}
