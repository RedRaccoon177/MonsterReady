using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region �̱��� �� �̺�Ʈ
    public static PlayerController _instance { get; private set; }
    public static event Action<int> OnGoldChanged;
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

    public Vector3 _playerPos;              // �÷��̾� ��ġ
    public int _playerGold = 0;             // �÷��̾� ���
    public int _playergem = 2;              // �÷��̾� ����
    public int _playerPassLevel = 1;        // ��Ʋ �н� ����
    public int _playerSpeedLevel = 1;       // �̵� �ӵ� ����
    public int _playerHoldMaxLevel = 1;     // ��� �뷮 ����
    public int _playerMakeMoneyLevel = 1;   // ���ͷ� ����
    #endregion

    #region �������� ������Ƽ
    public int _Gold 
    {
        get => _playerGold;
        set 
        {
            if (value < 0) _playerGold = 0;
            else _playerGold = value;
            OnGoldChanged?.Invoke(_playerGold);
        }
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
    /// <returns>���� ����</returns>
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

}
