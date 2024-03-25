using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Text;
using System.Runtime.CompilerServices;

namespace HvcNeoria.Unity.Utils
{
    public class SceneNamesGenerator
    {
        const string FileName = "SceneNames.cs";

        /// <summary>
        /// シーン名を列挙型で管理するクラスを生成する。
        /// </summary>
        /// <remarks>
        /// 使い方
        /// ・コンパイル時に自動で実行される。
        /// </remarks>
        [DidReloadScripts]
        static void Generate()
        {
            var builder = new StringBuilder();
            builder.AppendLine("public enum SceneNames");
            builder.AppendLine("{");

            foreach (var scene in EditorBuildSettings.scenes)
            {
                var sceneName = Path.GetFileNameWithoutExtension(scene.path);
                builder.AppendLine($"    {sceneName},");
            }

            builder.AppendLine("}");

            var thisFolderPath = Path.GetDirectoryName(GetThisFilePath());
            File.WriteAllText(thisFolderPath + "/" + FileName, builder.ToString());
        }

        /// <summary>
        /// このファイルのパスを取得する。
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns></returns>
        static string GetThisFilePath([CallerFilePath] string sourceFilePath = "")
        {
            return sourceFilePath;
        }
    }
}
