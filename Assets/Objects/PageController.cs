using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageController : MonoBehaviour
{
    [field: SerializeField]
    private Animation AnimationComponent { get; set; }
    [field: SerializeField]
    public MeshCollider TargetObjectCollider { get; private set; }
    [field: SerializeField]
    public SkinnedMeshRenderer TargetObjectRenderer { get; private set; }
    [field: SerializeField]
    public PageSide SideOfPage { get; private set; }

    private const string LEFT_TO_RIGHT_ANIMATION_NAME = "LeftToRightPageFlip";
    private const string RIGHT_TO_LEFT_ANIMATION_NAME = "RightToLeftPageFlip";
    private float TOP_PAGE_Y_POSITION = 0.00001f;

    public void UpdateNormals ()
    {
        Mesh mesh = new Mesh();
        TargetObjectRenderer.BakeMesh(mesh, true);
        mesh.RecalculateBounds();
        TargetObjectCollider.sharedMesh = null;
        TargetObjectCollider.sharedMesh = mesh;
    }

    public void FlipPage (float animationPlaybackSpeed)
    {
        string animatorTriggerName;
        PageSide targetSide;

        targetSide = SideOfPage == PageSide.LEFT ? PageSide.RIGHT : PageSide.LEFT;
        animatorTriggerName = GetAnimationNameByTargetSide(targetSide);
        AnimationComponent[animatorTriggerName].speed = animationPlaybackSpeed;
        AnimationComponent.Play(animatorTriggerName);
        SideOfPage = targetSide;
        StartCoroutine(UpdateNormalsAfterAnimationIsFinished());
    }

    public void SetPageToSide (PageSide targetSide)
    {
        string targetAnimationName = GetAnimationNameByTargetSide(targetSide);
        AnimationState animationState = AnimationComponent[targetAnimationName];
        animationState.normalizedTime = 1;
        AnimationComponent.Play(targetAnimationName);
        StartCoroutine(UpdateNormalsAfterAnimationIsFinished());
    }

    public void SetTopPageMark (int pageIndex)
    {
        float yPosition = pageIndex * TOP_PAGE_Y_POSITION;

        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
    }

    public bool IsAnimating ()
    {
        return AnimationComponent.isPlaying;
    }

    private IEnumerator UpdateNormalsAfterAnimationIsFinished ()
    {
        yield return new WaitWhile(() => AnimationComponent.isPlaying);
        UpdateNormals();
    }

    private string GetAnimationNameByTargetSide (PageSide targetSide)
    {
        return targetSide == PageSide.LEFT ? RIGHT_TO_LEFT_ANIMATION_NAME : LEFT_TO_RIGHT_ANIMATION_NAME;
    }
}
