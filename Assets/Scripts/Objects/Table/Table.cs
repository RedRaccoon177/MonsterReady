using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : BaseObject, ILevelable
{
    [SerializeField] public int _level;
    [SerializeField] public bool _isTrash { get; private set; }
    [SerializeField] public int _trashCount { get; private set; }

    private void Start()
    {
        _trashCount = 0;
        _isTrash = false;
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
}
