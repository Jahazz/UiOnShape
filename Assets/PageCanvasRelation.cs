using System;
using UnityEngine;

[Serializable]
public class PageCanvasRelation
{
    [field: SerializeField]
    public PageController PageControllerInstance { get; private set; }
    [field: SerializeField]
    public CanvasController CanvasControllerTopInstance { get; private set; }
    [field: SerializeField]
    public CanvasController CanvasControllerBottomInstance { get; private set; }
}
