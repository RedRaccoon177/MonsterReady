using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : BaseObject, ILevelable,INpcDestination
{
    [SerializeField] public int _level;
    [SerializeField] public int _currentTrashCount;
    [SerializeField] public Vector2 _nodeGridNum;

    private void Start()
    {
        _currentTrashCount = 0;
        SettingNode();
        SettingGMBaseDict();
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
    public bool HasStack()
    {
        return _currentTrashCount > 0;
    }
    public int GetStackCount()
    {
        return _currentTrashCount;
    }

    public void SettingNode() 
    {
        Node _tempNode = NodeManager._instance._nodeList[(int)_nodeGridNum.x, (int)_nodeGridNum.y];
        GameManager._instance._npcObjectNodeDict.TryAdd(_keyName, _tempNode);
    }
    public void SettingGMBaseDict()
    {
        GameManager._instance._baseObjectDict.TryAdd(_keyName, this);
    }
}
