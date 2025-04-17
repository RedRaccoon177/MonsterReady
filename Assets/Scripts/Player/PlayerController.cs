using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region 플레이어 변수들
    // 플레이어가 들 수 있는 최대 아이템 개수
    int _holdMaxAmount;

    // 수익을 저장하는 변수
    float _revenue;

    // 골드
    public int _gold = 0;
    int _gem;

    // 조이스틱 입력 참조용 (FloatingJoystick 연결)
    public FloatingJoystick _joy;

    // 이동 속도 조절 변수
    public float _moveSpeed;

    // Rigidbody 컴포넌트 참조
    Rigidbody _rg;

    // 이동할 방향 벡터
    Vector3 _moveVec;
    #endregion

    #region 변수들의 프로퍼티
    public int _Gold 
    {
        get => _gold;
        set 
        {
            if (value < 0) _gold = 0;
            else _gold = value;
        }
    }
    #endregion

    #region Awake, FixedUpdata, LateUpdate
    void Awake()
    {
        _rg = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // 조이스틱 입력값 받아옴
        float x = _joy.Horizontal;
        float z = _joy.Vertical;

        // 이동 벡터 생성
        if (x == 0 && z == 0) return;
        _moveVec = new Vector3(x, 0, z) * _moveSpeed * Time.deltaTime;

        // 플레이어 이동
        _rg.MovePosition(_rg.position + _moveVec);

        // 회전 처리
        RotateToMoveDirection();
    }
    void LateUpdate()
    {
        //TODO : 애니메이션 추후 추가할 예정.
    }
    #endregion

    //회전 함수
    void RotateToMoveDirection()
    {
        if (_moveVec.sqrMagnitude < 0.0001f) return; // 방향 벡터가 거의 0이라면 회전하지 않음

        // 이동 방향으로 회전
        Quaternion _dirQuat = Quaternion.LookRotation(_moveVec);
        Quaternion _moveQuat = Quaternion.Slerp(_rg.rotation, _dirQuat, 0.3f);
        _rg.MoveRotation(_moveQuat);
    }

    #region 골드 관련 메서드
    /// <summary>
    /// 골드 증가
    /// </summary>
    /// <param name="amount">증가량</param>
    public int AddGold(int amount)
    {
        _Gold += amount;
        Debug.Log($"[골드 획득] 현재 골드: {_Gold}");
        return 0;
    }

    /// <summary>
    /// 골드 감소. 감소 값이 현재 값보다 크면 실패
    /// </summary>
    /// <param name="amount">사용할 골드</param>
    /// <returns>성공 여부</returns>
    public int SpendGold(int amount)
    {
        if (_Gold >= amount)
        {
            _Gold -= amount;
            Debug.Log($"[골드 사용] {amount} 사용됨. 남은 골드: {_Gold}");
        }
        else if(_Gold < amount)
        {
            _Gold = 0;
            amount -= _Gold;
            Debug.Log($"[골드 사용] {amount} 사용됨. 남은 골드: {_Gold}");
            return amount;
        }
        return 0;
    }
    #endregion

}
