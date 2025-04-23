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

    [Header("��尣 ���� Ȯ�ο�")]
    [SerializeField] float _detectDistance = 2.1f;
    #endregion

    private void Start()
    {
        // 8���� ���� ���� (����ȭ ����)
        _directions = new Vector3[]
        {
            Vector3.right,                         // ������
            Vector3.left,                          // ����
            Vector3.forward,                       // �� (Z+)
            Vector3.back,                          // �Ʒ� (Z-)
            new Vector3(1, 0, 1).normalized,       // ������ �� �밢��
            new Vector3(1, 0, -1).normalized,      // ������ �Ʒ� �밢��
            new Vector3(-1, 0, 1).normalized,      // ���� �� �밢��
            new Vector3(-1, 0, -1).normalized      // ���� �Ʒ� �밢��
        };

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

        // �ֺ� ��� Ž�� ����
        RaycastHit hitS;
        foreach (var a in _directions)
        {
            // �ش� �������� Ray�� �߻��Ͽ� ���� ��� Ȯ��
            if (Physics.Raycast(transform.position, a, out hitS, _detectDistance, LayerMask.GetMask("Node")))
            {
                Node node = hitS.collider.GetComponent<Node>();
                if (node != null)
                {
                    // �ߺ� ���� ����
                    if (!_connectionNodes.Contains(node))
                    {
                        _connectionNodes.Add(node); // ���� ��� ��Ͽ� �߰�
                    }
                }
            }
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
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            _isWalkale = true; // ���� �߿��� �׻� �ʷ����� ���� ���� �ʱ�ȭ

        Gizmos.color = _isWalkale ? new Color(0f, 1f, 0f, 0.3f) : new Color(1f, 0f, 0f, 0.3f);
        Vector3 halfExtents = new Vector3(0.7f, 1f, 0.7f);
        Gizmos.DrawCube(transform.position, halfExtents * 2); // �ð�ȭ ũ�� = full size
    }
}
