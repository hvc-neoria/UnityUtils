using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class ComponentExtensions
    {
        /// <summary>
        /// コンポーネント取得失敗時に例外を出力するGetComponent。
        /// </summary>
        /// <param name="target">コンポーネントの取得先</param>
        /// <typeparam name="TComponent">取得するコンポーネントの型</typeparam>
        /// <returns>コンポーネント</returns>
        public static TComponent RequireComponent<TComponent>(this Component target) where TComponent : Component
        {
            TComponent result = target.GetComponent<TComponent>();
            // ジェネリックの比較の際は、Unityによってオーバーロードされた==を使えず、
            // いわゆる「偽装null」をnullとして判定してくれない。
            // そのため、UnityEngine.Object型にキャストすることで、nullチェックを実現する。
            if ((UnityEngine.Object)result == null)
            {
                throw new MissingComponentForExtensionException($"{target}から{typeof(TComponent)}を取得できませんでした。");
            }
            return result;
        }

        /// <summary>
        /// コンポーネント取得失敗時に例外を出力するGetComponent。
        /// </summary>
        /// <param name="target">コンポーネントの取得先</param>
        /// <typeparam name="TComponent">取得するコンポーネントの型</typeparam>
        /// <returns>コンポーネント</returns>
        public static TComponent RequireComponent<TComponent>(this GameObject target) where TComponent : Component
        {
            TComponent result = target.GetComponent<TComponent>();
            // ジェネリックの比較の際は、Unityによってオーバーロードされた==を使えず、
            // いわゆる「偽装null」をnullとして判定してくれない。
            // そのため、UnityEngine.Object型にキャストすることで、nullチェックを実現する。
            if ((UnityEngine.Object)result == null)
            {
                throw new MissingComponentForExtensionException($"{target}から{typeof(TComponent)}を取得できませんでした。");
            }
            return result;
        }

        /// <summary>
        /// コンポーネント取得失敗時に例外を出力するGetComponentInChildren。
        /// </summary>
        /// <param name="target">コンポーネントの取得先</param>
        /// <typeparam name="TComponent">取得するコンポーネントの型</typeparam>
        /// <returns>コンポーネント</returns>
        public static TComponent RequireComponentInChildren<TComponent>(this Component target) where TComponent : Component
        {
            TComponent result = target.GetComponentInChildren<TComponent>();
            // ジェネリックの比較の際は、Unityによってオーバーロードされた==を使えず、
            // いわゆる「偽装null」をnullとして判定してくれない。
            // そのため、UnityEngine.Object型にキャストすることで、nullチェックを実現する。
            if ((UnityEngine.Object)result == null)
            {
                throw new MissingComponentForExtensionException($"{target}から{typeof(TComponent)}を取得できませんでした。");
            }
            return result;
        }

        /// <summary>
        /// コンポーネント取得失敗時に例外を出力するGetComponentInChildren。
        /// </summary>
        /// <param name="target">コンポーネントの取得先</param>
        /// <typeparam name="TComponent">取得するコンポーネントの型</typeparam>
        /// <returns>コンポーネント</returns>
        public static TComponent RequireComponentInChildren<TComponent>(this GameObject target) where TComponent : Component
        {
            TComponent result = target.GetComponentInChildren<TComponent>();
            // ジェネリックの比較の際は、Unityによってオーバーロードされた==を使えず、
            // いわゆる「偽装null」をnullとして判定してくれない。
            // そのため、UnityEngine.Object型にキャストすることで、nullチェックを実現する。
            if ((UnityEngine.Object)result == null)
            {
                throw new MissingComponentForExtensionException($"{target}から{typeof(TComponent)}を取得できませんでした。");
            }
            return result;
        }

        /// <summary>
        /// コンポーネント取得失敗時に例外を出力するGetComponentInParent。
        /// </summary>
        /// <param name="target">コンポーネントの取得先</param>
        /// <typeparam name="TComponent">取得するコンポーネントの型</typeparam>
        /// <returns>コンポーネント</returns>
        public static TComponent RequireComponentInParent<TComponent>(this Component target) where TComponent : Component
        {
            TComponent result = target.GetComponentInParent<TComponent>();
            // ジェネリックの比較の際は、Unityによってオーバーロードされた==を使えず、
            // いわゆる「偽装null」をnullとして判定してくれない。
            // そのため、UnityEngine.Object型にキャストすることで、nullチェックを実現する。
            if ((UnityEngine.Object)result == null)
            {
                throw new MissingComponentForExtensionException($"{target}から{typeof(TComponent)}を取得できませんでした。");
            }
            return result;
        }

        /// <summary>
        /// コンポーネント取得失敗時に例外を出力するGetComponentInParent。
        /// </summary>
        /// <param name="target">コンポーネントの取得先</param>
        /// <typeparam name="TComponent">取得するコンポーネントの型</typeparam>
        /// <returns>コンポーネント</returns>
        public static TComponent RequireComponentInParent<TComponent>(this GameObject target) where TComponent : Component
        {
            TComponent result = target.GetComponentInParent<TComponent>();
            // ジェネリックの比較の際は、Unityによってオーバーロードされた==を使えず、
            // いわゆる「偽装null」をnullとして判定してくれない。
            // そのため、UnityEngine.Object型にキャストすることで、nullチェックを実現する。
            if ((UnityEngine.Object)result == null)
            {
                throw new MissingComponentForExtensionException($"{target}から{typeof(TComponent)}を取得できませんでした。");
            }
            return result;
        }

        /// <summary>
        /// 拡張メソッド用の、コンポーネント取得失敗の例外。
        /// </summary>
        public class MissingComponentForExtensionException : Exception
        {
            const string DefaultMessage = "コンポーネントが見つかりませんでした。";

            public MissingComponentForExtensionException() : base(DefaultMessage)
            {
            }

            public MissingComponentForExtensionException(string message) : base(message)
            {
            }

            public MissingComponentForExtensionException(string message, Exception inner) : base(message, inner)
            {
            }
        }


        /// <summary>
        /// 自分自身を含めないGetComponentsInChildren。
        /// 参考：https://baba-s.hatenablog.com/entry/2014/06/05/220224
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <typeparam name="TComponent">取得するコンポーネントの型</typeparam>
        /// <returns>コンポーネントの配列</returns>
        public static TComponent[] GetComponentsInChildrenWithoutSelf<TComponent>(this GameObject self) where TComponent : Component
        {
            return self.GetComponentsInChildren<TComponent>().Where(v => self != v.gameObject).ToArray();
        }

        /// <summary>
        /// 自分自身を含めないGetComponentsInChildren。
        /// 参考：https://baba-s.hatenablog.com/entry/2014/06/05/220224
        /// </summary>
        /// <param name="self">自分自身</param>
        /// <typeparam name="TComponent">取得するコンポーネントの型</typeparam>
        /// <returns>コンポーネントの配列</returns>
        public static TComponent[] GetComponentsInChildrenWithoutSelf<TComponent>(this Component self) where TComponent : Component
        {
            return self.GetComponentsInChildren<TComponent>().Where(v => self.gameObject != v.gameObject).ToArray();
        }


        /// <summary>
        /// 指定されたインターフェイスを実装したコンポーネントを持つオブジェクトを検索します。
        /// </summary>
        /// <typeparam name="TInterface">インターフェースの型</typeparam>
        public static TInterface FindObjectOfInterface<TInterface>() where TInterface : class
        {
            foreach (var component in GameObject.FindObjectsOfType<Component>())
            {
                if (component is TInterface result)
                {
                    return result;
                }
            }
            return null;
        }
    }
}
