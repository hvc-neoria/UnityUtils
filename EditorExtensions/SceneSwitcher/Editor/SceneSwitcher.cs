using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSwitcher : EditorWindow
{
    /// <Summary>
    /// ウィンドウを表示します。
    /// </Summary>
    [MenuItem("Tools/Scene Switcher")]
    static void Open()
    {
        var window = GetWindow<SceneSwitcher>();
        window.titleContent = new GUIContent("Scene Switcher");
    }

    /// <summary>
    /// Scene In Build のシーンをボタンで表示します。
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
