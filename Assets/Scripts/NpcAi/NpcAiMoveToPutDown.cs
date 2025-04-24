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
                // 카운터 1 or 카운터3
                break;
            case NpcPickUpObject.Trash:
                // 쓰레기통
                break;
            case NpcPickUpObject.MeatSat:
                // 카운터 2
                break;
            case NpcPickUpObject.None:
                // idel로
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
