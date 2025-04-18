using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : InteractionObject
{
    [Header("고기 프리펩")]
    [SerializeField] GameObject _meat;

    [Header("고기 배치하는 곳")]
    [SerializeField] Transform _locationOfMeat;

    [Header("최대치 고기 갯수")]
    [SerializeField] int _maxMeatCount = 20;

    [Header("현재 만들어진 고기")]
    [SerializeField] int _currentMeatCount = 0;

    [Header("고기 굽는 쿨타임 (초)")]
    [SerializeField] float _cooltimeMeatGrill = 3f;

    [Header("고기 쌓일 높이 간격")]
    [SerializeField] float _stackHeight = 0.1f;

    //고기 굽는거 담는 코루틴
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
                yield return null; // 쿨타임 중단됨 (고기 꽉 참)
                continue;
            }

            yield return new WaitForSeconds(_cooltimeMeatGrill);

            SpawnMeat();
        }
    }

    void SpawnMeat()
    {
        // 고기 위치 계산: 쌓이도록
        Vector3 spawnPos = _locationOfMeat.position + Vector3.up * _stackHeight * _currentMeatCount;

        GameObject meat = Instantiate(_meat, spawnPos, Quaternion.identity, _locationOfMeat);
        _currentMeatCount++;

        Debug.Log($"고기 생성됨 ({_currentMeatCount}/{_maxMeatCount})");
    }

    // 외부에서 고기를 가져가면 감소시킬 수 있게 메서드 제공
    public bool TakeMeat()
    {
        if (_currentMeatCount <= 0) return false;

        _currentMeatCount--;
        return true;
    }
}