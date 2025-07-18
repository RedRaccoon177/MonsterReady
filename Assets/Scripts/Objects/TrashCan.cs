using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : BaseObject
{
    Vector2 _nodeGridNum;
    PlayerController _player;
    private void Start()
    {
        SettingNode();
        SettingGMBaseDict();
        _player = PlayerController._instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Npc")) return;

        if (other.CompareTag("Player"))
        {
            if (_player._CurrentBone > 0)
            {
                _player.MinusBone(3);
                _player.CheckPickUpObject();
            }
        }
        else if (other.CompareTag("Npc"))
        {
            var npc = other.gameObject.GetComponent<NpcAi>();
            if (npc._CurrentBone > 0)
            {
                npc.MinusBone(3);
                npc.CurrentPickUpType();
            }
        }
    }
    public void SettingGMBaseDict()
    {
        GameManager._instance._baseObjectDict.TryAdd(GetKeyName(), this);
    }
    public void SettingNode()
    {
        Node _tempNode = NodeManager._instance._nodeList[15,18];
        GameManager._instance._npcObjectNodeDict.TryAdd(GetKeyName(), _tempNode);
        Debug.Log("추가완료 : " + GameManager._instance._npcObjectNodeDict[GetKeyName()]);
    }
}
