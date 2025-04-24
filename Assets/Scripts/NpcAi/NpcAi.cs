using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NpcPickUpObject
{
    Meat,
    Trash,
    MeatSat,
    None
}

public interface INpcState
{
    public void Enter(NpcAi npcAi);
    public void Exiter(NpcAi npcAi);
    public void Update(NpcAi npcAi);
}
public class NpcAi : MonoBehaviour
{
    public string _keyName;
    int _currentLevel;
    int _currentMaxLevel;
    float _moveSpeed;
    int _holdMaxAmount;
    bool _isUnlockNpc;
    public NpcPickUpObject _pickUpObject;
    INpcState _currentState;
    Vector3 _goalPos;
    public NpcIdle _npcIdle;
    public NpcAiMoveToPutDown _npcMoveToPutDown;
    public NpcMoveToPickUp _npcMoveToPickUp;

    public void ChangeState(INpcState nextState)
    {
        _currentState?.Exiter(this);
        _currentState = nextState;
        _currentState?.Enter(this);
    }
    private void Start()
    {
        ChangeState(_npcIdle);
    }
    private void Update()
    {
        _currentState.Update(this);
    }

    
}
