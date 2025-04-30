using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMove : INpcState
{
    int _currentIndex;
    public void Enter(NpcAi npcAi)
    {
        _currentIndex = 0;
    }

    public void Exiter(NpcAi npcAi)
    {
    }

    public void Update(NpcAi npcAi)
    {
        // 경로가 없거나 이미 도착했다면 다음 상태로 전환
        if (npcAi._path == null || _currentIndex >= npcAi._path.Count)
        {
            npcAi.ChangeState(npcAi._npcIdle);
            return;
        }
        npcAi.transform.position = Vector3.MoveTowards(npcAi.transform.position, npcAi._path[_currentIndex].transform.position, 4 * Time.deltaTime);
        if (Vector3.Distance(npcAi.transform.position, npcAi._path[_currentIndex].transform.position) < 0.1f)
        {
            _currentIndex++;
        }
    }
}
