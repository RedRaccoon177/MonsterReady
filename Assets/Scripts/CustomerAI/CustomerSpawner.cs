using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("스폰할 손님 프리팹들 (여러개 넣기)")]
    [SerializeField] List<GameObject> _customerPrefabs;

    [Header("스폰할 노드 위치 (배열 기준)")]
    [SerializeField] Vector2Int _spawnNodeGridPos = new Vector2Int(1, 24);

    [Header("스폰 딜레이 (초)")]
    [SerializeField] float _spawnDelay = 2f;

    [Header("최대 손님 수")]
    [SerializeField] int _maxCustomerCount = 10;

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

            _spawnedCustomers.RemoveAll(c => c == null);
            if (_spawnedCustomers.Count >= _maxCustomerCount)
                continue;

            Node node = NodeManager._instance._nodeList[gridPos.x, gridPos.y];
            if (node == null || !node._isWalkale)
            {
                Debug.LogError("스폰 위치 오류 or 장애물 있음.");
                yield break;
            }

            GameObject randomPrefab = GetRandomCustomerPrefab();
            if (randomPrefab == null)
            {
                Debug.LogWarning("손님 프리팹이 비어있습니다.");
                continue;
            }

            GameObject customer = Instantiate(randomPrefab, node.transform.position, Quaternion.identity);
            _spawnedCustomers.Add(customer);
        }
    }

    GameObject GetRandomCustomerPrefab()
    {
        if (_customerPrefabs.Count == 0)
            return null;

        int randomIndex = Random.Range(0, _customerPrefabs.Count);
        return _customerPrefabs[randomIndex];
    }
}
