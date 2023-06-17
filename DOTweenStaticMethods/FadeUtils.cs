using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class FadeUtils
{
    static Image image;

    /// <summary>
    /// 画面を黒い画面からフェードインさせる。
    /// </summary>
    /// <param name="duration"></param>
    public static void FadeIn(float duration)
    {
        FadeInFrom(Color.black, duration);
    }

    /// <summary>
    /// 画面を黒い画面へフェードアウトさせる。
    /// </summary>
    /// <param name="duration"></param>
    public static void FadeOut(float duration)
    {
        FadeOutTo(Color.black, duration);
    }

    /// <summary>
    /// 画面をフェードインさせる。
    /// </summary>
    /// <param name="from">フェード開始時の色</param>
    /// <param name="duration">時間</param>
    public static void FadeInFrom(Color from, float duration)
    {
        Fade(from, Color.clear, duration);
    }

    /// <summary>
    /// 画面をフェードアウトさせる。
    /// </summary>
    /// <param name="to">フェード終了時の色</param>
    /// <param name="duration">時間</param>
    public static void FadeOutTo(Color to, float duration)
    {
        Fade(Color.clear, to, duration);
    }

    /// <summary>
    /// 画面のフェードを行う。
    /// </summary>
    /// <param name="from">フェード開始時の色</param>
    /// <param name="to">フェード終了時の色</param>
    /// <param name="duration">時間</param>
    public static void Fade(Color from, Color to, float duration)
    {
        if (image == null)
        {
            var canvas = CreateCanvas();
            image = CreateFadeImage(canvas.transform);
        }
        image.color = from;
        image.DOColor(to, duration);
    }

    /// <summary>
    /// フェード用のキャンバスを作成する。
    /// </summary>
    /// <returns>キャンバスコンポーネント</returns>
    static Canvas CreateCanvas()
    {
        GameObject canvasObj = new GameObject("FadeCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    /// <summary>
    /// フェード用のイメージを作成する。
    /// </summary>
    /// <param name="canvasTrans">キャンバスのTransform</param>
    /// <returns>イメージコンポーネント</returns>
    static Image CreateFadeImage(Transform canvasTrans)
    {
        GameObject fadeImage = new GameObject("FadeImage");
        fadeImage.transform.SetParent(canvasTrans, false);

        var image = fadeImage.AddComponent<Image>();
        image.raycastTarget = false;
        image.rectTransform.anchorMin = Vector2.zero;
        image.rectTransform.anchorMax = Vector2.one;

        return image;
    }
}
