using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public int objectLevel;
    public string objectKeyName;
    public bool objectIsActive;
}

public interface IActivable
{
    //int isKeyName();
    bool isActive();
    void OnActive();
    void DeActive();
}

