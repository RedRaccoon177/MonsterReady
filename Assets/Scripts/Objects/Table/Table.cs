using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : BaseObject, ILevelable , IStackChecker
{
    [SerializeField] public int _level;
    [SerializeField] int _currentTrashCount;

    private void Start()
    {
        _currentTrashCount = 0;
    }
    public string GetKey()
    {
        return _keyName;
    }

    public int SetLevel(int level)
    {
        _level = level;
        return level;
    }

    public void LevelUp()
    {
        _level++;
    }

    public int GetLevel()
    {
        return _level;
    }

    public bool CheckStack()
    {
        return _currentTrashCount > 0  ;
    }
}
