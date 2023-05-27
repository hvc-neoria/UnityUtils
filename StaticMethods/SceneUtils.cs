using UnityEngine;
using UnityEngine.SceneManagement;

namespace HvcNeoria.Unity.Utils
{
    public static class SceneUtils
    {
        public static int ActiveSceneBuildIndex => SceneManager.GetActiveScene().buildIndex;

        /// <summary>
        /// 指定した待ち時間中にシーンを事前ロードし、待ち時間経過後にシーンをアクティブ化します。
        /// 待ち時間が経過してもロードが完了しない場合は、ロード完了後にシーンがアクティブ化されます。
        /// </summary>
        /// <param name="waitForSeconds">待ち時間（秒）</param>
        /// <param name="sceneBuildIndex">シーンのビルドインデックス</param>
        /// <param name="mode">シーンの読み込みモード</param>
        public static void ActivateSceneAfter(WaitForSeconds waitForSeconds, int sceneBuildIndex, LoadSceneMode mode = LoadSceneMode.Single)
        {
            AsyncOperation a = SceneManager.LoadSceneAsync(sceneBuildIndex, mode);
            a.allowSceneActivation = false;
            CoroutineUtils.Delay(waitForSeconds, () => a.allowSceneActivation = true);
        }

        /// <summary>
        /// 指定した待ち時間中にシーンを事前ロードし、待ち時間経過後にシーンをアクティブ化します。
        /// 待ち時間が経過してもロードが完了しない場合は、ロード完了後にシーンがアクティブ化されます。
        /// </summary>
        /// <param name="waitForSeconds">待ち時間（秒）</param>
        /// <param name="sceneName">シーン名</param>
        /// <param name="mode">シーンの読み込みモード</param>
        public static void ActivateSceneAfter(WaitForSeconds waitForSeconds, string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            AsyncOperation a = SceneManager.LoadSceneAsync(sceneName, mode);
            a.allowSceneActivation = false;
            CoroutineUtils.Delay(waitForSeconds, () => a.allowSceneActivation = true);
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
    }
}
