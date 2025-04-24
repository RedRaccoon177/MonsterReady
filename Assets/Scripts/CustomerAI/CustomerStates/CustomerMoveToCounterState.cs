using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMoveToCounterState : ICustomerState
{
    List<Node> _path;           // A*�� ���� ��θ� ������ ����Ʈ
    int _currentIndex;          // ���� ���󰡰� �ִ� ��� �ε���

    /// <summary>
    /// ���¿� ������ �� ȣ���. ���� ���� ���� ��带 �������� A* ��θ� �����
    /// </summary>
    public void Enter(CustomerAI customer)
    {
        // ���� ��ġ���� ���� ����� ���� ��� ã��
        Node start = GetClosestNode(customer.transform.position);

        // ������ ��� ���� (��: ī���� ��ġ�� Ư�� ��ǥ)
        Node goal = NodeManager._instance._nodeList[15, 5];

        if (start == null || goal == null)
            return;

        // A* �˰������� ��� ���
        _path = AStarPathfinder.FindPath(start, goal);
        _currentIndex = 0; // ��� ���� �ε��� �ʱ�ȭ
    }

    /// <summary>
    /// ���� ������Ʈ. �մ��� A* ��θ� ���� �� ĭ�� �̵���
    /// </summary>
    public void Update(CustomerAI customer)
    {
        // ��ΰ� ���ų� �̹� �����ߴٸ� ���� ���·� ��ȯ
        if (_path == null || _currentIndex >= _path.Count)
        {
            customer.SetState(new CustomerOrderAndWait()); // ���� �ൿ ���·� ��ȯ (�ֹ� ��� ��)
            return;
        }

        // ���� �̵� ��ǥ ��� ����
        Node targetNode = _path[_currentIndex];
        Vector3 targetPos = targetNode.transform.position;
        float step = 3f * Time.deltaTime; // ������ ��� �̵� �Ÿ� ���

        // �մ��� ���� ��� ��ġ�� �̵���Ŵ
        customer.transform.position = Vector3.MoveTowards(customer.transform.position, targetPos, step);

        // ��ǥ ��ġ�� ���� �����ߴٸ� ���� ���� ��ȯ
        if (Vector3.Distance(customer.transform.position, targetPos) < 0.1f)
        {
            _currentIndex++;
        }
    }

    /// <summary>
    /// ���� ���� �� ȣ��. ����� ó�� ����
    /// </summary>
    public void Exit(CustomerAI customer) { }

    /// <summary>
    /// �־��� ��ġ���� ���� ����� ��ȿ�� Node�� ã�� ��ȯ
    /// </summary>
    /// <param name="pos">���� �մ��� ��ġ</param>
    /// <returns>���� ����� Node</returns>
    private Node GetClosestNode(Vector3 pos)
    {
        float minDist = float.MaxValue;
        Node closest = null;

        foreach (Node node in NodeManager._instance._nodeList)
        {
            if (node == null)
            {
                Debug.Log("null node �߰ߵ�");
                continue;
            }

            if (!node._isWalkale)
            {
                Debug.DrawLine(pos, node.transform.position, Color.red, 2f); // ������
                continue;
            }

            Debug.DrawLine(pos, node.transform.position, Color.green, 2f); // ��ȿ�� ���: �ʷϼ�

            float dist = Vector3.Distance(pos, node.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = node;
            }
        }

        if (closest == null)
            Debug.LogError("GetClosestNode(): ��ȿ�� ��尡 ����");

        return closest;
    }

}
