using System.Linq;
using UnityEditor;

namespace HvcNeoria.Unity.Utils
{
    public static class UnityObjectExtensions
    {
        /// <summary>
        /// 指定したUnityEngine.Objectのサブアセットを取得する。
        /// </summary>
        /// <param name="obj">UnityEngineのObject</param>
        /// <returns>サブアセット</returns>
        public static UnityEngine.Object[] GetSubAssets(this UnityEngine.Object obj)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            var assets = AssetDatabase.LoadAllAssetsAtPath(path);
            return assets.Where(x => AssetDatabase.IsSubAsset(x)).ToArray();
        }
    }
}
