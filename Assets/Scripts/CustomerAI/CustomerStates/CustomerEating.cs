using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerEating : ICustomerState
{
    float _eatTimer = 0f;       // 시간 누적용
    float _eatDelay = 5f;     // 고기 하나 먹는 간격
    Table _table;               // 손님이 앉아있는 테이블 참조

    public void Enter(CustomerAI customer)
    {
        _eatTimer = 0f;
        _table = customer._table;

        // 1. 손님 정면 고기 제거
        customer.ClearAllMeatVisuals();

        // 2. 테이블 위에 고기 생성
        _table.AddMeat(customer._CurrentMeat);

        // 3. 가장 가까운 의자 위치로 이동
        Transform closestChair = GetClosestChair(_table.transform, customer.transform.position);
        if (closestChair != null)
        {
            customer.transform.position = closestChair.position;
            customer.transform.rotation = closestChair.rotation; // 의자 방향도 맞춰줌
        }

        customer.SetExclusiveAnimation("IsSittingAndEat");
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
        if (customer._currentChairNode != null)
        {
            customer._currentChairNode._isWalkale = true;  // 노드 되돌리기
            customer._currentChairNode = null;              // 안전하게 초기화
        }
    }

    /// <summary>
    /// Table 아래 자식 Chair들 중에서 손님에게 가장 가까운 것을 찾음
    /// </summary>
    Transform GetClosestChair(Transform tableTransform, Vector3 customerPos)
    {
        Transform closestChair = null;
        float minDistance = float.MaxValue;

        foreach (Transform child in tableTransform)
        {
            if (child.CompareTag("Chair"))  // Chair로 태그 구분
            {
                float dist = Vector3.Distance(customerPos, child.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestChair = child;
                }
            }
        }

        if (closestChair == null)
            Debug.LogWarning("테이블에 Chair 태그를 가진 자식이 없습니다!");

        return closestChair;
    }
}
