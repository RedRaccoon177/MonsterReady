using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatToBoxTrigger : MonoBehaviour
{
    [Header("오브젝트 풀링 연결")]
    [SerializeField] ObjectPooling _boxPool; // 박스를 관리하는 오브젝트 풀

    [Header("박스 프리펩")]
    [SerializeField] GameObject _boxPrefab;

    [Header("박스 배치하는 곳")]
    [SerializeField] Transform _boxSpawnLocation;

    [Header("현재 박스 갯수")]
    [SerializeField] public int _currentBoxCount = 0;

    [Header("박스 쌓일 높이 간격")]
    [SerializeField] float _stackHeight = 0.333f;
    [SerializeField] float _counterY = 1.9f;

    [Header("고기 테이블 정보")]
    [SerializeField] MeatInputTrigger _meatInputTrigger;

    // 생성된 박스 오브젝트들을 담는 리스트
    List<GameObject> _boxList = new List<GameObject>();

    // 플레이어 정보
    PlayerController _player;

    // 고기를 박스로 전환 시키는 코루틴
    Coroutine _meatChangeToBoxC;

    //고기를 박스로 전환시키고 있는 중인가?
    bool _isActive = false;

    //고기를 박스로 전환시키는 시간
    WaitForSeconds _waitDelay;
    [SerializeField] float _waitTime = 1;

    void Start()
    {
        _waitDelay = new WaitForSeconds(_waitTime);
        _player = PlayerController._instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isActive && _meatInputTrigger._currentMeatCount > 0)
        {
            _meatChangeToBoxC = StartCoroutine(MeatChangeToBox());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _meatChangeToBoxC != null)
        {
            StopCoroutine(_meatChangeToBoxC);
            _meatChangeToBoxC = null;
            _isActive = false;
        }
    }

    #region 고기를 박스로 전환하는 코루틴
    IEnumerator MeatChangeToBox()
    {
        _isActive = true;

        while (_meatInputTrigger._currentMeatCount > 0)
        {
            ChangeMeatTOBox();            // 고기 하나 전환
            yield return _waitDelay;      // 1초 대기
        }

        _isActive = false;
    }

    public void ChangeMeatTOBox()
    {
        //1개의 고기를 뺀다.
        _meatInputTrigger.MinusMeat(1);

        //1개의 박스를 더한다.
        AddBox(1);
    }
    #endregion

    #region 박스 증가 및 감소
    /// <summary>
    /// 카운터 박스 증가 함수
    /// </summary>
    /// <param name="_meatCount"></param>
    void AddBox(int _meatCount)
    {
        _currentBoxCount = Mathf.Max(0, _currentBoxCount + _meatCount);
        UpdateBoxDisplay(_currentBoxCount);
    }

    /// <summary>
    /// 카운터 박스 감소
    /// </summary>
    /// <param name="_minusMeat"></param>
    public int MinusBox(int _minusMeat)
    {
        int _someoneGetMeat;

        if (_currentBoxCount < _minusMeat)
        {
            _someoneGetMeat = _currentBoxCount;
            _currentBoxCount = 0;
        }
        else
        {
            _someoneGetMeat = _minusMeat;
            _currentBoxCount -= _minusMeat;
        }

        UpdateBoxDisplay(_currentBoxCount);
        return _someoneGetMeat;
    }
    #endregion

    #region 박스 시각화 하는 코드 모음
    //박스 시각화 하는 코드
    public void UpdateBoxDisplay(int currentMeat)
    {
        // 1. 고기 개수가 부족하면 채워줌
        while (_boxList.Count < currentMeat)
        {
            GameObject box = _boxPool.GetBox(); // 오브젝트 풀에서 꺼냄

            // 생성 위치값, 회전값, 크기값
            box.transform.localPosition = GetStackPosition(_boxList.Count);
            box.transform.localRotation = Quaternion.identity;
            box.transform.localScale = _boxPrefab.transform.localScale;

            _boxList.Add(box);
        }

        // 2. 고기 개수가 초과되면 제거 (위에서부터 하나씩)
        while (_boxList.Count > currentMeat)
        {
            GameObject lastMeat = _boxList[_boxList.Count - 1];
            _boxList.RemoveAt(_boxList.Count - 1);
            _boxPool.ReturnToPool(lastMeat);
        }
    }
    Vector3 GetStackPosition(int index)
    {
        return new Vector3
        (
            _boxSpawnLocation.position.x,
            _boxSpawnLocation.position.y + index * _stackHeight + _counterY,
            _boxSpawnLocation.position.z
        );
    }
    #endregion

}
