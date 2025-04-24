using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcIdle : INpcState
{
    public void Enter(NpcAi npcAi)
    {
        // 1ÃÊµÚ
        npcAi.ChangeState(npcAi._npcMoveToPickUp);
    }

    public void Exiter(NpcAi npcAi)
    {
    }

    public void Update(NpcAi npcAi)
    {
    }
}
