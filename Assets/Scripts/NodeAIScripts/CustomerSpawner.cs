using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("������ �մ� ������")]
    [SerializeField] private GameObject _customerPrefab;

    [Header("������ ��� ��ġ (�迭 ����)")]
    [SerializeField] private Vector2Int _spawnNodeGridPos = new Vector2Int(1, 24);

    [Header("���� ������ (��)")]
    [SerializeField] private float _spawnDelay = 2f;

    void Start()
    {
        StartCoroutine(SpawnCustomerWithDelay(_spawnNodeGridPos, _spawnDelay));
    }

    //TODO: ���� Ư�� ���ǿ� ���缭 �մ� ����. (�ʹ� ���� �����ǰų� �ʹ� ���� �����Ǹ� �ȵǹǷ�)
    IEnumerator SpawnCustomerWithDelay(Vector2Int gridPos, float delay)
    {
        yield return new WaitForSeconds(delay);

        Node node = NodeManager._instance._nodeList[gridPos.x, gridPos.y];

        if (node == null || !node._isWalkale)
        {
            Debug.LogError("�����Ϸ��� ��ġ�� �߸��Ǿ��ų�, ��ֹ��� ����.");
            yield break;
        }

        //TODO: �մ� ������Ʈ Ǯ������ ����
        Instantiate(_customerPrefab, node.transform.position, Quaternion.identity);
    }
}
