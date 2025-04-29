using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class CustomerOrderAndWait : ICustomerState
{
    Counter _counter = GameManager._instance._counters[0];
    int _requestedMeat;

    public void Enter(CustomerAI customer)
    {
        _requestedMeat = Random.Range(customer._MinMeat, customer._MaxMeat);
        customer._CurrentMeat = 0; // ���� �ʱ�ȭ

        Debug.Log($"[CustomerOrderAndWait] ��� {_requestedMeat}�� �䱸");
    }

    public void Update(CustomerAI customer)
    {
        int beforeMeat = customer._CurrentMeat;
        int neededMeat = _requestedMeat - beforeMeat;

        if (neededMeat > 0)
        {
            int receivedMeat = _counter.MinusMeat(neededMeat);

            if (receivedMeat > 0)
            {
                customer.AddMeat(receivedMeat);                         // ��� ������Ʈ ����
                customer.UpdateMeatDisplay(customer._CurrentMeat);      // �ð� ����ȭ
            }
        }

        if (customer._CurrentMeat >= _requestedMeat)
        {
            customer.SetState(new CustomerMoveToTable());
        }
    }

    public void Exit(CustomerAI customer) { }
}

