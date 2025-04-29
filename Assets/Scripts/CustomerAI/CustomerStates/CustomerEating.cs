using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerEating : ICustomerState
{
    float _eatTimer = 0f;       // �ð� ������
    float _eatDelay = 5f;     // ��� �ϳ� �Դ� ����
    Table _table;               // �մ��� �ɾ��ִ� ���̺� ����

    public void Enter(CustomerAI customer)
    {
        _eatTimer = 0f;
        _table = customer._table;

        // 1. �մ� ���� ��� ����
        customer.ClearAllMeatVisuals();

        // 2. ���̺� ���� ��� ����
        _table.AddMeat(customer._CurrentMeat);
    }


    public void Update(CustomerAI customer)
    {
        if (customer._CurrentMeat <= 0)
        {
            // �� �Ծ����� ���� ����� ���� ���·� ��ȯ
            _table.AddBones(customer._AteMeatCount); // ���� ��� ����ŭ �� ����
            customer.SetState(new CustomerGoingHome()); // ���� ���·� (��: ����)
            return;
        }

        _eatTimer += Time.deltaTime;

        if (_eatTimer >= _eatDelay)
        {
            _eatTimer = 0f;

            // ��� �ϳ� ����
            customer.EatOneMeat();

            // ���̺� �� ��⵵ �ϳ� ����
            _table.RemoveMeat();
        }
    }

    public void Exit(CustomerAI customer) { }
}
