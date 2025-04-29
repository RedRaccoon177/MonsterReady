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

    #region 손님 필드
    public Table _table;         // 내가 앉아있는 테이블
    public int _AteMeatCount = 0; // 먹은 고기 총 개수

    [Header("오브젝트 풀링 연결")]
    [SerializeField] ObjectPooling _meatPool;

    [Header("고기, 뼈 프리팹")]
    [SerializeField] GameObject _meatPrefab;              // 고기 프리팹

    [Header("고기, 뼈 배치 위치 및 쌓기 간격")]
    [SerializeField] Transform _meatSpawnLocation;        // 고기/뼈 스폰 기준 위치

    List<GameObject> _meatList = new List<GameObject>();  // 쌓인 고기 오브젝트 리스트

    [Header("고기 배치 위치 및 쌓기 간격")]
    [SerializeField] float _stackHeight = 0.11f;          // 쌓이는 간격 높이

    //손님이 요구하는 최소치 고기
    [SerializeField] int _minMeat;

    //손님이 최대치 고기를 받는 양
    [SerializeField] int _maxMeat;
    
    //손님이 받은 현재 고기 양
    [SerializeField] int _currentMeat;
    #endregion

    #region 변수 프로퍼티
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
        // ObjectPooling 자동 연결
        if (_meatPool == null)
        {
            _meatPool = FindObjectOfType<ObjectPooling>();

            if (_meatPool == null)
            {
                Debug.LogError("[CustomerAI] 씬에 ObjectPooling 오브젝트가 없습니다!");
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

    #region 상태패턴 지정 함수
    public void SetState(ICustomerState newState)
    {
        _currentState?.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }
    #endregion

    #region 고기 관련 함수
    /// <summary>
    /// 고기 하나 먹기
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
    /// 테이블에 고기 추가
    /// </summary>
    public void AddMeat(int amount)
    {
        _CurrentMeat += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject meat = _meatPool.GetMeat();                      // 고기 풀에서 꺼내기
            meat.transform.SetParent(_meatSpawnLocation);
            meat.transform.localPosition = GetStackMeatAndBonePosition(_meatList.Count); // 현재 고기 수만큼 높이 조정
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }
    }

    public void UpdateMeatDisplay(int currentMeat)
    {
        // 1. 고기 개수가 부족하면 채워줌
        while (_meatList.Count < currentMeat)
        {
            GameObject meat = _meatPool.GetMeat();

            // 부모 설정
            meat.transform.SetParent(transform);  // 손님 오브젝트에 따라다니게
            meat.transform.localPosition = GetStackMeatAndBonePosition(_meatList.Count);
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }

        // 2. 고기 개수가 초과되면 제거
        while (_meatList.Count > currentMeat)
        {
            GameObject lastMeat = _meatList[_meatList.Count - 1];
            _meatList.RemoveAt(_meatList.Count - 1);
            _meatPool.ReturnToPool(lastMeat);
        }
    }

    /// <summary>
    /// 손님 기준 정면에 고기 쌓는 위치 계산
    /// </summary>
    Vector3 GetStackMeatAndBonePosition(int index)
    {
        float baseHeight = 1f; // 손님 위치보다 위에
        float forwardOffset = 0.5f; // 손님 앞쪽
        return new Vector3(
            0f,
            baseHeight + index * _stackHeight,
            forwardOffset
        );
    }

    /// <summary>
    /// 손님 앞에 보이는 모든 고기 시각화 제거
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