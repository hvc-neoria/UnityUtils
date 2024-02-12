using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HvcNeoria.Unity.Utils
{
    public static class SceneExtensions
    {
        /// <summary>
        /// アクティブシーンのビルドインデックスを取得します。
        /// </summary>
        /// <returns></returns>
        public static int ActiveSceneBuildIndex => SceneManager.GetActiveScene().buildIndex;

        /// <summary>
        /// 前回シングルモードでロードしたシーンのビルドインデックスを取得します。
        /// ※本クラスのメソッドを使用してシーンをロードした場合のみ、値が設定されます。
        /// </summary>
        public static int PreviouslyLoadedSceneIndex { get; private set; }

        /// <summary>
        /// シングルモードでシーンをロード中かどうかを取得します。
        /// </summary>
        static bool IsLoadingSceneInSingleMode { get; set; }

        /// <summary>
        /// 指定した待ち時間中にシーンを事前ロードし、待ち時間経過後にシーンをアクティブ化します。
        /// </summary>
        /// <remarks>
        /// 待ち時間が経過してもロードが完了しない場合は、ロード完了後にシーンがアクティブ化されます。
        /// </remarks>
        /// <param name="mono">MonoBehaviour。コルーチンの実行に必要。</param>
        /// <param name="waitForSeconds">待ち時間（秒）</param>
        /// <param name="singleScene">シングルモードでロードするシーン</param>
        /// <param name="additiveScenes">アディティブモードでロードするシーン</param>
        public static void ActivateSceneAfter(this MonoBehaviour mono, float waitForSeconds, Scene singleScene, params Scene[] additiveScenes)
        {
            mono.ActivateSceneAfter(
                waitForSeconds,
                () => SceneManager.LoadSceneAsync(singleScene.buildIndex, LoadSceneMode.Single),
                () => GetAdditiveSceneOperations(additiveScenes)
            );
        }

        /// <summary>
        /// ロードシーンモードがアディティブな複数のシーンをロードします。
        /// </summary>
        /// <param name="additiveScenes">アディティブシーン</param>
        /// <returns>AsyncOperation配列</returns>
        static AsyncOperation[] GetAdditiveSceneOperations(Scene[] additiveScenes)
        {
            var additiveSceneOperations = new AsyncOperation[additiveScenes.Length];
            for (int i = 0; i < additiveScenes.Length; i++)
            {
                additiveSceneOperations[i] = SceneManager.LoadSceneAsync(additiveScenes[i].buildIndex, LoadSceneMode.Additive);
            }
            return additiveSceneOperations;
        }

        /// <summary>
        /// 指定した待ち時間中にシーンを事前ロードし、待ち時間経過後にシーンをアクティブ化します。
        /// </summary>
        /// <remarks>
        /// 待ち時間が経過してもロードが完了しない場合は、ロード完了後にシーンがアクティブ化されます。
        /// </remarks>
        /// <param name="mono">MonoBehaviour。コルーチンの実行に必要。</param>
        /// <param name="waitForSeconds">待ち時間（秒）</param>
        /// <param name="loadSingleSceneAsyncMethod">非同期かつシングルでシーンをロードするメソッド</param>
        /// <param name="loadAdditiveSceneAsyncMethods">非同期でアディティブなシーンをロードするメソッド</param>
        static void ActivateSceneAfter(this MonoBehaviour mono, float waitForSeconds, Func<AsyncOperation> loadSingleSceneAsyncMethod, Func<AsyncOperation[]> loadAdditiveSceneAsyncMethods)
        {
            if (IsLoadingSceneInSingleMode)
            {
                Debug.LogWarning("シングルモードでのシーンロード中に、追加でシーンロードしません。\n追加シーンがアクティブ化しないまま残り続けてしまうためです。");
                return;
            }

            IsLoadingSceneInSingleMode = true;
            AsyncOperation singleSceneOperation = loadSingleSceneAsyncMethod();
            singleSceneOperation.allowSceneActivation = false;

            AsyncOperation[] additiveSceneOperations = loadAdditiveSceneAsyncMethods();

            mono.Delay(waitForSeconds, () =>
            {
                PreviouslyLoadedSceneIndex = ActiveSceneBuildIndex;
                singleSceneOperation.allowSceneActivation = true;
                IsLoadingSceneInSingleMode = false;

                foreach (var asyncOperation in additiveSceneOperations)
                {
                    asyncOperation.allowSceneActivation = true;
                }
            });
        }

        /// <summary>
        /// ビルドインデックス上で、このシーンの次のシーンを取得します。
        /// このシーンが最後のシーンの場合は、最初のシーンを取得します。
        /// </summary>
        /// <param name="scene">シーン</param>
        /// <returns>次のシーン</returns>
        public static Scene GetNextScene(this Scene scene)
        {

            int lastIndex = SceneManager.sceneCountInBuildSettings - 1;
            var index = scene.buildIndex >= lastIndex ? 0 : scene.buildIndex + 1;
            return SceneManager.GetSceneByBuildIndex(index);
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
