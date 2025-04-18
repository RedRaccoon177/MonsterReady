using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region 싱글톤 및 이벤트
    public static PlayerController _instance { get; private set; }  // 플레이어 싱글톤
    public static event Action OnJoystickReleased;                  //조이 스틱 이벤트
    public static event Action<int> OnGoldChanged;                  // 골드 변화 이벤트
    #endregion

    #region 플레이어 변수들
    // 플레이어가 들 수 있는 최대 아이템 개수
    int _holdMaxAmount;

    // 수익을 저장하는 변수
    float _revenue;

    // 조이스틱 입력 참조용 (FloatingJoystick 연결)
    public FloatingJoystick _joy;

    // 이동 속도 조절 변수
    public float _moveSpeed;

    // Rigidbody 컴포넌트 참조
    Rigidbody _rg;

    // 이동할 방향 벡터
    Vector3 _moveVec;

    Vector3 _playerPos;              // 플레이어 위치
    int _playerGold = 0;             // 플레이어 골드
    int _playerGem = 0;              // 플레이어 보석
    int _playerPassLevel = 1;        // 배틀 패스 레벨
    int _playerSpeedLevel = 1;       // 이동 속도 레벨
    int _playerHoldMaxLevel = 1;     // 드는 용량 레벨
    int _playerMakeMoneyLevel = 1;   // 수익률 레벨
    #endregion

    #region 변수들 프로퍼티
    public Vector3 _MoveVec 
    {
        get => _moveVec;
        set => _moveVec = value;
    }

    public Vector3 _PlayerPos
    {
        get => _playerPos;
        set => _playerPos = value;
    }

    public int _Gold
    {
        get => _playerGold;
        set
        {
            _playerGold = Mathf.Max(0, value);
            OnGoldChanged?.Invoke(_playerGold); // 옵저버 패턴 이벤트 호출
        }
    }

    public int _Gem
    {
        get => _playerGem;
        set => _playerGem = Mathf.Max(0, value);
        //TODO: 추후 잼도 옵저버 패턴으로 이벤트 호출해야함
    }

    public int _PassLevel
    {
        get => _playerPassLevel;
        set => _playerPassLevel = Mathf.Max(1, value);
    }

    public int _SpeedLevel
    {
        get => _playerSpeedLevel;
        set => _playerSpeedLevel = Mathf.Max(1, value);
    }

    public int _HoldMaxLevel
    {
        get => _playerHoldMaxLevel;
        set => _playerHoldMaxLevel = Mathf.Max(1, value);
    }

    public int _MakeMoneyLevel
    {
        get => _playerMakeMoneyLevel;
        set => _playerMakeMoneyLevel = Mathf.Max(1, value);
    }
    #endregion

    #region Awake, FixedUpdata, LateUpdate
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

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

    #region 플레이어 움직임 회전 함수
    void RotateToMoveDirection()
    {
        if (_moveVec.sqrMagnitude < 0.0001f) return; // 방향 벡터가 거의 0이라면 회전하지 않음

        // 이동 방향으로 회전
        Quaternion _dirQuat = Quaternion.LookRotation(_moveVec);
        Quaternion _moveQuat = Quaternion.Slerp(_rg.rotation, _dirQuat, 0.3f);
        _rg.MoveRotation(_moveQuat);
    }
    #endregion

    #region 골드 관련 메서드
    /// <summary>
    /// 골드 증가
    /// </summary>
    /// <param name="amount">증가량</param>
    public int AddGold(int amount)
    {
        _Gold += amount;
        return 0;
    }

    /// <summary>
    /// 골드 감소. 감소 값이 현재 값보다 크면 실패
    /// </summary>
    /// <param name="amount">사용할 골드</param>
    /// <returns> 0 혹은 (변수 값 = 변수 값 - _gold) </returns>
    public int SpendGold(int amount)
    {
        if (_Gold >= amount)
        {
            _Gold -= amount;

        }
        else if(_Gold < amount)
        {
            _Gold = 0;
            amount -= _Gold;
            return amount;
        }
        return 0;
    }
    #endregion

    public static void InvokeJoystickReleased()
    {
        OnJoystickReleased?.Invoke();
    }
}
