using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HvcNeoria.Unity.Utils
{
    /// <summary>
    /// メソッドの引数をSceneにしたかったが、
    /// ビルドインデックスまたはシーン名からロードしていないSceneを取得できないため、
    /// ビルドインデックスに統一した。
    /// </summary>
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
        /// <param name="singleSceneBuildIndex">シングルモードでロードするシーンのビルドインデックス</param>
        /// <param name="additiveSceneBuildIndexes">アディティブモードでロードするシーンのビルドインデックス</param>
        public static void ActivateSceneAfter(this MonoBehaviour mono, float waitForSeconds, int singleSceneIndex, params int[] additiveSceneIndexes)
        {
            mono.ActivateSceneAfter(
                waitForSeconds,
                () => SceneManager.LoadSceneAsync(singleSceneIndex, LoadSceneMode.Single),
                () => GetAdditiveSceneOperations(additiveSceneIndexes)
            );
        }

        /// <summary>
        /// ロードシーンモードがアディティブな複数のシーンをロードします。
        /// </summary>
        /// <param name="additiveSceneIndexes">ビルドインデックス</param>
        /// <returns></returns>
        static AsyncOperation[] GetAdditiveSceneOperations(int[] additiveSceneIndexes)
        {
            AsyncOperation[] additiveSceneOperations = new AsyncOperation[additiveSceneIndexes.Length];
            for (int i = 0; i < additiveSceneIndexes.Length; i++)
            {
                additiveSceneOperations[i] = SceneManager.LoadSceneAsync(additiveSceneIndexes[i], LoadSceneMode.Additive);
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
        /// アクティブシーンの次のシーンのビルドインデックスを取得します。
        /// 最後のシーンで実行すると、0を返します。
        /// </summary>
        /// <param name="buildIndex">ビルドインデックス</param>
        /// <returns>次のビルドインデックス</returns>
        public static int GetNextBuildIndex(int buildIndex)
        {
            int lastIndex = SceneManager.sceneCountInBuildSettings - 1;
            return buildIndex >= lastIndex ? 0 : buildIndex + 1;
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

        /// <summary>
        /// 同名のシーンが存在する場合、ビルドインデックスが若い方を返します。
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        public static int ToBuildIndex(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                // シーン名を抽出
                var fileName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (sceneName == fileName)
                {
                    return i;
                }
            }
            throw new ArgumentException($"シーン名 {sceneName} が見つかりませんでした。");
        }
    }
}
