using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAiMoveToPutDown:INpcState
{
    public void Enter(NpcAi npcAi)
    {
        switch (npcAi._pickUpObject)
        {
            case NpcPickUpObject.Meat:
                // ī���� 1 or ī����3
                break;
            case NpcPickUpObject.Trash:
                // ��������
                break;
            case NpcPickUpObject.MeatSat:
                // ī���� 2
                break;
            case NpcPickUpObject.None:
                // idel��
                break;
        }
    }

    public void Exiter(NpcAi npcAi)
    {

    }

    public void Update(NpcAi npcAi)
    {

    }

}
