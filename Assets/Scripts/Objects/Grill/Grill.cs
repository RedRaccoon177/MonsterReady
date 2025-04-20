using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grill : InteractionObject
{
    #region 변수들
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

    // 생성된 고기 오브젝트들을 담는 리스트 (시각적 관리용)
    List<GameObject> _meatList = new List<GameObject>();

    //고기 굽는거 담는 코루틴
    Coroutine _grillRoutine;
    #endregion

    #region Start, Update 문
    void Start()
    {
        // 게임 시작 시 고기 자동 생성 시작
        StartGrill();
    }

    void Update()
    {
        // 디버그용 : 마우스 좌클릭으로 고기 하나 빼기
        if (Input.GetMouseButtonDown(0))
        {
            TakeMeat();
        }
    }
    #endregion

    // 고기 굽기 시작
    public void StartGrill()
    {
        // 중복 실행 방지
        if (_grillRoutine == null)
            _grillRoutine = StartCoroutine(GrillLoop());
    }

    // 고기 굽기 루프
    IEnumerator GrillLoop()
    {
        while (true)
        {
            // 최대치에 도달하면 대기 (생성 중단)
            if (_currentMeatCount >= _maxMeatCount)
            {
                yield return null;
                continue;
            }

            // 고기 굽는 시간 대기
            yield return new WaitForSeconds(_cooltimeMeatGrill);

            // 고기 생성
            SpawnMeat();
        }
    }

    // 고기 생성 및 위치 지정
    void SpawnMeat()
    {
        // 쌓이는 위치 계산 (현재 고기 개수 기준으로 위로 올림)
        Vector3 spawnPos = _locationOfMeat.position + Vector3.up * _stackHeight * _currentMeatCount;

        // 고기 생성 및 부모 설정
        GameObject meat = Instantiate(_meat, spawnPos, Quaternion.identity, _locationOfMeat);
        _meatList.Add(meat); // 리스트에 추가

        _currentMeatCount++;
        Debug.Log($"고기 생성됨 ({_currentMeatCount}/{_maxMeatCount})");
    }

    // 외부에서 고기를 하나 가져갈 때 호출
    public bool TakeMeat()
    {
        // 고기가 없으면 실패
        if (_currentMeatCount <= 0 || _meatList.Count == 0) return false;

        // 마지막 고기 제거
        GameObject lastMeat = _meatList[_meatList.Count - 1];
        _meatList.RemoveAt(_meatList.Count - 1);
        Destroy(lastMeat); // 시각적으로 제거

        _currentMeatCount--;
        return true;
    }

    // 플레이어가 범위에 들어왔을 때 고기 자동 제공
    private void OnTriggerEnter(Collider other)
    {
        // 태그가 Player가 아닐 경우 무시
        if (!other.CompareTag("Player")) return;

        // 플레이어 컴포넌트 가져오기
        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        // 플레이어가 필요한 고기 개수 계산
        int playerNeed = player._MaxMeat - player._CurrentMeat;
        if (playerNeed <= 0) return;

        // 줄 수 있는 고기 개수 결정
        int giveMeatCount = Mathf.Min(_currentMeatCount, playerNeed);

        // 반복하며 고기 전달
        for (int i = 0; i < giveMeatCount; i++)
        {
            if (TakeMeat())
            {
                player.AddMeat(1); // 플레이어에 고기 추가
            }
        }

        Debug.Log($"플레이어가 고기 {giveMeatCount}개 수령함 (현재 보유: {player._CurrentMeat}/{player._MaxMeat})");
    }
}