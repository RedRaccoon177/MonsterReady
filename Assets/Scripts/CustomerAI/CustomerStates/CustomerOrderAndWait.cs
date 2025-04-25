using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CustomerOrderAndWait : ICustomerState
{
    int _customerCurrentMeat;

    //ī������ ����
    Counter _counter = GameManager._instance._counters[0];

    int _requestedMeat;

    public void Enter(CustomerAI customer)
    {
        // �մ��� ���ϴ� ��� ����
        _requestedMeat = Random.Range(customer._MinMeat, customer._MaxMeat);

        _customerCurrentMeat = _counter.MinusMeat(_requestedMeat);
    }

    public void Update(CustomerAI customer)
    {
        if (_customerCurrentMeat != _requestedMeat)
        {
            _customerCurrentMeat = _counter.MinusMeat(_requestedMeat);
        }
        else
        {
            customer.SetState(new CustomerMoveToTable());
        }
    }

    public void Exit(CustomerAI customer)
    {

    }
}
