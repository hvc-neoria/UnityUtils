using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HvcNeoria.Unity.Utils
{
    public static class RendererExtensions
    {
        /// <summary>
        /// マテリアルの色を設定する。
        /// マテリアルを複製せずに済む。
        /// </summary>
        /// <param name="renderer">レンダラー</param>
        /// <param name="color">色</param>
        public static void SetColor(this Renderer renderer, Color color)
        {
            var block = new MaterialPropertyBlock();
            block.SetColor("_Color", color);
            renderer.SetPropertyBlock(block);

            // 複数回色を変更する場合は、下記ID算出を先に行うことで処理を減らすことができる
            // int id = Shader.PropertyToID("_Color");
            // block.SetColor(id, Color.red);
            // https://qiita.com/OKsaiyowa/items/465caccc9e0b9d94ba35
        }

        /// <summary>
        /// マテリアルの色を取得する。
        /// </summary>
        /// <param name="renderer">レンダラー</param>
        /// <returns>色</returns>
        public static Color GetColor(this Renderer renderer)
        {
            var block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            return block.GetColor("_Color");
        }

        /// <summary>
        /// マテリアルのテクスチャを設定する。
        /// マテリアルを複製せずに済む。
        /// </summary>
        /// <param name="renderer">レンダラー</param>
        /// <param name="texture">テクスチャー</param>
        public static void SetTexture(this Renderer renderer, Texture texture)
        {
            var block = new MaterialPropertyBlock();
            block.SetTexture("_MainTex", texture);
            renderer.SetPropertyBlock(block);
        }
    }
}
