using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace HvcNeoria.Unity.Utils
{
    public static class SceneUtils
    {
        public static int ActiveSceneBuildIndex => SceneManager.GetActiveScene().buildIndex;

        /// <summary>
        /// 前回ロードしたシーンのビルドインデックスを取得します。
        /// 本クラスのメソッドを使用してシーンをロードした場合のみ、値が設定されます。
        /// Additiveモードでロードしたシーンは対象外です。
        /// </summary>
        public static int PreviouslyLoadedSceneIndex { get; private set; }

        static string SceneLoadingInSingle;

        /// <summary>
        /// 指定した待ち時間中にシーンを事前ロードし、待ち時間経過後にシーンをアクティブ化します。
        /// 待ち時間が経過してもロードが完了しない場合は、ロード完了後にシーンがアクティブ化されます。
        /// </summary>
        /// <param name="mono">MonoBehaviour。コルーチンの実行に必要。</param>
        /// <param name="waitForSeconds">待ち時間（秒）</param>
        /// <param name="sceneBuildIndex">シーンのビルドインデックス</param>
        /// <param name="mode">シーンの読み込みモード</param>
        public static void ActivateSceneAfter(this MonoBehaviour mono, WaitForSeconds waitForSeconds, int sceneBuildIndex, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (SceneLoadingInSingle != null && mode == LoadSceneMode.Single)
            {
                Debug.LogWarning("シングルモードでシーンロード中に、追加でシングルモードでシーンロードしません。\n一方のシーンがアクティブ化しないまま残り続けるためです。");
                return;
            }

            if (mode == LoadSceneMode.Single) SceneLoadingInSingle = SceneManager.GetSceneByBuildIndex(sceneBuildIndex).name;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex, mode);
            asyncOperation.allowSceneActivation = false;

            mono.Delay(waitForSeconds, () =>
            {
                SceneLoadingInSingle = null;
                if (mode == LoadSceneMode.Single) PreviouslyLoadedSceneIndex = ActiveSceneBuildIndex;
                asyncOperation.allowSceneActivation = true;
            });
        }

        /// <summary>
        /// 指定した待ち時間中にシーンを事前ロードし、待ち時間経過後にシーンをアクティブ化します。
        /// 待ち時間が経過してもロードが完了しない場合は、ロード完了後にシーンがアクティブ化されます。
        /// </summary>
        /// <param name="mono">MonoBehaviour。コルーチンの実行に必要。</param>
        /// <param name="waitForSeconds">待ち時間（秒）</param>
        /// <param name="sceneName">シーン名</param>
        /// <param name="mode">シーンの読み込みモード</param>
        public static void ActivateSceneAfter(this MonoBehaviour mono, WaitForSeconds waitForSeconds, string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            if (SceneLoadingInSingle != null && mode == LoadSceneMode.Single)
            {
                Debug.LogWarning("シングルモードでシーンロード中に、追加でシングルモードでシーンロードしません。\n一方のシーンがアクティブ化しないまま残り続けるためです。");
                return;
            }

            if (mode == LoadSceneMode.Single) SceneLoadingInSingle = sceneName;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);
            asyncOperation.allowSceneActivation = false;

            mono.Delay(waitForSeconds, () =>
            {
                SceneLoadingInSingle = null;
                if (mode == LoadSceneMode.Single) PreviouslyLoadedSceneIndex = ActiveSceneBuildIndex;
                asyncOperation.allowSceneActivation = true;
            });
        }

        /// <summary>
        /// アクティブシーンの次のシーンのビルドインデックスを取得します。
        /// 最後のシーンで実行すると、0を返します。
        /// </summary>
        public static int GetNextBuildIndex()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int lastIndex = SceneManager.sceneCountInBuildSettings - 1;
            int index = currentIndex >= lastIndex ? 0 : currentIndex + 1;
            return index;
        }

        /// <summary>
        /// 次回のシーンロード完了時にアクションを実行します。
        /// シーン間のデータ共有に使用できます。
        /// </summary>
        /// <param name="action">実行するアクション</param>
        public static void OnSceneLoadedForNextTime(Action action)
        {
            SceneManager.sceneLoaded += DoOnce;

            void DoOnce(Scene scene, LoadSceneMode mode)
            {
                SceneManager.sceneLoaded -= DoOnce;
                action();
            }
        }
    }
}
