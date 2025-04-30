using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "NPC/NpcData")]
public class NpcScriptableObject : ScriptableObject
{
    [Header("npc이름")] public string keyName;
    [Header("npc아이콘")] public Sprite iconImage;
    [Header("npc 최대 레벨")] public int maxLevel;
    [Header("npc 시작 레벨")] public int startLevel;
    [Header("npc 가격")] public int price;
    [Header("npc 외형 프리팹")] public GameObject npcPrefab;
    //아이콘
}
