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
    public Table _table;         // 내가 앉아있는 테이블
    public int _AteMeatCount = 0; // 먹은 고기 총 개수

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

    /// <summary>
    /// 고기 하나 먹기
    /// </summary>
    public void EatOneMeat()
    {
        if (_currentMeat > 0)
        {
            _currentMeat--;
            _AteMeatCount++;
        }
    }
}