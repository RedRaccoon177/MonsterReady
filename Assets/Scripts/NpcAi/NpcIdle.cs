using System.Collections;
using UnityEngine;

public class NpcIdle : INpcState
{
    public void Enter(NpcAi npcAi)
    {
        npcAi._path = null;
        //npcAi.CurrentPickUpType(); // 현재 내가 무엇을 들고 있는지
        if (npcAi._pickUpObject == NpcPickUpObject.None)
        {
            if (npcAi._questCor == null)
            {
                npcAi._questCor = npcAi.StartCoroutine(MoveToPickUpPath(npcAi)); // 들고 있는게 없다면 할일(목적지) 찾기
            }
        }
        else
        {
            MoveToPutdownPath(npcAi);
        }
    }

    public void Exiter(NpcAi npcAi)
    {
    }

    public void Update(NpcAi npcAi)
    {
    }

    public void MoveToPutDown()
    {

    }
    public IEnumerator MoveToPickUpPath(NpcAi npcAi)
    {
        yield return new WaitForSeconds(4f); // 딕셔너리가 만들어지기 전에 실행되어서 넣음
        npcAi._questCor = null;
        while (npcAi._path == null)
        {
            var tempList = GameManager._instance._warkableObjectList;
            var maxHasStackIdx = 0;
            for (int i = 0; i < tempList.Count; i++)
            {
                if (tempList[i].HasStack() == true && tempList[i].IsDestination() == false)
                {
                    if (tempList[maxHasStackIdx].GetStackCount() < tempList[i].GetStackCount())
                    {
                        maxHasStackIdx = i;
                    }
                }
            }

            if (maxHasStackIdx == 0 && tempList[0].HasStack() == false)
            {
                npcAi._destination = (INpcDestination)GameManager._instance._baseObjectDict["counter1"];
            }
            else
            {
                tempList[maxHasStackIdx].OnDestination();
                npcAi._destination = tempList[maxHasStackIdx];
            }
            npcAi._path = AStarPathfinder.FindPath
            (
                NodeManager._instance.GetNearestNodeOptimized(npcAi.transform.position), // 현재 내 위치 근방 노드 찾기
                GameManager._instance._npcObjectNodeDict[npcAi._destination.GetKey()] // 키값으로 목적지 노드 찾기
            );

        }
        if (npcAi._path != null)
        {
            npcAi.ChangeState(npcAi._npcMove);
        }
        else
        {
            npcAi.ChangeState(npcAi._npcIdle);
        }
    }

    public void MoveToPutdownPath(NpcAi npcAi)
    {
        var baseObject = GameManager._instance._baseObjectDict;
        var nodeObject = GameManager._instance._npcObjectNodeDict;
        switch (npcAi._pickUpObject)
        {
            case NpcPickUpObject.Meat:
                var rand = Random.Range(0,2);
                if (baseObject["counter1"].isActive() == true)
                {
                    npcAi._targetNode = nodeObject["counter1"];
                }
                if (GameManager._instance._isCounterSecondActive == true && rand == 1)
                {
                    npcAi._targetNode = NodeManager._instance._nodeList[19,5];
                }
                break;
            case NpcPickUpObject.Bone:
                npcAi._targetNode = nodeObject["trashCan"];
                break;
            case NpcPickUpObject.Box:
                Debug.Log("목적지 노드 : " + nodeObject["counter2"]._gridPos);
                npcAi._targetNode = nodeObject["counter2"];
                break;
            case NpcPickUpObject.None:
                npcAi.ChangeState(npcAi._npcIdle);
                break;
        }
        npcAi._path = AStarPathfinder.FindPath(NodeManager._instance.GetNearestNodeOptimized(npcAi.transform.position), npcAi._targetNode);
        npcAi.ChangeState(npcAi._npcMove);
    }
}
