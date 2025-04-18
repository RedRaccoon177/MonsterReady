using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : InteractionObject
{
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

    //��� ���°� ��� �ڷ�ƾ
    Coroutine _grillRoutine;

    void Start()
    {
        StartGrill();
    }

    void Update()
    {
        if( Input.GetMouseButtonDown(0))
        {
            TakeMeat();
        }
    }

    public void StartGrill()
    {
        if (_grillRoutine == null)
            _grillRoutine = StartCoroutine(GrillLoop());
    }

    IEnumerator GrillLoop()
    {
        while (true)
        {
            if (_currentMeatCount >= _maxMeatCount)
            {
                yield return null; // ��Ÿ�� �ߴܵ� (��� �� ��)
                continue;
            }

            yield return new WaitForSeconds(_cooltimeMeatGrill);

            SpawnMeat();
        }
    }

    void SpawnMeat()
    {
        // ��� ��ġ ���: ���̵���
        Vector3 spawnPos = _locationOfMeat.position + Vector3.up * _stackHeight * _currentMeatCount;

        GameObject meat = Instantiate(_meat, spawnPos, Quaternion.identity, _locationOfMeat);
        _currentMeatCount++;

        Debug.Log($"��� ������ ({_currentMeatCount}/{_maxMeatCount})");
    }

    // �ܺο��� ��⸦ �������� ���ҽ�ų �� �ְ� �޼��� ����
    public bool TakeMeat()
    {
        if (_currentMeatCount <= 0) return false;

        _currentMeatCount--;
        return true;
    }
}