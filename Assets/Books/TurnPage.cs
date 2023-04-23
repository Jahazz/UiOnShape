using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnPage : MonoBehaviour
{
    [field: SerializeField]
    List<Transform> Bones { get; set; }

    private Vector3 EndPosition { get; set; }

    // Start is called before the first frame update
    void Start()
    {

    }
}
