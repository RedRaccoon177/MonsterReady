using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region �̱��� �� �̺�Ʈ
    public static PlayerController _instance { get; private set; }  // �÷��̾� �̱���
    public static event Action OnJoystickReleased;                  //���� ��ƽ �̺�Ʈ
    public static event Action<int> OnGoldChanged;                  // ��� ��ȭ �̺�Ʈ
    #endregion

    #region �÷��̾� ������
    // �÷��̾ �� �� �ִ� �ִ� ������ ����
    int _holdMaxAmount;

    // ������ �����ϴ� ����
    float _revenue;

    // ���̽�ƽ �Է� ������ (FloatingJoystick ����)
    public FloatingJoystick _joy;

    // �̵� �ӵ� ���� ����
    public float _moveSpeed;

    // Rigidbody ������Ʈ ����
    Rigidbody _rg;

    // �̵��� ���� ����
    Vector3 _moveVec;

    Vector3 _playerPos;              // �÷��̾� ��ġ
    int _playerGold = 0;             // �÷��̾� ���
    int _playerGem = 0;              // �÷��̾� ����
    int _playerPassLevel = 1;        // ��Ʋ �н� ����
    int _playerSpeedLevel = 1;       // �̵� �ӵ� ����
    int _playerHoldMaxLevel = 1;     // ��� �뷮 ����
    int _playerMakeMoneyLevel = 1;   // ���ͷ� ����
    #endregion

    #region ������ ������Ƽ
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
            OnGoldChanged?.Invoke(_playerGold); // ������ ���� �̺�Ʈ ȣ��
        }
    }

    public int _Gem
    {
        get => _playerGem;
        set => _playerGem = Mathf.Max(0, value);
        //TODO: ���� �뵵 ������ �������� �̺�Ʈ ȣ���ؾ���
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
        // ���̽�ƽ �Է°� �޾ƿ�
        float x = _joy.Horizontal;
        float z = _joy.Vertical;

        // �̵� ���� ����
        if (x == 0 && z == 0) return;
        _moveVec = new Vector3(x, 0, z) * _moveSpeed * Time.deltaTime;

        // �÷��̾� �̵�
        _rg.MovePosition(_rg.position + _moveVec);

        // ȸ�� ó��
        RotateToMoveDirection();
    }

    void LateUpdate()
    {
        //TODO : �ִϸ��̼� ���� �߰��� ����.
    }
    #endregion

    #region �÷��̾� ������ ȸ�� �Լ�
    void RotateToMoveDirection()
    {
        if (_moveVec.sqrMagnitude < 0.0001f) return; // ���� ���Ͱ� ���� 0�̶�� ȸ������ ����

        // �̵� �������� ȸ��
        Quaternion _dirQuat = Quaternion.LookRotation(_moveVec);
        Quaternion _moveQuat = Quaternion.Slerp(_rg.rotation, _dirQuat, 0.3f);
        _rg.MoveRotation(_moveQuat);
    }
    #endregion

    #region ��� ���� �޼���
    /// <summary>
    /// ��� ����
    /// </summary>
    /// <param name="amount">������</param>
    public int AddGold(int amount)
    {
        _Gold += amount;
        return 0;
    }

    /// <summary>
    /// ��� ����. ���� ���� ���� ������ ũ�� ����
    /// </summary>
    /// <param name="amount">����� ���</param>
    /// <returns> 0 Ȥ�� (���� �� = ���� �� - _gold) </returns>
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
