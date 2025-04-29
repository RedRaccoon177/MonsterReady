using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public interface ICustomerState
{
    void Enter(CustomerAI customer);
    void Update(CustomerAI customer);
    void Exit(CustomerAI customer);
}

public class CustomerAI : MonoBehaviour
{
    ICustomerState _currentState;

    #region �մ� �ʵ�
    public Table _table;         // ���� �ɾ��ִ� ���̺�
    public int _AteMeatCount = 0; // ���� ��� �� ����

    [Header("������Ʈ Ǯ�� ����")]
    [SerializeField] ObjectPooling _meatPool;

    [Header("���, �� ������")]
    [SerializeField] GameObject _meatPrefab;              // ��� ������

    [Header("���, �� ��ġ ��ġ �� �ױ� ����")]
    [SerializeField] Transform _meatSpawnLocation;        // ���/�� ���� ���� ��ġ

    List<GameObject> _meatList = new List<GameObject>();  // ���� ��� ������Ʈ ����Ʈ

    [Header("��� ��ġ ��ġ �� �ױ� ����")]
    [SerializeField] float _stackHeight = 0.11f;          // ���̴� ���� ����

    //�մ��� �䱸�ϴ� �ּ�ġ ���
    [SerializeField] int _minMeat;

    //�մ��� �ִ�ġ ��⸦ �޴� ��
    [SerializeField] int _maxMeat;
    
    //�մ��� ���� ���� ��� ��
    [SerializeField] int _currentMeat;
    #endregion

    #region ���� ������Ƽ
    public int _MaxMeat => _maxMeat;
    public int _MinMeat => _minMeat;
    public int _CurrentMeat 
    {
        get => _currentMeat;
        set => _currentMeat = value;
    }
    #endregion

    void Awake()
    {
        // ObjectPooling �ڵ� ����
        if (_meatPool == null)
        {
            _meatPool = FindObjectOfType<ObjectPooling>();

            if (_meatPool == null)
            {
                Debug.LogError("[CustomerAI] ���� ObjectPooling ������Ʈ�� �����ϴ�!");
            }
        }
    }


    #region Start, Update
    public void Start()
    {
        SetState(new CustomerMoveToCounterState());
    }

    void Update()
    {
        _currentState?.Update(this);
    }
    #endregion

    #region �������� ���� �Լ�
    public void SetState(ICustomerState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }
    #endregion

    #region ��� ���� �Լ�
    /// <summary>
    /// ��� �ϳ� �Ա�
    /// </summary>
    public void EatOneMeat()
    {
        if (_currentMeat > 0)
        {
            _currentMeat--;
            _AteMeatCount++;
        }
    }

    /// <summary>
    /// ���̺� ��� �߰�
    /// </summary>
    public void AddMeat(int amount)
    {
        _CurrentMeat += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject meat = _meatPool.GetMeat();                      // ��� Ǯ���� ������
            meat.transform.SetParent(_meatSpawnLocation);
            meat.transform.localPosition = GetStackMeatAndBonePosition(_meatList.Count); // ���� ��� ����ŭ ���� ����
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }
    }

    public void UpdateMeatDisplay(int currentMeat)
    {
        // 1. ��� ������ �����ϸ� ä����
        while (_meatList.Count < currentMeat)
        {
            GameObject meat = _meatPool.GetMeat();

            // �θ� ����
            meat.transform.SetParent(transform);  // �մ� ������Ʈ�� ����ٴϰ�
            meat.transform.localPosition = GetStackMeatAndBonePosition(_meatList.Count);
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }

        // 2. ��� ������ �ʰ��Ǹ� ����
        while (_meatList.Count > currentMeat)
        {
            GameObject lastMeat = _meatList[_meatList.Count - 1];
            _meatList.RemoveAt(_meatList.Count - 1);
            _meatPool.ReturnToPool(lastMeat);
        }
    }

    /// <summary>
    /// �մ� ���� ���鿡 ��� �״� ��ġ ���
    /// </summary>
    Vector3 GetStackMeatAndBonePosition(int index)
    {
        float baseHeight = 1f; // �մ� ��ġ���� ����
        float forwardOffset = 0.5f; // �մ� ����
        return new Vector3(
            0f,
            baseHeight + index * _stackHeight,
            forwardOffset
        );
    }

    /// <summary>
    /// �մ� �տ� ���̴� ��� ��� �ð�ȭ ����
    /// </summary>
    public void ClearAllMeatVisuals()
    {
        foreach (var meat in _meatList)
        {
            _meatPool.ReturnToPool(meat);
        }
        _meatList.Clear();
    }
    #endregion
}