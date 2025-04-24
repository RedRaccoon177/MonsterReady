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