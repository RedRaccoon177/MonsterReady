using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdle : INpcState
{
    public void Enter(NpcAi npcAi)
    {
        npcAi._path = null;
        npcAi._destination = null;
        npcAi.currentPickUpType(); // ���� ���� ������ ��� �ִ���
        if (npcAi._pickUpObject == NpcPickUpObject.None)
        {
            if (npcAi._questCor == null)
            {
                npcAi._questCor = npcAi.StartCoroutine(MoveToPickUpPath(npcAi)); // ��� �ִ°� ���ٸ� ����(������) ã��
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
        yield return new WaitForSeconds(4f); // ��ųʸ��� ��������� ���� ����Ǿ ����
        //npcAi._questCor = null;
        while (npcAi._path == null)
        {
            var tempList = GameManager._instance._warkableObjectList;
            for (int i = 0; i < tempList.Count; i++)
            {
                Debug.Log("Ž��... " + tempList[i].GetKey());
                // �ش� ������Ʈ�� ����� �ִٸ�? 
                if (tempList[i].HasStack() == true)
                {
                    npcAi._destination = tempList[i];
                    Debug.Log("11 : " + NodeManager._instance.GetNearestNodeOptimized(npcAi.transform.position).transform.position);
                    Debug.Log("22 : " + GameManager._instance._npcObjectNodeDict[npcAi._destination.GetKey()].transform.position);
                    Debug.Log("33 : " + npcAi._destination.GetKey());

                    npcAi._path = AStarPathfinder.FindPath
                        (
                            NodeManager._instance.GetNearestNodeOptimized(npcAi.transform.position), // ���� �� ��ġ �ٹ� ��� ã��
                            GameManager._instance._npcObjectNodeDict[npcAi._destination.GetKey()] // Ű������ ������ ��� ã��
                        );
                    break;
                }
            }
            yield return new WaitForSeconds(2);
        }
        if (npcAi._path != null)
        {
            npcAi.ChangeState(npcAi._npcMove);
        }
    }

    public void MoveToPutdownPath(NpcAi npcAi)
    {
        Debug.Log(npcAi._currentState);
        var baseObject = GameManager._instance._baseObjectDict;
        var nodeObject = GameManager._instance._npcObjectNodeDict;
        npcAi.currentPickUpType();
        switch (npcAi._pickUpObject)
        {
            case NpcPickUpObject.Meat:
                if (baseObject["ī����1"].isActive() == true)
                {
                    npcAi._targetNode = nodeObject["ī����1"];
                }
                else if (baseObject["ī����2"].isActive() == true)
                {
                    npcAi._targetNode = nodeObject["ī����2"];
                }
                break;
            case NpcPickUpObject.Trash:
                if (baseObject["��������"].isActive() == true)
                {
                    npcAi._targetNode = nodeObject["��������"];
                }
                break;
            case NpcPickUpObject.MeatSat:
                break;
            case NpcPickUpObject.None:
                npcAi.ChangeState(npcAi._npcIdle);
                break;
        }
        npcAi._path = AStarPathfinder.FindPath(NodeManager._instance.GetNearestNodeOptimized(npcAi.transform.position), npcAi._targetNode);
        npcAi.ChangeState(npcAi._npcMove);
    }
}
