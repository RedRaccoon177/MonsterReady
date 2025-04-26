using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerEating : ICustomerState
{
    float _eatTimer = 0f;       // 시간 누적용
    float _eatDelay = 0.5f;     // 고기 하나 먹는 간격
    Table _table;               // 손님이 앉아있는 테이블 참조

    public void Enter(CustomerAI customer)
    {
        _eatTimer = 0f;
        _table = customer._table;  // 손님이 어느 테이블에 앉아 있는지 가져오기 (CustomerAI에 _table 변수 추가 필요)
    }

    public void Update(CustomerAI customer)
    {
        if (customer._CurrentMeat <= 0)
        {
            // 다 먹었으면 뼈를 남기고 다음 상태로 전환
            _table.AddBones(customer._AteMeatCount); // 먹은 고기 수만큼 뼈 남김
            customer.SetState(new CustomerGoingHome()); // 다음 상태로 (예: 퇴장)
            return;
        }

        _eatTimer += Time.deltaTime;

        if (_eatTimer >= _eatDelay)
        {
            _eatTimer = 0f;

            // 고기 하나 먹음
            customer.EatOneMeat();

            // 테이블 위 고기도 하나 제거
            _table.RemoveMeat();
        }
    }

    public void Exit(CustomerAI customer)
    {

    }
}
