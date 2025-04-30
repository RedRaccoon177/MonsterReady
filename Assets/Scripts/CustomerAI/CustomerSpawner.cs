using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("스폰할 손님 프리팹")]
    [SerializeField] GameObject _customerPrefab;

    [Header("스폰할 노드 위치 (배열 기준)")]
    [SerializeField] Vector2Int _spawnNodeGridPos = new Vector2Int(1, 24);

    [Header("스폰 딜레이 (초)")]
    [SerializeField] float _spawnDelay = 2f;

    void Start()
    {
        StartCoroutine(SpawnCustomerRepeatedly(_spawnNodeGridPos, _spawnDelay));
    }

    /// <summary>
    /// 지정된 딜레이마다 반복적으로 손님을 생성함
    /// </summary>
    IEnumerator SpawnCustomerRepeatedly(Vector2Int gridPos, float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(10000f);

            Node node = NodeManager._instance._nodeList[gridPos.x, gridPos.y];

            if (node == null || !node._isWalkale)
            {
                Debug.LogError("스폰하려는 위치가 잘못되었거나, 장애물이 있음.");
                yield break; // 또는 continue; 로 바꾸면 계속 반복 시도
            }

            // TODO: 손님 오브젝트 풀링으로 바꾸기
            Instantiate(_customerPrefab, node.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(delay);
        }
    }
}
