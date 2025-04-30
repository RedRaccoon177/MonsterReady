using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NpcDataList
{
    public List<NpcData> npcDatas; 
}
[Serializable]
public class NpcData
{
    public string keyName;
    public int currentLevel;
    public bool isUnlock;
}
