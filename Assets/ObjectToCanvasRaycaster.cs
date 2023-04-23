using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class ObjectToCanvasRaycaster : MonoBehaviour
{

    [field: SerializeField]
    private PlayerInput PlayerInputInstance { get; set; }
    [field: SerializeField]
    private RectTransform UiMainRectTransform { get; set; }
    [field: SerializeField]
    private RectTransform FakeCursor { get; set; }
    [field: SerializeField]
    private bool IsDebug { get; set; }

    [field: Space]
    [field: SerializeField]
    private RenderTexture TargetRenderTexture { get; set; }
    [field: SerializeField]
    private Collider TargetObjectCollider { get; set; }
    [field: SerializeField]
    private int TargetRenderTextureWidth { get; set; }

    private Mouse VirtualMouse { get; set; }
    private const string VIRTUAL_MOUSE_NAME = "VirtualMouse";


    protected virtual void Update ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Vector2 textureCoord = hit.textureCoord;
            textureCoord = new Vector2(textureCoord.x * UiMainRectTransform.rect.width, textureCoord.y * UiMainRectTransform.rect.height);

            if (IsDebug == true)
            {
                Debug.Log(hit.transform.name);
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                FakeCursor.anchoredPosition = textureCoord;
            }

            InputState.Change(VirtualMouse, textureCoord);
        }
    }


    public virtual void Awake ()
    {
        VirtualMouse = (Mouse)InputSystem.GetDevice(VIRTUAL_MOUSE_NAME);

        if (VirtualMouse == null)
        {
            VirtualMouse = (Mouse)InputSystem.AddDevice(VIRTUAL_MOUSE_NAME);
        }
        else if (VirtualMouse.added == false)
        {
            InputSystem.AddDevice(VirtualMouse);
        }

        InputUser.PerformPairingWithDevice(VirtualMouse, PlayerInputInstance.user);
    }

    public void FitToObject ()
    {
        Vector3 TargetColliderSize = TargetObjectCollider.bounds.size;
        float ratio = TargetColliderSize.z/TargetColliderSize.x;
        TargetRenderTexture.Release();
        TargetRenderTexture.width = TargetRenderTextureWidth;
        TargetRenderTexture.height = (int)(TargetRenderTexture.width * ratio);
        TargetRenderTexture.Create();
    }
}
