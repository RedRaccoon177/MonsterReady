using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "NPC/NpcData")]
public class NpcScriptableObject : ScriptableObject
{
    [Header("npc�̸�")] public string keyName;
    [Header("npc������")] public Sprite iconImage;
    [Header("npc �ִ� ����")] public int maxLevel;
    [Header("npc ���� ����")] public int startLevel;
    [Header("npc ����")] public int price;
    [Header("npc ���� ������")] public GameObject npcPrefab;
    //������
}
