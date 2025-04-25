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

    #region �մ� ���� �� ������Ƽ
    //�մ��� ī���ͷ� ���µ� �ʿ��� ��ǥ

    //�մ��� �䱸�ϴ� �ּ�ġ ���
    [SerializeField] int _minMeat;

    //�մ��� �ִ�ġ ��⸦ �޴� ��
    [SerializeField] int _maxMeat;
    
    //�մ��� ���� ���� ��� ��
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