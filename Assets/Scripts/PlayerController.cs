using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

    // ���� �����ӿ��� �̵��� ���� ����
    Vector3 _moveVec;

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

    //ȸ�� �Լ�
    void RotateToMoveDirection()
    {
        // �̵� �������� ȸ��
        Quaternion _dirQuat = Quaternion.LookRotation(_moveVec);
        Quaternion _moveQuat = Quaternion.Slerp(_rg.rotation, _dirQuat, 0.3f);
        _rg.MoveRotation(_moveQuat);
    }


    void LateUpdate()
    {
        //TODO : �ִϸ��̼� ���� �߰��� ����.
    }
}
