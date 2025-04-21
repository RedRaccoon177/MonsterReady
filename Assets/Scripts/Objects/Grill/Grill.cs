using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : InteractionObject
{
    #region 변수들
    [Header("오브젝트 풀링 연결")]
    [SerializeField] ObjectPooling _meatPool; // 고기를 관리하는 오브젝트 풀

    [Header("고기 프리펩")]
    [SerializeField] GameObject _meatPrefab;

    [Header("고기 배치하는 곳")]
    [SerializeField] Transform _meatSpawnLocation;

    [Header("최대치 고기 갯수")]
    [SerializeField] int _maxMeatCount = 5;

    [Header("현재 만들어진 고기")]
    [SerializeField] int _currentMeatCount = 0;

    [Header("고기 굽는 쿨타임 (초)")]
    [SerializeField] float _cooltimeMeatGrill = 3f;

    [Header("고기 쌓일 높이 간격")]
    [SerializeField] float _stackHeight = 0.11f;

    // 생성된 고기 오브젝트들을 담는 리스트
    List<GameObject> _meatList = new List<GameObject>();

    //고기 굽는거 담는 코루틴
    Coroutine _grillRoutine;

    //플레이어 정보
    PlayerController _player;
    #endregion

    #region Start, OnTriggerEnter
    void Start()
    {
        _player = PlayerController._instance;
        // 게임 시작 시 고기 자동 생성 시작
        StartGrill();
    }

    // 플레이어가 범위에 들어왔을 때 고기 자동 제공
    private void OnTriggerEnter(Collider other)
    {
        // 태그가 Player가 아닐 경우 무시
        if (!other.CompareTag("Player")) return;


        //플레이어의 정보를 바탕으로 빼야할 고기 값
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
            //TODO: NPC 캐릭터들 고기 획득
        }
    }
    #endregion

    #region 고기 생성 코루틴
    // 고기 굽기 시작
    public void StartGrill()
    {
        // 중복 실행 방지
        if (_grillRoutine == null)
        {
            _grillRoutine = StartCoroutine(GrillLoop());
        }
    }
    
    // 고기 굽기 루프
    IEnumerator GrillLoop()
    {
        while (true)
        {
            // 고기 굽는 시간 대기
            yield return new WaitForSeconds(_cooltimeMeatGrill);

            // 최대치에 도달하면 대기 (생성 중단)
            if (_currentMeatCount >= _maxMeatCount)
            {
                yield return null;
                continue;   //아래 코드 무시하고 다시 while처음으로 이동
            }

            // 고기 생성
            AddMeat(1);
        }
    }
    #endregion

    #region 고기 증가 및 감소
    /// <summary>
    /// 고기 증가
    /// </summary>
    /// <param name="_meatCount"></param>
    void AddMeat(int _meatCount)
    {
        _currentMeatCount = Mathf.Clamp(_currentMeatCount + _meatCount, 0, _maxMeatCount);
        UpdateMeatDisplay(_currentMeatCount);
    }

    /// <summary>
    /// 고기 감소
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

    #region 고기 시각화 하는 코드 모음
    //고기 시각화 하는 코드
    public void UpdateMeatDisplay(int currentMeat)
    {
        // 1. 고기 개수가 부족하면 채워줌
        while (_meatList.Count < currentMeat)
        {
            GameObject meat = _meatPool.GetMeat(); // 오브젝트 풀에서 꺼냄

            // 생성 위치값, 회전값, 크기값
            meat.transform.localPosition = GetStackPosition(_meatList.Count);
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }

        // 2. 고기 개수가 초과되면 제거 (위에서부터 하나씩)
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