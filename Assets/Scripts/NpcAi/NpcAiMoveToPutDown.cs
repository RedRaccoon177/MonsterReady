using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NpcAiMoveToPutDown:INpcState
{
    public void Enter(NpcAi npcAi)
    {
        Debug.Log("여기야222");
        npcAi._pickUpObject = NpcPickUpObject.Trash;
        switch (npcAi._pickUpObject)
        {
            case NpcPickUpObject.Meat:
                var a =GameManager._instance._npcObjectNodeDict["카운터1"];
                var b =GameManager._instance._npcObjectNodeDict["카운터3"];
                break;
            case NpcPickUpObject.Trash:
                //var c =GameManager._instance.test["쓰레기통"];
                break;
            case NpcPickUpObject.MeatSat:
                var d =GameManager._instance._npcObjectNodeDict["카운터2"];
                break;
            case NpcPickUpObject.None:
                npcAi.ChangeState(npcAi._npcIdle);
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
