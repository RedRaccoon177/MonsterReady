using UnityEngine;
using System.Collections.Generic;

public class GoldObject : MonoBehaviour
{
    [SerializeField] public string _key;               // 골드바 가로 개수 (열 수)
    [Header("오브젝트 풀링 연결")]
    [SerializeField] ObjectPooling _goldPool; // 골드바를 관리하는 오브젝트 풀

    [Header("골드 프리팹 및 부모")]
    [SerializeField] GameObject _goldPrefab;        // 골드바 프리팹
    [SerializeField] Transform _goldSpawnLocation;  // 생성된 골드바들을 담을 위치

    [Header("배치 설정")]
    int _maxX = 4;               // 골드바 가로 개수 (열 수)
    int _maxZ = 2;               // 골드바 세로 개수 (행 수)
    int _maxY = 40;              // 골드바 층 수 (높이)
    float _spacingX = 0.33f;     // 가로 간격 (x축 간격)
    float _spacingY = 0.14f;     // 높이 간격 (y축 간격)
    float _spacingZ = 0.735f;      // 세로 간격 (z축 간격)
    Vector3 _spawnRotation = new Vector3(0f, 90f, 0f); // 골드바 회전값

    // 생성된 골드바들을 추적하는 리스트
    List<GameObject> _goldInstances = new List<GameObject>();

    // 현재 골드 수치 (화면에 표시되는 수가 아님, 논리적 수치)
    [SerializeField] public int _currentGold = 0;

    // 최대 시각적으로 표현 가능한 골드바 수 (가로 x 세로 x 높이)
    int _maxVisualGold => _maxX * _maxZ * _maxY;

    void Start()
    {
        // 초기 골드 상태 반영
        UpdateGoldDisplay(_currentGold);
    }

    #region 골드 시각적 생성
    /// <summary>
    /// 현재 골드 수에 따라 골드바를 화면에 생성 및 삭제(오브젝트 풀링이용)
    /// </summary>
    void UpdateGoldDisplay(int currentGold)
    {
        // 최대 시각적 개수까지 표시
        int visualCount = Mathf.Min(currentGold, _maxVisualGold);

        // 부족한 골드바가 있으면 풀에서 꺼내서 새로 배치
        while (_goldInstances.Count < visualCount)
        {
            GameObject gold = _goldPool.GetGoldBar(); // 풀에서 골드바 꺼냄

            // 트랜스폼 초기화 (위치 / 회전 / 크기 세팅)
            gold.transform.localPosition = GetStackPosition(_goldInstances.Count);
            gold.transform.localRotation = Quaternion.Euler(_spawnRotation); // 회전값
            gold.transform.localScale = _goldPrefab.transform.localScale; // 프리팹의 원래 크기 유지

            _goldInstances.Add(gold); // 리스트에 등록
        }

        // 초과된 골드바는 숨기고 풀에 반환
        for (int i = 0; i < _goldInstances.Count; i++)
        {
            bool shouldBeActive = i < visualCount;
            GameObject gold = _goldInstances[i];

            if (!shouldBeActive)
            {
                _goldPool.ReturnToPool(gold); // 풀로 반환
            }
        }
    }
    /// <summary>
    /// 주어진 인덱스를 기준으로 3D 위치를 계산하여 골드바를 정렬합니다.
    /// 중심 기준으로 퍼지도록 오프셋도 적용됩니다.
    /// </summary>
    Vector3 GetStackPosition(int index)
    {
        // x: 열 번호, z: 행 번호, y: 층 번호 계산
        int x = index % _maxX;
        int z = (index / _maxX) % _maxZ;
        int y = index / (_maxX * _maxZ);

        // 중앙 정렬을 위한 오프셋
        float offsetX = (_maxX - 1) * _spacingX * 0.5f;
        float offsetZ = (_maxZ - 1) * _spacingZ * 0.5f;

        // 최종 위치 반환
        return new Vector3
        (
            _goldSpawnLocation.position.x + - x * _spacingX + offsetX,   // 오른쪽에서 왼쪽으로 정렬
            _goldSpawnLocation.position.y + y * _spacingY,             // 아래에서 위로 쌓기
            _goldSpawnLocation.position.z + -z * _spacingZ + offsetZ    // 뒤쪽에서 앞쪽으로 정렬
        );
    }
    #endregion

    public void AddGold(int _addGold)
    {
        _currentGold += _addGold;
        UpdateGoldDisplay(_currentGold);
    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어가 트리거에 닿으면 골드 증가
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            _currentGold = player.AddGold(_currentGold); // 플레이어로부터 골드 증감 처리
            UpdateGoldDisplay(_currentGold);             // 시각적 갱신
        }
    }
}
