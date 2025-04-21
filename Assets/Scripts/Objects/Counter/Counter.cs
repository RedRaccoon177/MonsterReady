using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : BaseObject, ILevelable
{
    #region 기본 정보
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

    // 생성된 고기 오브젝트들을 담는 리스트
    List<GameObject> _meatList = new List<GameObject>();

    //고기 굽는거 담는 코루틴
    Coroutine _grillRoutine;

    //플레이어 정보
    PlayerController _player;
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
