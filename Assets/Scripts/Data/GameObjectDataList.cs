using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectDataList 
{
    public List<ObjectData> objectDatas;
}

[System.Serializable]
public class ObjectData
{
    public string keyName;
    public bool isActive;
    public int level;
}

/// <summary>
/// 해금 오브젝트 리스트
/// </summary>
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


/// <summary>
/// 모든 오브젝트 활성화 여부
/// </summary>
[System.Serializable]
public class ActiveObjectList
{
    public List<ActiveObject> activeObjects;
}

[System.Serializable]
public class ActiveObject
{
    public string key;
    public bool isActive;
}
#region

//[System.Serializable]
//public class GrillData
//{
//    public bool isActive;
//    public int grillLevel;
//}

//[System.Serializable]
//public class CounterData
//{
//    public bool isActive;
//    public int counterLevel;
//}

//[System.Serializable]
//public class TableData
//{
//    public bool isActive;
//    public int tableLevel;
//}

#endregion
