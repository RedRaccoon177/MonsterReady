using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeHam : MonoBehaviour
{
    // �ֺ� ���� ���͵� (�����¿� + �밢�� �� 8����)
    Vector3[] directions;

    // �� ��尡 �̵� �������� ����
    [SerializeField] public bool _isWalkale;

    // ����� ���� ��� ���
    public List<NodeHam> _connectionNodes = new List<NodeHam>();

    private void Start()
    {
        // 8���� ���� (����ȭ ����)
        directions = new Vector3[]
        {
            Vector3.right,                         // ������
            Vector3.left,                          // ����
            Vector3.forward,                       // ��
            Vector3.back,                          // ��
            new Vector3(1, 0, 1).normalized,       // ������ �� �밢��
            new Vector3(1, 0, -1).normalized,      // ������ �Ʒ� �밢��
            new Vector3(-1, 0, 1).normalized,      // ���� �� �밢��
            new Vector3(-1, 0, -1).normalized      // ���� �Ʒ� �밢��
        };

        _isWalkale = true; // �⺻�����δ� �̵� �����ϴٰ� ����

        // OverlapBox�� �̿��� �ֺ� �浹ü ���� (0.7 x 1 x 0.7 ũ��)
        Vector3 halfExtents = new Vector3(0.7f, 1f, 0.7f);
        Collider[] hits = Physics.OverlapBox(transform.position, halfExtents);

        int count = 0;
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue; // �ڱ� �ڽ��� ����

            // Ư�� �±״� ���� ó��
            if (hit.CompareTag("Player") || hit.CompareTag("Door") || hit.CompareTag("Plane") || hit.CompareTag("ExpandWall"))
                continue;

            count++; // ���ܰ� �ƴ� �浹ü�� �ϳ��� �ִٸ�
        }

        // �浹ü�� ������ �̵� �Ұ����� ���� ǥ��
        if (count >= 1)
        {
            _isWalkale = false;
        }

        // �ֺ� ��� Ž���� ���� Raycast ����
        RaycastHit hitS;
        foreach (var a in directions)
        {
            // ������ �������� Ray�� ���� Node ���̾ �浹�ߴ��� Ȯ��
            if (Physics.Raycast(transform.position, a, out hitS, Mathf.Infinity, LayerMask.GetMask("Node")))
            {
                NodeHam node = hitS.collider.GetComponent<NodeHam>();
                if (node != null)
                {
                    // �浹�� ������Ʈ�� NodeHam�� �ִٸ� ���� ���� ���
                    _connectionNodes.Add(node);
                }
            }
        }
    }


    /// <summary>
    /// ��� halfExtents �ð�ȭ (��ġ�� ����, �Ȱ�ġ�� ����)
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            _isWalkale = true;

        Gizmos.color = _isWalkale ? new Color(0f, 1f, 0f, 0.3f) : new Color(1f, 0f, 0f, 0.3f);
        Vector3 halfExtents = new Vector3(0.7f, 1f, 0.7f);
        Gizmos.DrawCube(transform.position, halfExtents * 2);
    }
}
