using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NpcAiMoveToPutDown:INpcState
{
    public void Enter(NpcAi npcAi)
    {
        Debug.Log("�����222");
        npcAi._pickUpObject = NpcPickUpObject.Trash;
        switch (npcAi._pickUpObject)
        {
            case NpcPickUpObject.Meat:
                var a =GameManager._instance._npcObjectNodeDict["ī����1"];
                var b =GameManager._instance._npcObjectNodeDict["ī����3"];
                break;
            case NpcPickUpObject.Trash:
                //var c =GameManager._instance.test["��������"];
                break;
            case NpcPickUpObject.MeatSat:
                var d =GameManager._instance._npcObjectNodeDict["ī����2"];
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
