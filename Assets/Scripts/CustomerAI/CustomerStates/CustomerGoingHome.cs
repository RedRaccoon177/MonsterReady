using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGoingHome : ICustomerState
{
    List<Node> _path;       // A* ��� ����
    int _currentIndex;      // ���� ��ǥ ��� �ε���
    Vector2Int _exitNodeGridPos = new Vector2Int(0, 0); // �ⱸ ��ġ (��: 0,0) ���ⱸ ��ǥ ���� �ʿ�

    public void Enter(CustomerAI customer)
    {
        // ���� ��ġ���� ���� ����� ���� ��� ã��
        Node _startNode = GetClosestNode(customer.transform.position);

        // ���� ���� ��� ���� (�ⱸ)
        Node _goalNode = NodeManager._instance._nodeList[_exitNodeGridPos.x, _exitNodeGridPos.y];

        if (_startNode == null || _goalNode == null)
        {
            Debug.LogError("CustomerGoingHome: ���� ��� �Ǵ� ��ǥ ��带 ã�� �� �����ϴ�.");
            return;
        }

        // A*�� ��� ���
        _path = AStarPathfinder.FindPath(_startNode, _goalNode);
        _currentIndex = 0;
    }

    public void Update(CustomerAI customer)
    {
        if (_path == null || _currentIndex >= _path.Count)
        {
            // ���� �Ϸ� �� �մ� ����
            GameObject.Destroy(customer.gameObject);
            return;
        }

        Node _targetNode = _path[_currentIndex];
        Vector3 _targetPos = _targetNode.transform.position;
        float _step = 5f * Time.deltaTime;

        customer.transform.position = Vector3.MoveTowards(customer.transform.position, _targetPos, _step);

        if (Vector3.Distance(customer.transform.position, _targetPos) < 0.1f)
        {
            _currentIndex++;
        }
    }

    public void Exit(CustomerAI customer)
    {

    }

    // ���� ��ġ���� ���� ����� ��带 ã�� �Լ�
    Node GetClosestNode(Vector3 pos)
    {
        float _minDist = float.MaxValue;
        Node _closestNode = null;

        foreach (Node _node in NodeManager._instance._nodeList)
        {
            if (_node == null || !_node._isWalkale)
                continue;

            float _dist = Vector3.Distance(pos, _node.transform.position);
            if (_dist < _minDist)
            {
                _minDist = _dist;
                _closestNode = _node;
            }
        }

        if (_closestNode == null)
            Debug.LogError("GetClosestNode(): ��ȿ�� ��带 ã�� ���߽��ϴ�.");

        return _closestNode;
    }
}
