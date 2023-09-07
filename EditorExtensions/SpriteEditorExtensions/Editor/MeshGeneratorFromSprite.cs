using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class MeshGeneratorFromSprite
    {
        /// <summary>
        /// コンテキストメニューで最下部に表示するための優先度の値。
        /// </summary>
        const int PriorityForBottomInContextMenu = 200000;

        /// <summary>
        /// 選択されたSpriteからMeshを生成する。
        /// </summary>
        /// <remarks>
        /// プロジェクトビューでSpriteを右クリックし、本処理を選択すると実行する。
        /// </remarks>
        [MenuItem("Assets/GenerateMeshesFromSprites", priority = PriorityForBottomInContextMenu)]
        static void GenerateMeshesFromSprites()
        {
            var sprites = Selection.objects.OfType<Texture2D>().Select(x => (Sprite)x.GetSubAssets()[0]);

            foreach (var sprite in sprites)
            {
                string meshesFolderPath = sprite.GenerateMeshesFolderPath();
                if (!Directory.Exists(meshesFolderPath)) Directory.CreateDirectory(meshesFolderPath);

                string meshFilePath = GenerateMeshFilePath(meshesFolderPath, sprite.name);
                var mesh = sprite.ToMesh();
                AssetDatabase.CreateAsset(mesh, meshFilePath);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        /// <summary>
        /// 選択されたオブジェクトが全てSpriteである場合、
        /// コンテキストメニューで<see cref="GenerateMeshesFromSprites"/>を使用可能にする。
        /// </summary>
        /// <returns>選択されたオブジェクトが全てSpriteの場合に、trueを返す。</returns>
        [MenuItem("Assets/GenerateMeshesFromSprites", validate = true)]
        static bool AreAllSelectedObjectsSprites()
        {
            if (Selection.objects.Any(x => !(x is Texture2D))) return false;
            if (Selection.objects.OfType<Texture2D>().Any(x => !x.GetSubAssets().Any())) return false;
            if (Selection.objects.OfType<Texture2D>().Any(x => !(x.GetSubAssets()[0] is Sprite))) return false;
            return true;
        }

        /// <summary>
        /// Spriteが格納されているフォルダの兄弟フォルダとしてMeshesフォルダを作成するための、パスを生成する。
        /// </summary>
        /// <param name="sprite">Sprite</param>
        /// <returns>Meshesフォルダを作成するためのパス</returns>
        static string GenerateMeshesFolderPath(this Sprite sprite)
        {
            string spriteFilePath = AssetDatabase.GetAssetPath(sprite);
            string parentFolderPath = Directory.GetParent(spriteFilePath).Parent.FullName;
            string meshesFolderPath = Path.Combine(parentFolderPath, "Meshes");
            return meshesFolderPath;
        }

        /// <summary>
        /// Meshのファイルパスを生成する。
        /// </summary>
        /// <param name="meshesFolderPath">Meshesフォルダのパス</param>
        /// <param name="fileName">ファイル名</param>
        /// <returns>Meshのファイルパス</returns>
        static string GenerateMeshFilePath(string meshesFolderPath, string fileName)
        {
            string meshesFolderPathForUnity = Regex.Replace(meshesFolderPath, @".+\\Assets\\", "Assets/");
            string filePath = $"{meshesFolderPathForUnity}/{fileName}.mesh";
            return filePath;
        }

        /// <summary>
        /// SpriteからMeshを生成する。
        /// </summary>
        /// <param name="sprite">Sprite</param>
        /// <returns>Mesh</returns>
        static Mesh ToMesh(this Sprite sprite)
        {
            var width = sprite.bounds.size.x;
            var height = sprite.bounds.size.y;
            var mesh = new Mesh();

            // MeshのサイズをSpriteのサイズに合わせるため、幅と高さで割る
            mesh.SetVertices(Array.ConvertAll(sprite.vertices, v => new Vector3(v.x / width, v.y / height, 0)).ToList());
            mesh.SetUVs(0, sprite.uv.ToList());
            mesh.SetTriangles(Array.ConvertAll(sprite.triangles, x => (int)x), 0);

            return mesh;
        }
    }
}
