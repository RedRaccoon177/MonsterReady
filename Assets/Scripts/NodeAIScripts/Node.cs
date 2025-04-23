using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Node : MonoBehaviour
{
    #region ��� ������
    // 8���� ���� ���͵� (�����¿� + �밢��)
    Vector3[] _directions;

    // �ش� ��尡 �̵� �������� ���� (��ֹ� ���� �Ǵ�)
    [SerializeField] public bool _isWalkale;

    // ������ ���� ������ ��� ���
    public List<Node> _connectionNodes = new List<Node>();

    // ����� �׸��� �� ��ǥ (A*���� �Ÿ� �������� ���)
    [SerializeField] Vector2Int _gridPos;

    #endregion

    void Start()
    {
        // �̵� �������� �ʱⰪ ����
        _isWalkale = true;

        // �ش� ��� ��ġ�� ��ֹ��� �ִ��� �˻�
        Vector3 halfExtents = new Vector3(0.7f, 1f, 0.7f); // �˻� �ڽ��� ũ��
        Collider[] hits = Physics.OverlapBox(transform.position, halfExtents);

        int count = 0;
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue; // �ڱ� �ڽ� ����

            // ���� �±״� �浹ü�� �Ǵ����� ����
            if (hit.CompareTag("Player") || hit.CompareTag("Door") || hit.CompareTag("Plane") || hit.CompareTag("ExpandWall"))
                continue;

            count++; // ������ �� ���� ��ֹ� ����
        }

        // �浹ü�� �ϳ��� ������ �̵� �Ұ��� ó��
        if (count >= 1)
        {
            _isWalkale = false;
        }
    }

    /// <summary>
    /// ����� ��ǥ ���� �ʱ�ȭ (NodeManager���� �Ҵ�)
    /// </summary>
    /// <param name="gridPos">�׸��� �� ��ǥ</param>
    public void Init(Vector2Int gridPos)
    {
        _gridPos = gridPos;
    }

    /// <summary>
    /// �����Ϳ��� ��� �ð�ȭ
    /// ��ġ�� ��ֹ��� ������ ������, �̵� �����ϸ� ���λ�
    /// </summary>
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            _isWalkale = true; // ���� �߿��� �׻� �ʷ����� ���� ���� �ʱ�ȭ

        Gizmos.color = _isWalkale ? new Color(0f, 1f, 0f, 0.3f) : new Color(1f, 0f, 0f, 0.3f);
        Vector3 halfExtents = new Vector3(0.7f, 1f, 0.7f);
        Gizmos.DrawCube(transform.position, halfExtents * 2); // �ð�ȭ ũ�� = full size
    }

    public void ConnectionNodes()
    {
        // 8���� ������
        Vector2Int[] offsets = new Vector2Int[]
        {
        new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1),
        new Vector2Int(0, -1),                     new Vector2Int(0, 1),
        new Vector2Int(1, -1),  new Vector2Int(1, 0),  new Vector2Int(1, 1)
        };

        foreach (var offset in offsets)
        {
            int newX = _gridPos.x + offset.x;
            int newY = _gridPos.y + offset.y;

            // �迭 ���� üũ
            if (newX >= 0 && newX < NodeManager._instance._nodeList.GetLength(0) &&
                newY >= 0 && newY < NodeManager._instance._nodeList.GetLength(1))
            {
                Node neighbor = NodeManager._instance._nodeList[newX, newY];

                //�̵� ������ ��常 ����
                if (neighbor != null && neighbor._isWalkale && !_connectionNodes.Contains(neighbor))
                {
                    _connectionNodes.Add(neighbor);
                }
            }
        }
    }
}
