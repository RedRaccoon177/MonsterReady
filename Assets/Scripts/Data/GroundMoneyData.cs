using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroundMoneyDataList
{
    public List<GroundMoneyData> groundMoneys;
}

[System.Serializable]
public class GroundMoneyData
{
    string key;
    int currentMoney;
}

