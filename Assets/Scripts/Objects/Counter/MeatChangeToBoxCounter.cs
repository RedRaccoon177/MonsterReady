using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatChangeToBoxCounter : MonoBehaviour
{
    [Header("오브젝트 풀링 연결")]
    [SerializeField] private ObjectPooling _meatPool; // 고기를 관리하는 오브젝트 풀

    [Header("고기 프리팹")]
    [SerializeField] private GameObject _meatPrefab;

    [Header("고기 배치하는 곳")]
    [SerializeField] private Transform _meatSpawnLocation;

    [Header("현재 고기 갯수")]
    [SerializeField] public int _currentMeatCount = 0;

    [Header("고기 쌓일 높이 간격")]
    [SerializeField] private float _stackHeight = 0.11f;
    [SerializeField] private float _counterY = 1.65f;

    private List<GameObject> _meatList = new List<GameObject>(); // 생성된 고기 오브젝트 리스트

    private PlayerController _player;

    private void Awake()
    {
        _player = PlayerController._instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Npc")) return;

        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null && player._CurrentMeat > 0)
            {
                AddMeat(player._CurrentMeat);
                player.MinusMeat(_currentMeatCount);
                player.CheckPickUpObject();
            }
        }
        else if (other.CompareTag("Npc"))
        {
            var npc = other.GetComponent<NpcAi>();
            if (npc != null && npc._CurrentMeat > 0)
            {
                AddMeat(npc._CurrentMeat);
                npc.MinusMeat(_currentMeatCount);
                npc.CurrentPickUpType();
            }
        }
    }

    /// <summary>
    /// 고기 추가
    /// </summary>
    private void AddMeat(int meatCount)
    {
        _currentMeatCount = Mathf.Max(0, _currentMeatCount + meatCount);
        UpdateMeatDisplay(_currentMeatCount);
    }

    /// <summary>
    /// 고기 감소
    /// </summary>
    public int MinusMeat(int minusMeat)
    {
        int takenMeat;
        if (_currentMeatCount < minusMeat)
        {
            takenMeat = _currentMeatCount;
            _currentMeatCount = 0;
        }
        else
        {
            takenMeat = minusMeat;
            _currentMeatCount -= minusMeat;
        }

        UpdateMeatDisplay(_currentMeatCount);
        return takenMeat;
    }

    /// <summary>
    /// 고기 시각화 갱신
    /// </summary>
    public void UpdateMeatDisplay(int currentMeat)
    {
        while (_meatList.Count < currentMeat)
        {
            GameObject meat = _meatPool.GetMeat();
            meat.transform.localPosition = GetStackPosition(_meatList.Count);
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }

        while (_meatList.Count > currentMeat)
        {
            GameObject lastMeat = _meatList[_meatList.Count - 1];
            _meatList.RemoveAt(_meatList.Count - 1);
            _meatPool.ReturnToPool(lastMeat);
        }
    }

    /// <summary>
    /// 고기 쌓는 위치 계산
    /// </summary>
    private Vector3 GetStackPosition(int index)
    {
        return new Vector3(
            _meatSpawnLocation.position.x,
            _meatSpawnLocation.position.y + index * _stackHeight + _counterY,
            _meatSpawnLocation.position.z
        );
    }
}
