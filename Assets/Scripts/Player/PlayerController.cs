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

    [Header("�÷��̾��� ���")]
    [SerializeField] int _maxMeat;              //���� ��� �ִ� ��� �ִ� ��
    [SerializeField] int _currentMeat;          //���� ��� �ִ� ��� ��
    List<GameObject> _meatList = new List<GameObject>();    //������ ��� ������Ʈ�� ��� ����Ʈ

    [Header("��� ������")]
    [SerializeField] GameObject _meatPrefab;

    [Header("������Ʈ Ǯ�� ����")]
    [SerializeField] ObjectPooling _meatPool; // ��⸦ �����ϴ� ������Ʈ Ǯ

    [Header("��� ���� ���� ����")]
    [SerializeField] float _stackHeight = 0.11f;

    [Header("��� ��ġ�ϴ� ��")]
    [SerializeField] Transform _meatSpawnLocation;
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

    public int _MaxMeat
    {
        get => _maxMeat;
        set => _maxMeat = value;
    }

    public int _CurrentMeat
    {
        get => _currentMeat;
        set
        {
            _currentMeat = Mathf.Clamp(0, value, _MaxMeat);
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

        _MaxMeat = 4;
        _CurrentMeat = 0;
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
    /// <param name="gold">������</param>
    public int AddGold(int gold)
    {
        _Gold += gold;
        return 0;
    }

    /// <summary>
    /// ��� ����
    /// </summary>
    /// <param name="gold">����� ���</param>
    /// <returns> 0 Ȥ�� (���� �� = ���� �� - _gold) </returns>
    public int MinusGold(int gold)
    {
        if (_Gold >= gold)
        {
            _Gold -= gold;

        }
        else if(_Gold < gold)
        {
            _Gold = 0;
            gold -= _Gold;
            return gold;
        }
        return 0;
    }
    #endregion

    #region ��� ���� �޼���
    /// <summary>
    /// ��� ����. ��ĥ ���, ��ġ�� ���� ��ȯ
    /// </summary>
    public int AddMeat(int meat)
    {
        int spaceLeft = _MaxMeat - _currentMeat;
        int toAdd = Mathf.Min(spaceLeft, meat);

        _currentMeat += toAdd;

        UpdateMeatDisplay(_currentMeat);
        return meat - toAdd; // ��ģ ��
    }

    /// <summary>
    /// ��� ����
    /// </summary>
    public int MinusMeat(int amount)
    {
        int removed = Mathf.Min(_currentMeat, amount);
        _currentMeat -= removed;

        UpdateMeatDisplay(_currentMeat);
        return removed;
    }

    /// <summary>
    /// ��� �ð�ȭ �Լ�
    /// </summary>
    /// <param name="currentMeat"></param>
    public void UpdateMeatDisplay(int currentMeat)
    {
        // 1. ��� ������ �����ϸ� ä����
        while (_meatList.Count < currentMeat)
        {
            GameObject meat = _meatPool.GetMeat(); // ������Ʈ Ǯ���� ����
            meat.transform.SetParent(_meatSpawnLocation, false);
            // ���� ��ġ��, ȸ����, ũ�Ⱚ
            meat.transform.localPosition = GetStackPosition(_meatList.Count);
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }

        // 2. ��� ������ �ʰ��Ǹ� ���� (���������� �ϳ���)
        while (_meatList.Count > currentMeat)
        {
            GameObject lastMeat = _meatList[_meatList.Count - 1];
            _meatList.RemoveAt(_meatList.Count - 1);
            _meatPool.ReturnToPool(lastMeat);
        }
    }
    Vector3 GetStackPosition(int index)
    {
        return new Vector3(0, index * _stackHeight, 0);
    }
    #endregion

    #region ���̽�ƽ �̺�Ʈ �Լ�
    public static void InvokeJoystickReleased()
    {
        OnJoystickReleased?.Invoke();
    }
    #endregion
}
