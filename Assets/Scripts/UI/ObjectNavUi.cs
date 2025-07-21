using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObjectNavUi : UiBase
{
    [SerializeField] public Vector3 targetPos { get; set; }
    private void LateUpdate()
    {
        if (targetPos == Vector3.zero)
        {
            return;
        }
        Vector3 viewPortPos = Camera.main.WorldToScreenPoint(targetPos);
        transform.position = viewPortPos;
    }
    private void OnDisable()
    {
        targetPos = Vector3.zero;
    }
}
