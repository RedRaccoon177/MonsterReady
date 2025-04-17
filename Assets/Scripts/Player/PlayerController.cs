using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region �÷��̾� ������
    // �÷��̾ �� �� �ִ� �ִ� ������ ����
    int _holdMaxAmount;

    // ������ �����ϴ� ����
    float _revenue;

    // ���
    public int _gold = 0;
    int _gem;

    // ���̽�ƽ �Է� ������ (FloatingJoystick ����)
    public FloatingJoystick _joy;

    // �̵� �ӵ� ���� ����
    public float _moveSpeed;

    // Rigidbody ������Ʈ ����
    Rigidbody _rg;

    // �̵��� ���� ����
    Vector3 _moveVec;
    #endregion

    #region �������� ������Ƽ
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

    //ȸ�� �Լ�
    void RotateToMoveDirection()
    {
        if (_moveVec.sqrMagnitude < 0.0001f) return; // ���� ���Ͱ� ���� 0�̶�� ȸ������ ����

        // �̵� �������� ȸ��
        Quaternion _dirQuat = Quaternion.LookRotation(_moveVec);
        Quaternion _moveQuat = Quaternion.Slerp(_rg.rotation, _dirQuat, 0.3f);
        _rg.MoveRotation(_moveQuat);
    }

    #region ��� ���� �޼���
    /// <summary>
    /// ��� ����
    /// </summary>
    /// <param name="amount">������</param>
    public int AddGold(int amount)
    {
        _Gold += amount;
        Debug.Log($"[��� ȹ��] ���� ���: {_Gold}");
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
            Debug.Log($"[��� ���] {amount} ����. ���� ���: {_Gold}");
        }
        else if(_Gold < amount)
        {
            _Gold = 0;
            amount -= _Gold;
            Debug.Log($"[��� ���] {amount} ����. ���� ���: {_Gold}");
            return amount;
        }
        return 0;
    }
    #endregion

}
