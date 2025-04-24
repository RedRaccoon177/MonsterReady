using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("스폰할 손님 프리팹")]
    [SerializeField] private GameObject _customerPrefab;

    [Header("스폰할 노드 위치 (배열 기준)")]
    [SerializeField] private Vector2Int _spawnNodeGridPos = new Vector2Int(1, 24);

    [Header("스폰 딜레이 (초)")]
    [SerializeField] private float _spawnDelay = 2f;

    void Start()
    {
        StartCoroutine(SpawnCustomerWithDelay(_spawnNodeGridPos, _spawnDelay));
    }

    //TODO: 추후 특정 조건에 맞춰서 손님 생성. (너무 많이 생성되거나 너무 적게 생성되면 안되므로)
    IEnumerator SpawnCustomerWithDelay(Vector2Int gridPos, float delay)
    {
        yield return new WaitForSeconds(delay);

        Node node = NodeManager._instance._nodeList[gridPos.x, gridPos.y];

        if (node == null || !node._isWalkale)
        {
            Debug.LogError("스폰하려는 위치가 잘못되었거나, 장애물이 있음.");
            yield break;
        }

        //TODO: 손님 오브젝트 풀링으로 관리
        Instantiate(_customerPrefab, node.transform.position, Quaternion.identity);
    }
}
