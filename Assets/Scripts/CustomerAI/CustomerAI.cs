using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public interface ICustomerState
{
    void Enter(CustomerAI customer);
    void Update(CustomerAI customer);
    void Exit(CustomerAI customer);
}

public class CustomerAI : MonoBehaviour
{
    ICustomerState _currentState;

    #region 손님 변수 및 프로퍼티
    //손님이 카운터로 가는데 필요한 좌표

    //손님이 요구하는 최소치 고기
    [SerializeField] int _minMeat;

    //손님이 최대치 고기를 받는 양
    [SerializeField] int _maxMeat;
    
    //손님이 받은 현재 고기 양
    [SerializeField] int _currentMeat;

    public int _MaxMeat => _maxMeat;
    public int _MinMeat => _minMeat;
    public int _CurrentMeat => _currentMeat;
    #endregion

    public void Start()
    {
        SetState(new CustomerMoveToCounterState());
    }

    public void SetState(ICustomerState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }

    void Update()
    {
        _currentState?.Update(this);
    }
}