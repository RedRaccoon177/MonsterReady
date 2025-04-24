using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMoveToPickUp : INpcState
{
    public void Enter(NpcAi npcAi)
    {
        MoveToPickUp();
        //도착하면 movePutDown으로 변경
    }

    public void Exiter(NpcAi npcAi)
    {
    }

    public void Update(NpcAi npcAi)
    {
    }
    public void MoveToPickUp()
    {
        var tempList = GameManager._instance._stackableObjectList;
        for (int i = 0; i < tempList.Count; i++)
        {
            // 해당 오브젝트에 들것이 있다면? 
            if (tempList[i].CheckStack() == true)
            {
                // 해당 오브젝트와 연결된 노드를 목적지로 추가
                break;
            }
        }
    }
}
