using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : InteractionObject
{
    #region ������
    [Header("������Ʈ Ǯ�� ����")]
    [SerializeField] ObjectPooling _meatPool; // ��⸦ �����ϴ� ������Ʈ Ǯ

    [Header("��� ������")]
    [SerializeField] GameObject _meatPrefab;

    [Header("��� ��ġ�ϴ� ��")]
    [SerializeField] Transform _meatSpawnLocation;

    [Header("�ִ�ġ ��� ����")]
    [SerializeField] int _maxMeatCount = 5;

    [Header("���� ������� ���")]
    [SerializeField] int _currentMeatCount = 0;

    [Header("��� ���� ��Ÿ�� (��)")]
    [SerializeField] float _cooltimeMeatGrill = 3f;

    [Header("��� ���� ���� ����")]
    [SerializeField] float _stackHeight = 0.11f;

    // ������ ��� ������Ʈ���� ��� ����Ʈ
    List<GameObject> _meatList = new List<GameObject>();

    //��� ���°� ��� �ڷ�ƾ
    Coroutine _grillRoutine;

    //�÷��̾� ����
    PlayerController _player;
    #endregion

    #region Start, OnTriggerEnter
    void Start()
    {
        _player = PlayerController._instance;
        // ���� ���� �� ��� �ڵ� ���� ����
        StartGrill();
    }

    // �÷��̾ ������ ������ �� ��� �ڵ� ����
    private void OnTriggerEnter(Collider other)
    {
        // �±װ� Player�� �ƴ� ��� ����
        if (!other.CompareTag("Player")) return;


        //�÷��̾��� ������ �������� ������ ��� ��
        if (other.CompareTag("Player"))
        {
            if (_player._MaxMeat != _player._CurrentMeat)
            {
                int _minusMeat = _player._MaxMeat - _player._CurrentMeat;
                _player.AddMeat(MinusMeat(_minusMeat));
            }
        }
        else if (other.CompareTag("NPC"))
        {
            //TODO: NPC ĳ���͵� ��� ȹ��
        }
    }
    #endregion

    #region ��� ���� �ڷ�ƾ
    // ��� ���� ����
    public void StartGrill()
    {
        // �ߺ� ���� ����
        if (_grillRoutine == null)
        {
            _grillRoutine = StartCoroutine(GrillLoop());
        }
    }
    
    // ��� ���� ����
    IEnumerator GrillLoop()
    {
        while (true)
        {
            // ��� ���� �ð� ���
            yield return new WaitForSeconds(_cooltimeMeatGrill);

            // �ִ�ġ�� �����ϸ� ��� (���� �ߴ�)
            if (_currentMeatCount >= _maxMeatCount)
            {
                yield return null;
                continue;   //�Ʒ� �ڵ� �����ϰ� �ٽ� whileó������ �̵�
            }

            // ��� ����
            AddMeat(1);
        }
    }
    #endregion

    #region ��� ���� �� ����
    /// <summary>
    /// ��� ����
    /// </summary>
    /// <param name="_meatCount"></param>
    void AddMeat(int _meatCount)
    {
        _currentMeatCount = Mathf.Clamp(_currentMeatCount + _meatCount, 0, _maxMeatCount);
        UpdateMeatDisplay(_currentMeatCount);
    }

    /// <summary>
    /// ��� ����
    /// </summary>
    /// <param name="_minusMeat"></param>
    int MinusMeat(int _minusMeat)
    {
        int _playerGetMeat = _currentMeatCount;

        if (_currentMeatCount < _minusMeat)
            _currentMeatCount = 0;
        else if (_currentMeatCount >= _minusMeat)
            _currentMeatCount -= _minusMeat;

        UpdateMeatDisplay(_currentMeatCount);
        return _playerGetMeat;
    }
    #endregion

    #region ��� �ð�ȭ �ϴ� �ڵ� ����
    //��� �ð�ȭ �ϴ� �ڵ�
    public void UpdateMeatDisplay(int currentMeat)
    {
        // 1. ��� ������ �����ϸ� ä����
        while (_meatList.Count < currentMeat)
        {
            GameObject meat = _meatPool.GetMeat(); // ������Ʈ Ǯ���� ����

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
        return new Vector3
        (
            _meatSpawnLocation.position.x,
            _meatSpawnLocation.position.y + index * _stackHeight,
            _meatSpawnLocation.position.z
        );
    }
    #endregion
}