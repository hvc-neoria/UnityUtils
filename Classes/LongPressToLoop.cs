using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;


namespace HvcNeoria.Unity.Utils
{
    /// <summary>
    /// 長押し入力で処理を繰り返し実行させるクラス。
    /// OnUpdate()をUpdate関数内で呼び出す必要がある。
    /// </summary>
    public class LongPressToLoop
    {
        readonly Action action;
        readonly Func<bool> buttonDown;
        readonly Func<bool> buttonUp;
        readonly WaitForSeconds waitTimeAtFirst;
        readonly WaitForSeconds waitTimeAtLoop;
        readonly MonoBehaviour mono;

        Coroutine coroutine;

        /// <summary>
        /// 長押し入力で処理を繰り返し実行させるクラス。
        /// 当クラス使用時は、OnUpdate()をUpdate関数内で呼び出してください。
        /// </summary>
        /// <param name="action">繰り返し実行する処理</param>
        /// <param name="buttonDown">ボタンを押し下げるメソッド</param>
        /// <param name="buttonUp">ボタンを押し上げるメソッド</param>
        /// <param name="waitTimeAfterFirstAction">1回目のアクション後の待ち時間</param>
        /// <param name="waitTimeAtLoop">ループ時の待ち時間</param>
        public LongPressToLoop(Action action, Func<bool> buttonDown, Func<bool> buttonUp, float waitTimeAfterFirstAction, float waitTimeAtLoop)
        {
            this.action = action;
            this.buttonDown = buttonDown;
            this.buttonUp = buttonUp;
            this.waitTimeAtFirst = new WaitForSeconds(waitTimeAfterFirstAction);
            this.waitTimeAtLoop = new WaitForSeconds(waitTimeAtLoop);
            mono = GameObject.FindObjectOfType<MonoBehaviour>();
        }

        /// <summary>
        /// Update関数内で呼び出す必要があるメソッド。
        /// </summary>
        public void OnUpdate()
        {
            if (buttonDown())
            {
                coroutine = mono.StartCoroutine(Coroutine());
            }

            if (buttonUp())
            {
                mono.StopCoroutine(coroutine);
            }
        }

        IEnumerator Coroutine()
        {
            action();
            yield return waitTimeAtFirst;

            while (true)
            {
                action();
                yield return waitTimeAtLoop;
            }
        }
    }
}
