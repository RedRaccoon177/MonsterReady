using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CustomerOrderAndWait : ICustomerState
{
    //ī������ ����
    Counter _counter = GameManager._instance._counters[0];

    // �䱸�� ��� ��
    int _requestedMeat;

    public void Enter(CustomerAI customer)
    {
        // �մ��� ���ϴ� ��� ����
        _requestedMeat = Random.Range(customer._MinMeat, customer._MaxMeat);

        customer._CurrentMeat = _counter.MinusMeat(_requestedMeat);
        
        Debug.Log("��� �䱸");
    }

    public void Update(CustomerAI customer)
    {
        if (customer._CurrentMeat != _requestedMeat)
        {
            customer._CurrentMeat = _counter.MinusMeat(_requestedMeat);
            Debug.Log("��� �䱸");
        }
        else
        {
            customer.SetState(new CustomerMoveToTable());
        }
    }

    public void Exit(CustomerAI customer) { }
}
