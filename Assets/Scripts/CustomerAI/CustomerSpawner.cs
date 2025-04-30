using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [Header("������ �մ� ������")]
    [SerializeField] GameObject _customerPrefab;

    [Header("������ ��� ��ġ (�迭 ����)")]
    [SerializeField] Vector2Int _spawnNodeGridPos = new Vector2Int(1, 24);

    [Header("���� ������ (��)")]
    [SerializeField] float _spawnDelay = 2f;

    void Start()
    {
        StartCoroutine(SpawnCustomerRepeatedly(_spawnNodeGridPos, _spawnDelay));
    }

    /// <summary>
    /// ������ �����̸��� �ݺ������� �մ��� ������
    /// </summary>
    IEnumerator SpawnCustomerRepeatedly(Vector2Int gridPos, float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(10000f);

            Node node = NodeManager._instance._nodeList[gridPos.x, gridPos.y];

            if (node == null || !node._isWalkale)
            {
                Debug.LogError("�����Ϸ��� ��ġ�� �߸��Ǿ��ų�, ��ֹ��� ����.");
                yield break; // �Ǵ� continue; �� �ٲٸ� ��� �ݺ� �õ�
            }

            // TODO: �մ� ������Ʈ Ǯ������ �ٲٱ�
            Instantiate(_customerPrefab, node.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(delay);
        }
    }
}
