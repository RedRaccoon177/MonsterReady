using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : BaseObject, ILevelable, IStackChecker
{
    #region 키값 및 레벨
    [SerializeField] public int _level;

    public string GetKey()
    {
        return _keyName;
    }

    public int SetLevel(int level)
    {
        _level = level;
        return level;
    }

    public void LevelUp()
    {
        _level++;
    }

    public int GetLevel()
    {
        return _level;
    }
    #endregion

    #region 변수들
    [Header("오브젝트 풀링 연결")]
    [SerializeField] ObjectPooling _meatPool; // 고기를 관리하는 오브젝트 풀

    [Header("고기 프리펩")]
    [SerializeField] GameObject _meatPrefab;

    [Header("고기 배치하는 곳")]
    [SerializeField] Transform _meatSpawnLocation;

    [Header("현재 고기 갯수")]
    [SerializeField] int _currentMeatCount = 0;

    [Header("고기 쌓일 높이 간격")]
    [SerializeField] float _stackHeight = 0.11f;
    [SerializeField] float _counterY = 1.65f;

    // 생성된 고기 오브젝트들을 담는 리스트
    List<GameObject> _meatList = new List<GameObject>();

    //고기 굽는거 담는 코루틴
    Coroutine _grillRoutine;

    //플레이어 정보
    PlayerController _player;

    [Header("카운터 옆에 달린 현금")]
    [SerializeField] GoldObject _goldObject;
    #endregion

    void Start()
    {
        _player = PlayerController._instance;
    }

    #region 고기 증가 및 감소
    /// <summary>
    /// 카운터 고기 증가 함수
    /// </summary>
    /// <param name="_meatCount"></param>
    void AddMeat(int _meatCount)
    {
        _currentMeatCount = Mathf.Max(0, _currentMeatCount + _meatCount);
        UpdateMeatDisplay(_currentMeatCount);
    }

    /// <summary>
    /// 카운터 고기 감소
    /// </summary>
    /// <param name="_minusMeat"></param>
    int MinusMeat(int _minusMeat)
    {
        int _playerGetMeat;

        if (_currentMeatCount < _minusMeat)
        {
            _playerGetMeat = _currentMeatCount;
            _currentMeatCount = 0;
        }
        else
        {
            _playerGetMeat = _minusMeat;
            _currentMeatCount -= _minusMeat;
        }

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
            _meatSpawnLocation.position.y + index * _stackHeight + _counterY,
            _meatSpawnLocation.position.z
        );
    }
    #endregion

    // 플레이어가 범위에 들어왔을 때 고기 자동 제공
    private void OnTriggerEnter(Collider other)
    {
        // 태그가 Player가 아닐 경우 무시
        if (!other.CompareTag("Player")) return;

        //플레이어의 정보를 바탕으로 더해야 할 고기
        if (other.CompareTag("Player"))
        {
            if (0 != _player._CurrentMeat)
            {
                AddMeat( _player._CurrentMeat );
                _player.MinusMeat(_currentMeatCount);
            }
        }
        else if (other.CompareTag("NPC"))
        {
            //TODO: NPC 캐릭터들 고기 획득
        }
    }

    public bool CheckStack()
    {
        return _currentMeatCount > 0;
    }
}
