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
        Debug.Log("여기야");
        //도착하면 movePutDown으로 변경
    }

    public void Exiter(NpcAi npcAi)
    {
    }

    public void Update(NpcAi npcAi)
    {
        // 경로가 없거나 이미 도착했다면 다음 상태로 전환
        if (npcAi._path == null || _currentIndex >= npcAi._path.Count)
        {
            npcAi.ChangeState(npcAi._npcMoveToPutDown); // 다음 행동 상태로 전환 (주문 대기 등)
            return;
        }
        npcAi.transform.position = Vector3.MoveTowards(npcAi.transform.position, npcAi._path[_currentIndex].transform.position, 4*Time.deltaTime);
        if (Vector3.Distance(npcAi.transform.position, npcAi._path[_currentIndex].transform.position) < 0.1f)
        {
            _currentIndex++;
        }
    }
    
}
