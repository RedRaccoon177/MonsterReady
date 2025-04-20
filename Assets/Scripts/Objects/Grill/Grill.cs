using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : InteractionObject
{
    #region ������
    [Header("��� ������")]
    [SerializeField] GameObject _meat;

    [Header("��� ��ġ�ϴ� ��")]
    [SerializeField] Transform _locationOfMeat;

    [Header("�ִ�ġ ��� ����")]
    [SerializeField] int _maxMeatCount = 20;

    [Header("���� ������� ���")]
    [SerializeField] int _currentMeatCount = 0;

    [Header("��� ���� ��Ÿ�� (��)")]
    [SerializeField] float _cooltimeMeatGrill = 3f;

    [Header("��� ���� ���� ����")]
    [SerializeField] float _stackHeight = 0.1f;

    // ������ ��� ������Ʈ���� ��� ����Ʈ (�ð��� ������)
    List<GameObject> _meatList = new List<GameObject>();

    //��� ���°� ��� �ڷ�ƾ
    Coroutine _grillRoutine;
    #endregion

    #region Start, Update ��
    void Start()
    {
        // ���� ���� �� ��� �ڵ� ���� ����
        StartGrill();
    }

    void Update()
    {
        // ����׿� : ���콺 ��Ŭ������ ��� �ϳ� ����
        if (Input.GetMouseButtonDown(0))
        {
            TakeMeat();
        }
    }
    #endregion

    // ��� ���� ����
    public void StartGrill()
    {
        // �ߺ� ���� ����
        if (_grillRoutine == null)
            _grillRoutine = StartCoroutine(GrillLoop());
    }

    // ��� ���� ����
    IEnumerator GrillLoop()
    {
        while (true)
        {
            // �ִ�ġ�� �����ϸ� ��� (���� �ߴ�)
            if (_currentMeatCount >= _maxMeatCount)
            {
                yield return null;
                continue;
            }

            // ��� ���� �ð� ���
            yield return new WaitForSeconds(_cooltimeMeatGrill);

            // ��� ����
            SpawnMeat();
        }
    }

    // ��� ���� �� ��ġ ����
    void SpawnMeat()
    {
        // ���̴� ��ġ ��� (���� ��� ���� �������� ���� �ø�)
        Vector3 spawnPos = _locationOfMeat.position + Vector3.up * _stackHeight * _currentMeatCount;

        // ��� ���� �� �θ� ����
        GameObject meat = Instantiate(_meat, spawnPos, Quaternion.identity, _locationOfMeat);
        _meatList.Add(meat); // ����Ʈ�� �߰�

        _currentMeatCount++;
        Debug.Log($"��� ������ ({_currentMeatCount}/{_maxMeatCount})");
    }

    // �ܺο��� ��⸦ �ϳ� ������ �� ȣ��
    public bool TakeMeat()
    {
        // ��Ⱑ ������ ����
        if (_currentMeatCount <= 0 || _meatList.Count == 0) return false;

        // ������ ��� ����
        GameObject lastMeat = _meatList[_meatList.Count - 1];
        _meatList.RemoveAt(_meatList.Count - 1);
        Destroy(lastMeat); // �ð������� ����

        _currentMeatCount--;
        return true;
    }

    // �÷��̾ ������ ������ �� ��� �ڵ� ����
    private void OnTriggerEnter(Collider other)
    {
        // �±װ� Player�� �ƴ� ��� ����
        if (!other.CompareTag("Player")) return;

        // �÷��̾� ������Ʈ ��������
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        // �÷��̾ �ʿ��� ��� ���� ���
        int playerNeed = player._MaxMeat - player._CurrentMeat;
        if (playerNeed <= 0) return;

        // �� �� �ִ� ��� ���� ����
        int giveMeatCount = Mathf.Min(_currentMeatCount, playerNeed);

        // �ݺ��ϸ� ��� ����
        for (int i = 0; i < giveMeatCount; i++)
        {
            if (TakeMeat())
            {
                player.AddMeat(1); // �÷��̾ ��� �߰�
            }
        }

        Debug.Log($"�÷��̾ ��� {giveMeatCount}�� ������ (���� ����: {player._CurrentMeat}/{player._MaxMeat})");
    }
}