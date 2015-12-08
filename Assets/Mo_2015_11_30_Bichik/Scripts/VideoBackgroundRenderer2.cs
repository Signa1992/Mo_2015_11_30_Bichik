using UnityEngine;
using System;
using System.Collections;
using Vuforia;

/// <summary>
/// Обеспечивает двухпроходную отрисовку измображения с камеры, сохроняя отрисованное в пространстве камеры изображение в текстуру.
/// Этот метод необходим из-за разницы разрешений экрана и камеры.
/// В последующем рендеринг может быть заменен на правильное считывание тестуры, непосредственно из Vuforia, но с воссозданием механизма компенсации разниц разрешений.
/// </summary>
[RequireComponent(typeof(Camera))]
public class VideoBackgroundRenderer2 : MonoBehaviour
{
    public static VideoBackgroundRenderer2 Instance { get; private set; }

    public VideoBackgroundRenderer2() : base()
    {
        Instance = this;
    }   

    [Header("General")]
    public LayerMask BackgroundLayer = 31;

    private Camera AugmentedCamera;
    private RenderTexture backgroundTexture;

    public IEnumerator RenderingCoroutine(Action<Texture> onResponse)
    {
        yield return new WaitForEndOfFrame();

        var currentCullingMask = Instance.AugmentedCamera.cullingMask;
        Instance.AugmentedCamera.cullingMask = Instance.BackgroundLayer;
        Instance.AugmentedCamera.targetTexture = backgroundTexture;
        RenderTexture.active = backgroundTexture;
        Instance.AugmentedCamera.Render();
        Instance.AugmentedCamera.cullingMask = currentCullingMask;
        Instance.AugmentedCamera.targetTexture = null;

        if (onResponse != null)
        {
            onResponse(backgroundTexture);
        }
    }

    public static void RequestBackgroundTexture(Action<Texture> onResponse)
    {
        Instance.StartCoroutine(Instance.RenderingCoroutine(onResponse));
    }

    private void Awake()
    {
        AugmentedCamera = GetComponent(typeof(Camera)) as Camera;
        backgroundTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
    }
}
