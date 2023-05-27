using System;
using System.Collections;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    // 個人的に遅延処理はCoroutineかDOTweenが使いやすい。
    // 参考：https://12px.com/blog/2016/11/unity-delay/
    public static class CoroutineUtils
    {
        static MonoBehaviour mono;

        static CoroutineUtils()
        {
            mono = GameObject.FindObjectOfType<MonoBehaviour>();
        }

        /// <summary>
        /// 指定時間後にactionを実行する。
        /// </summary>
        /// <param name="waitForSeconds">待ち時間（秒）</param>
        /// <param name="action">アクション</param>
        /// <returns>コルーチン</returns>
        public static Coroutine Delay(WaitForSeconds waitForSeconds, Action action)
        {
            return mono.StartCoroutine(DelayCoroutine(waitForSeconds, action));
        }

        /// <summary>
        /// 指定時間後にactionを実行する。
        /// 呼び出す度にWaitForSecondsをインスタンス化する点に注意。
        /// </summary>
        /// <param name="waitForSeconds">待ち時間（秒）</param>
        /// <param name="action">アクション</param>
        /// <returns>コルーチン</returns>
        public static Coroutine Delay(float waitForSeconds, Action action)
        {
            return mono.StartCoroutine(DelayCoroutine(new WaitForSeconds(waitForSeconds), action));
        }

        /// <summary>
        /// 1フレーム後にactionを実行する。
        /// </summary>
        /// <param name="action">アクション</param>
        /// <returns>コルーチン</returns>
        public static Coroutine Delay1Frame(Action action)
        {
            return mono.StartCoroutine(Delay1FrameCoroutine(action));
        }

        /// <summary>
        /// 指定した時間間隔でactionを実行する。
        /// 最初のactionは本メソッド実行時。
        /// </summary>
        /// <param name="intervalTime">繰り返しの時間間隔</param>
        /// <param name="action">アクション</param>
        /// <param name="timeToExecuteLastAction">最後のアクションを実行するまでの時間</param>
        /// <returns>コルーチン</returns>
        public static Coroutine Loop(WaitForSeconds intervalTime, Action action, float timeToExecuteLastAction = Mathf.Infinity)
        {
            action();
            return LoopWhoseFirstActionIsAfterInterval(intervalTime, action, timeToExecuteLastAction);
        }

        /// <summary>
        /// 指定した時間間隔でactionを実行する。
        /// 最初のactionはintervalTime後に実行する。
        /// </summary>
        /// <param name="intervalTime">繰り返しの時間間隔</param>
        /// <param name="action">アクション</param>
        /// <param name="timeToExecuteLastAction">最後のアクションを実行するまでの時間</param>
        /// <returns>コルーチン</returns>
        public static Coroutine LoopWhoseFirstActionIsAfterInterval(WaitForSeconds intervalTime, Action action, float timeToExecuteLastAction = Mathf.Infinity)
        {
            return mono.StartCoroutine(LoopCoroutine(intervalTime, action, timeToExecuteLastAction));
        }

        /// <summary>
        /// 指定時間後にactionを実行する。
        /// </summary>
        /// <param name="waitForSeconds">待ち時間（秒）</param>
        /// <param name="action">アクション</param>
        /// <returns>IEnumerator</returns>
        static IEnumerator DelayCoroutine(WaitForSeconds waitForSeconds, Action action)
        {
            yield return waitForSeconds;
            action();
        }

        /// <summary>
        /// 1フレーム後にactionを実行する。
        /// </summary>
        /// <param name="action">アクション</param>
        /// <returns>IEnumerator</returns>
        static IEnumerator Delay1FrameCoroutine(Action action)
        {
            yield return null;
            action();
        }

        /// <summary>
        /// 指定した時間間隔でactionを実行する。
        /// 1度目のactionはintervalTime後に実行する。
        /// </summary>
        /// <param name="intervalTime">繰り返しの時間間隔</param>
        /// <param name="action">アクション</param>
        /// <param name="timeToExecuteLastAction">最後のアクションを実行するまでの時間</param>
        /// <returns>IEnumerator</returns>
        static IEnumerator LoopCoroutine(WaitForSeconds intervalTime, Action action, float timeToExecuteLastAction)
        {
            float startedTime = Time.time;

            while (Time.time < startedTime + timeToExecuteLastAction)
            {
                yield return intervalTime;
                action();
            }
        }
    }
}
