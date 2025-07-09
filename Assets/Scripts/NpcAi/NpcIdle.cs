using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdle : INpcState
{
    public void Enter(NpcAi npcAi)
    {
        npcAi._path = null;
        npcAi.currentPickUpType(); // 현재 내가 무엇을 들고 있는지
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
                // 해당 오브젝트에 들것이 있다면? , 다른 npc가 목적지로 설정 안했다면
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
                npcAi._destination = (INpcDestination)GameManager._instance._baseObjectDict["카운터1"];
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
                if (baseObject["카운터1"].isActive() == true)
                {
                    Debug.Log(1111111111111111111);
                    npcAi._targetNode = nodeObject["카운터1"];
                }
                else if (baseObject["카운터2"].isActive() == true)
                {
                    npcAi._targetNode = nodeObject["카운터2"];
                }
                break;
            case NpcPickUpObject.Bone:
                npcAi._targetNode = nodeObject["쓰레기통"];
                break;
            case NpcPickUpObject.MeatSat:
                break;
            case NpcPickUpObject.None:
                npcAi.ChangeState(npcAi._npcIdle);
                break;
        }
        Debug.Log("목표 : " + npcAi._targetNode._gridPos);
        npcAi._path = AStarPathfinder.FindPath(NodeManager._instance.GetNearestNodeOptimized(npcAi.transform.position), npcAi._targetNode);
        npcAi.ChangeState(npcAi._npcMove);
    }
}
