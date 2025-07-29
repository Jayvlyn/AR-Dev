using System;
using UnityEngine;

public class MainAttachPoint : MonoBehaviour
{
    public static Transform mainAttachPoint;

    private void Awake()
    {
        mainAttachPoint = this.transform;
    }
}
