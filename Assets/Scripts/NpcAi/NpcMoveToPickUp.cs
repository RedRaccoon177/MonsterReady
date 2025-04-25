using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NpcMoveToPickUp : INpcState
{
    int _currentIndex;
    public void Enter(NpcAi npcAi)
    {
        _currentIndex = 0;
        Debug.Log("�����");
        //�����ϸ� movePutDown���� ����
    }

    public void Exiter(NpcAi npcAi)
    {
    }

    public void Update(NpcAi npcAi)
    {
        // ��ΰ� ���ų� �̹� �����ߴٸ� ���� ���·� ��ȯ
        if (npcAi._path == null || _currentIndex >= npcAi._path.Count)
        {
            npcAi.ChangeState(npcAi._npcMoveToPutDown); // ���� �ൿ ���·� ��ȯ (�ֹ� ��� ��)
            return;
        }
        npcAi.transform.position = Vector3.MoveTowards(npcAi.transform.position, npcAi._path[_currentIndex].transform.position, 4*Time.deltaTime);
        if (Vector3.Distance(npcAi.transform.position, npcAi._path[_currentIndex].transform.position) < 0.1f)
        {
            _currentIndex++;
        }
    }
    
}
