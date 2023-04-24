using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;
using System.Linq;

public class ObjectToCanvasRaycaster : MonoBehaviour
{

    [field: SerializeField]
    private PlayerInput PlayerInputInstance { get; set; }
    [field: SerializeField]
    private bool IsDebug { get; set; }

    [field: Space]
    [field: SerializeField]
    private int TargetRenderTextureWidth { get; set; }
    [field: SerializeField]
    private List<PageCanvasRelation> PageCanvasInspectorCollection { get; set; } = new List<PageCanvasRelation>();
    private PageCanvasRelation LeftPage { get; set; }
    private PageCanvasRelation RightPage { get; set; }

    private Mouse VirtualMouse { get; set; }
    private const string VIRTUAL_MOUSE_NAME = "VirtualMouse";

    public void FlipPageLeftToRight ()
    {
        if (LeftPage != null && LeftPage?.CanvasControllerTopInstance != null)
        {
            LeftPage.PageControllerInstance.FlipPage();
        }

        RecalculatePages();
    }

    public void FlipPageRightToLeft ()
    {

        if (RightPage != null && RightPage?.CanvasControllerBottomInstance != null)
        {
            RightPage.PageControllerInstance.FlipPage();
        }

        RecalculatePages();
    }

    protected virtual void Update ()
    {
        RaycastPages();
    }

    private void RaycastPages ()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log(hit.transform.name);
            CanvasController currentCanvasController = null;

            if (hit.collider == LeftPage?.PageControllerInstance.TargetObjectCollider)
            {
                currentCanvasController = LeftPage.CanvasControllerBottomInstance;
            }
            else if (hit.collider == RightPage?.PageControllerInstance.TargetObjectCollider)
            {
                currentCanvasController = RightPage.CanvasControllerTopInstance;
            }

            if (currentCanvasController != null)
            {
                CheckHitForPage(ray, hit, currentCanvasController);
            }
        }
    }

    private void CheckHitForPage (Ray ray, RaycastHit hit, CanvasController currentCanvasController)
    {
        Vector2 textureCoord = hit.textureCoord;
        textureCoord = new Vector2(textureCoord.x * currentCanvasController.UiMainRectTransform.rect.width, textureCoord.y * currentCanvasController.UiMainRectTransform.rect.height);

        if (IsDebug == true)
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
        }
        DisableAllExcept(currentCanvasController);
        InputState.Change(VirtualMouse, textureCoord);
    }

    public void DisableAllExcept (CanvasController currentCanvasController)
    {
        foreach (PageCanvasRelation item in PageCanvasInspectorCollection)
        {
            if (item.CanvasControllerBottomInstance != currentCanvasController)
            {
                item.CanvasControllerBottomInstance?.SetStateOfRaycaster(false);
            }
            else
            {
                item.CanvasControllerBottomInstance?.SetStateOfRaycaster(true);
            }

            if (item.CanvasControllerTopInstance != currentCanvasController)
            {
                item.CanvasControllerTopInstance?.SetStateOfRaycaster(false);
            }
            else
            {
                item.CanvasControllerTopInstance?.SetStateOfRaycaster(true);
            }
        }
    }


    public virtual void Awake ()
    {
        SetupVirtualMouse();
        SetPagesToDefaultStates();
        RecalculatePages();
    }

    private void SetPagesToDefaultStates ()
    {
        foreach (var page in PageCanvasInspectorCollection)
        {
            page.PageControllerInstance.SetPageToSide(page.PageControllerInstance.SideOfPage);
        }
    }

    private void SetupVirtualMouse ()
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

    private PageCanvasRelation GetPageCanvasPair (PageController controllerToFindBy)
    {
        return PageCanvasInspectorCollection.Where(x => x.PageControllerInstance == controllerToFindBy).FirstOrDefault();
    }
    private PageCanvasRelation GetPageCanvasPair (CanvasController controllerToFindBy)
    {
        return PageCanvasInspectorCollection.Where(x => x.CanvasControllerTopInstance == controllerToFindBy || x.CanvasControllerBottomInstance == controllerToFindBy).FirstOrDefault();
    }

    public void FitToObject ()
    {
        //Vector3 TargetColliderSize = TargetObjectCollider.bounds.size;
        //float ratio = TargetColliderSize.z / TargetColliderSize.x;
        //TargetRenderTexture.Release();
        //TargetRenderTexture.width = TargetRenderTextureWidth;
        //TargetRenderTexture.height = (int)(TargetRenderTexture.width * ratio);
        //TargetRenderTexture.Create();
    }

    private void RecalculatePages ()
    {
        PageCanvasRelation leftPage = null;
        PageCanvasRelation rightPage = null;


        PageCanvasRelation page;

        for (int i = PageCanvasInspectorCollection.Count - 1; i >= 0; i--)
        {
            page = PageCanvasInspectorCollection[i];

            if (leftPage == null && page.PageControllerInstance.SideOfPage == PageSide.LEFT)
            {
                leftPage = page;
            }
        }

        for (int i = 0; i < PageCanvasInspectorCollection.Count; i++)
        {
            page = PageCanvasInspectorCollection[i];

            if (rightPage == null && page.PageControllerInstance.SideOfPage == PageSide.RIGHT)
            {
                rightPage = page;
            }
        }

        LeftPage = leftPage;
        RightPage = rightPage;

        int leftIndex = PageCanvasInspectorCollection.IndexOf(leftPage);
        int rightIndex = PageCanvasInspectorCollection.IndexOf(rightPage);

        for (int i = leftIndex; i >= 0; i--)//todo: just one loop with if
        {
            PageCanvasInspectorCollection[i].PageControllerInstance.SetTopPageMark(i-1);
        }

        for (int i = rightIndex; i < PageCanvasInspectorCollection.Count; i++)
        {
            PageCanvasInspectorCollection[i].PageControllerInstance.SetTopPageMark(rightIndex-i-1);
        }

        LeftPage?.PageControllerInstance.SetTopPageMark(leftIndex);
        RightPage?.PageControllerInstance.SetTopPageMark(rightIndex);
    }
}
