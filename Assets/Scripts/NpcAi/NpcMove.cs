using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcMove : INpcState
{
    int _currentIndex;
    public void Enter(NpcAi npcAi)
    {
        npcAi._npcAnimator.SetBool("IsWalk",true);
        _currentIndex = 0;
    }

    public void Exiter(NpcAi npcAi)
    {
        npcAi._npcAnimator.SetBool("IsWalk",false);
    }

    public void Update(NpcAi npcAi)
    {
        if (npcAi._path == null || _currentIndex >= npcAi._path.Count)
        {
            npcAi._destination.OffDestination();
            npcAi.ChangeState(npcAi._npcIdle);
            return;
        }

        Transform target = npcAi._path[_currentIndex].transform;

        // 1. 이동 방향 계산
        Vector3 direction = (target.position - npcAi.transform.position).normalized;
        direction.y = 0f; // y축 회전 방지 (지면 기준)

        // 2. 회전
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            npcAi.transform.rotation = Quaternion.Slerp(npcAi.transform.rotation, targetRotation, Time.deltaTime * 7f); // 7은 회전 속도
        }

        // 3. 이동
        npcAi.transform.position = Vector3.MoveTowards(npcAi.transform.position, target.position, npcAi._moveSpeed * Time.deltaTime);

        if (Vector3.Distance(npcAi.transform.position, target.position) < 0.1f)
        {
            _currentIndex++;
        }
    }
}
