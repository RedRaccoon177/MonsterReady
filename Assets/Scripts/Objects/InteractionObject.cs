using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    public string _keyName;
    public bool _isActive;
    public bool isActive() 
    {
        return _isActive;
    }
    public void DeActive()
    {
        _isActive = false;
        gameObject.SetActive(false);
    }
    public void OnActive()
    {
        _isActive = true;
        gameObject.SetActive(true);
    }
}

public interface ILevelable 
{
    int GetLevel();
    int SetLevel(int level);
    void LevelUp();
}

