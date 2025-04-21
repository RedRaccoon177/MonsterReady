using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expen : MonoBehaviour, IActivable
{
    [SerializeField] bool _isActive;
    public bool isActive()
    {
        return _isActive;
    }
    public void DeActive()
    {
        _isActive = false;
    }
    public void OnActive()
    {
        _isActive = true;
        gameObject.SetActive(true);
    }
}
