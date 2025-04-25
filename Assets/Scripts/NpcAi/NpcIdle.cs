using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdle : INpcState
{
    public void Enter(NpcAi npcAi)
    {
        if (npcAi._questCor == null)
        {
            npcAi._questCor = npcAi.StartCoroutine(MoveToPickUp(npcAi));
        }
    }

    public void Exiter(NpcAi npcAi)
    {
    }

    public void Update(NpcAi npcAi)
    {
    }
    public IEnumerator MoveToPickUp(NpcAi npcAi)
    {
        yield return new WaitForSeconds(4);
        npcAi._path = null;
        npcAi._destination = null;
        while (npcAi._path == null)
        {
            var tempList = GameManager._instance._warkableObjectList;
            for (int i = 0; i < tempList.Count; i++)
            {
                Debug.Log("탐색... " + tempList[i].GetKey());
                // 해당 오브젝트에 들것이 있다면? 
                if (tempList[i].HasStack() == true)
                {
                    npcAi._destination = tempList[i];
                    Debug.Log("11 : " + NodeManager._instance.GetNearestNodeOptimized(npcAi.transform.position).transform.position);
                    Debug.Log("22 : " + GameManager._instance._npcObjectNodeDict[npcAi._destination.GetKey()].transform.position);
                    Debug.Log("33 : " + npcAi._destination.GetKey());

                    npcAi._path = AStarPathfinder.FindPath
                        (
                            NodeManager._instance.GetNearestNodeOptimized(npcAi.transform.position), // 현재 내 위치 근방 노드 찾기
                            GameManager._instance._npcObjectNodeDict[npcAi._destination.GetKey()] // 키값으로 목적지 노드 찾기
                        );
                    break;
                }
            }
            yield return new WaitForSeconds(2);
        }
        if (npcAi._path != null)
        {
            npcAi.ChangeState(npcAi._npcMoveToPickUp);
        }
    }
}
