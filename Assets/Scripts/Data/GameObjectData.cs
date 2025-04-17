using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameObjectData 
{
    public List<ObjectData> objectDatas;
}

[System.Serializable]
public class ObjectData
{
    public int key;
    public string name;
    public bool isActive;
    public int level;
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
