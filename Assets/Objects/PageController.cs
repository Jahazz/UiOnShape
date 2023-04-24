using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageController : MonoBehaviour
{
    [field: SerializeField]
    private Animator AnimatorComponent { get; set; }
    [field: SerializeField]
    public MeshCollider TargetObjectCollider { get; private set; }
    [field: SerializeField]
    public SkinnedMeshRenderer TargetObjectRenderer { get; private set; }
    [field: SerializeField]
    public PageSide SideOfPage { get; private set; }

    private const string LEFT_TO_RIGHT_ANIMATOR_TRIGGER_NAME = "LeftToRight";
    private const string RIGHT_TO_LEFT_ANIMATOR_TRIGGER_NAME = "RightToLeft";
    private float TOP_PAGE_Y_POSITION = 0.00001f;
    private float DEFAULT_PAGE_Y_POSITION = 0;

    public void Update ()
    {
        Mesh mesh = new Mesh();
        TargetObjectRenderer.BakeMesh(mesh,true);
        mesh.RecalculateBounds();
        TargetObjectCollider.sharedMesh = null;
        TargetObjectCollider.sharedMesh = mesh;
    }

    public void FlipPage ()
    {
        string animatorTriggerName;
        PageSide currentSide;

        if (SideOfPage == PageSide.LEFT)
        {
            animatorTriggerName = LEFT_TO_RIGHT_ANIMATOR_TRIGGER_NAME;
            currentSide = PageSide.RIGHT;
        }
        else
        {
            animatorTriggerName = RIGHT_TO_LEFT_ANIMATOR_TRIGGER_NAME;
            currentSide = PageSide.LEFT;
        }

        AnimatorComponent.SetTrigger(animatorTriggerName);
        SideOfPage = currentSide;
    }

    public void SetTopPageMark(bool isTop)
    {
        float yPosition;

        if (isTop == true)
        {
            yPosition = TOP_PAGE_Y_POSITION;
        }
        else
        {
            yPosition = DEFAULT_PAGE_Y_POSITION;
        }

        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
    }
}
