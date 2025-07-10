using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("스폰할 손님 프리팹")]
    [SerializeField] GameObject _customerPrefab;

    [Header("스폰할 노드 위치 (배열 기준)")]
    [SerializeField] Vector2Int _spawnNodeGridPos = new Vector2Int(1, 24);

    [Header("스폰 딜레이 (초)")]
    [SerializeField] float _spawnDelay = 2f;

    [Header("최대 손님 수")]
    [SerializeField] int _maxCustomerCount = 10;

    // 현재 스폰된 손님들을 관리할 리스트
    private List<GameObject> _spawnedCustomers = new List<GameObject>();

    void Start()
    {
        StartCoroutine(SpawnCustomerRepeatedly(_spawnNodeGridPos, _spawnDelay));
    }

    IEnumerator SpawnCustomerRepeatedly(Vector2Int gridPos, float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);

            // 현재 손님 수 체크
            _spawnedCustomers.RemoveAll(c => c == null); // 죽은 손님 정리
            if (_spawnedCustomers.Count >= _maxCustomerCount)
            {
                continue; // 최대치면 스폰 건너뜀
            }

            Node node = NodeManager._instance._nodeList[gridPos.x, gridPos.y];
            if (node == null || !node._isWalkale)
            {
                Debug.LogError("스폰하려는 위치가 잘못되었거나, 장애물이 있음.");
                yield break;
            }

            GameObject customer = Instantiate(_customerPrefab, node.transform.position, Quaternion.identity);
            _spawnedCustomers.Add(customer);
        }
    }
}
