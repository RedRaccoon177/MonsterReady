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
            npcAi.ChangeState(npcAi._npcMoveToPickUp);
        }
    }
}
