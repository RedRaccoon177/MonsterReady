using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderAndWait : ICustomerState
{
    Counter _counter = GameManager._instance._counters[0];
    int _requestedMeat;

    public void Enter(CustomerAI customer)
    {
        _requestedMeat = Random.Range(customer._MinMeat, customer._MaxMeat);
        customer._CurrentMeat = 0; // 수령 초기화
        UiManager._instance.OnUi(UiType.MeatOrderUi);
        customer.SetExclusiveAnimation("IsCarrying");
    }

    public void Update(CustomerAI customer)
    {
        WaitForGetMeat(customer);
    }

    public void Exit(CustomerAI customer) { }

    #region 고기 받는 함수
    public void WaitForGetMeat(CustomerAI customer)
    {
        int beforeMeat = customer._CurrentMeat;
        int neededMeat = _requestedMeat - beforeMeat;

        if (neededMeat > 0 && _counter._objectInteration._IsInteration)
        {
            int receivedMeat = _counter.MinusMeat(neededMeat);

            if (receivedMeat > 0)
            {
                customer.AddMeat(receivedMeat);                         // 고기 오브젝트 생성
                customer.UpdateMeatDisplay(customer._CurrentMeat);      // 시각 동기화
            }
        }

        if (customer._CurrentMeat >= _requestedMeat)
        {
            //UiManager._instance.ActiveMeatOrdreUi(_requestedMeat,false);
            customer.SetState(new CustomerMoveToTable());
        }
    }
    #endregion
}

