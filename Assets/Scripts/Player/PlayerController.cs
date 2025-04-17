using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    // 현재 프레임에서 이동할 방향 벡터
    Vector3 _moveVec;

    public Vector3 _playerPos; // 플레이어 위치
    public int _playerGold =1; // 플레이어 골드
    public int _playergem = 2; // 플레이어 보석
    public int _playerPassLevel = 1; // 배틀 패스 레벨
    public int _playerSpeedLevel = 1; // 이동 속도 레벨
    public int _playerHoldMaxLevel = 1; // 드는 용량 레벨
    public int _playerMakeMoneyLevel = 1; // 수익률 레벨



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

    //회전 함수
    void RotateToMoveDirection()
    {
        // 이동 방향으로 회전
        Quaternion _dirQuat = Quaternion.LookRotation(_moveVec);
        Quaternion _moveQuat = Quaternion.Slerp(_rg.rotation, _dirQuat, 0.3f);
        _rg.MoveRotation(_moveQuat);
    }


    void LateUpdate()
    {
        //TODO : 애니메이션 추후 추가할 예정.
    }
}
