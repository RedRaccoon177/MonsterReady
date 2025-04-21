using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� ������Ʈ
[System.Serializable]
public class GameObjectDataList 
{
    public List<ObjectData> objectDatas;
}

[System.Serializable]
public class ObjectData
{
    public string key;
    public bool isActive;
    public int level;
}

// �ر� ������Ʈ
[System.Serializable]
public class ActivatorList
{
    public List<Activator> activators;
}

[System.Serializable]
public class Activator
{
    public int step;
    public bool isActive;
}
