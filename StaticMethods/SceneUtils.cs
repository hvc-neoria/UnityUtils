using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        static bool IsLoadingSceneInSingleMode { get; set; }

        /// <summary>
        /// 指定した待ち時間中にシーンを事前ロードし、待ち時間経過後にシーンをアクティブ化します。
        /// </summary>
        /// <remarks>
        /// 待ち時間が経過してもロードが完了しない場合は、ロード完了後にシーンがアクティブ化されます。
        /// </remarks>
        /// <param name="mono">MonoBehaviour。コルーチンの実行に必要。</param>
        /// <param name="waitForSeconds">待ち時間（秒）</param>
        /// <param name="singleSceneName">シングルモードでロードするシーン名</param>
        /// <param name="additiveSceneNames">アディティブモードでロードするシーン名</param>
        public static void ActivateSceneAfter(this MonoBehaviour mono, float waitForSeconds, string singleSceneName, params string[] additiveSceneNames)
        {
            int singleSceneBuildIndex = SceneManager.GetSceneByName(singleSceneName).buildIndex;

            int[] additiveSceneBuildIndexes = new int[additiveSceneNames.Length];
            for (int i = 0; i < additiveSceneNames.Length; i++)
            {
                additiveSceneBuildIndexes[i] = SceneManager.GetSceneByName(additiveSceneNames[i]).buildIndex;
            }

            mono.ActivateSceneAfter(waitForSeconds, singleSceneBuildIndex, additiveSceneBuildIndexes);
        }

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
        public static void ActivateSceneAfter(this MonoBehaviour mono, float waitForSeconds, int singleSceneBuildIndex, params int[] additiveSceneBuildIndexes)
        {
            if (IsLoadingSceneInSingleMode)
            {
                Debug.LogWarning("シングルモードでのシーンロード中に、追加でシーンロードしません。\n追加シーンがアクティブ化しないまま残り続けてしまうためです。");
                return;
            }

            AsyncOperation singleSceneOperation = SceneManager.LoadSceneAsync(singleSceneBuildIndex, LoadSceneMode.Single);
            singleSceneOperation.allowSceneActivation = false;
            IsLoadingSceneInSingleMode = true;

            List<AsyncOperation> additiveSceneOperations = new List<AsyncOperation>(additiveSceneBuildIndexes.Length);
            foreach (int buildIndex in additiveSceneBuildIndexes)
            {
                AsyncOperation additiveSceneOperation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
                additiveSceneOperation.allowSceneActivation = false;
                additiveSceneOperations.Add(additiveSceneOperation);
            }

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
