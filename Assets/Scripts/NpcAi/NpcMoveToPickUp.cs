using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMoveToPickUp : INpcState
{
    public void Enter(NpcAi npcAi)
    {
        MoveToPickUp();
        //�����ϸ� movePutDown���� ����
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
            // �ش� ������Ʈ�� ����� �ִٸ�? 
            if (tempList[i].CheckStack() == true)
            {
                // �ش� ������Ʈ�� ����� ��带 �������� �߰�
                break;
            }
        }
    }
}
