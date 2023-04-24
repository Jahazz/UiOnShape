using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [field: SerializeField]
    public RectTransform UiMainRectTransform { get; private set; }
    [field: SerializeField]
    private RectTransform FakeCursor { get; set; }
    [field: SerializeField]
    private RenderTexture TargetRenderTexture { get; set; }
    [field: SerializeField]
    private GraphicRaycaster GraphicsRaycasterInstance { get; set; }

    public void SetFakeCursorPosition (Vector2 position)
    {
        FakeCursor.anchoredPosition = position;
    }

    public void SetStateOfRaycaster (bool isEnabled)
    {
        GraphicsRaycasterInstance.enabled = isEnabled;
    }
}
