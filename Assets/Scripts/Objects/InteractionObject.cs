using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [Header("오브젝트 레벨")]
    public int objectLevel;

    [Header("오브젝트 키 이름")]
    public string objectKeyName;

    [Header("오브젝트 활성화")]
    public bool objectIsActive;
}
