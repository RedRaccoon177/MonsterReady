using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMoveToCounterState : ICustomerState
{
    #region ������
    List<Node> _path;           // A*�� ���� ��θ� ������ ����Ʈ
    int _currentIndex;          // ���� ���󰡰� �ִ� ��� �ε���
    
    //���� �������� ī���� ��ġ
    Vector2Int _counterNodeGridPos = new Vector2Int(15, 5);
    #endregion

    #region  Enter, Update, Exit��
    public void Enter(CustomerAI customer)
    {
        // ���� ��ġ���� ���� ����� ���� ��� ã��
        Node _startNode = GetClosestNode(customer.transform.position);

        // ������ ��� ���� (��: ī���� ��ġ�� Ư�� ��ǥ)
        Node _goalNode = NodeManager._instance._nodeList[_counterNodeGridPos.x, _counterNodeGridPos.y];

        if (_startNode == null || _goalNode == null)
            return;

        // A* �˰������� ��� ���
        _path = AStarPathfinder.FindPath(_startNode, _goalNode);
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
            //�մ��� �ٿ� �����ߴ°�
            if(_path != null)
            {
                _path[_currentIndex - 1]._isCustomerWaiting = true;
            }

            customer.SetState(new CustomerOrderAndWait()); // ���� �ൿ ���·� ��ȯ (�ֹ� ��� ��)
            return;
        }

        // ���� �̵� ��ǥ ��� ����
        Node _targetNode = _path[_currentIndex];
        Vector3 _targetPos = _targetNode.transform.position;
        float _step = 5f * Time.deltaTime; // ������ ��� �̵� �Ÿ� ���

        //�տ� ��忡 �մ��� �ټ��� �ִٸ�?
        if(_targetNode._isCustomerWaiting)
        {
            if (_currentIndex != 0)
            {
                Node _currentNode = _path[_currentIndex - 1];

                // ���� ��ġ�� ��忡 �ƹ��� ������ ����
                _currentNode._isCustomerWaiting = true;
            }
        }
        else
        {
            // �մ��� ���� ��� ��ġ�� �̵���Ŵ
            customer.transform.position = Vector3.MoveTowards(customer.transform.position, _targetPos, _step);
            // ��ǥ ��ġ�� ���� �����ߴٸ� ���� ���� ��ȯ
            if (Vector3.Distance(customer.transform.position, _targetPos) < 0.1f)
            {
                _currentIndex++;
            }
        }
    }   

    public void Exit(CustomerAI customer) { }
    #endregion

    #region ���� ����� ��� ã��
    /// <summary>
    /// �־��� ��ġ���� ���� ����� ��ȿ�� Node�� ã�� ��ȯ
    /// </summary>
    /// <param name="pos">���� �մ��� ��ġ</param>
    /// <returns>���� ����� Node</returns>
    Node GetClosestNode(Vector3 pos)
    {
        //�ʱⰪ �ִ��
        float _minDist = float.MaxValue;
        Node _closestNode = null;

        foreach (Node _node in NodeManager._instance._nodeList)
        {
            if (_node == null)
            {
                Debug.Log("null node �߰ߵ�");
                continue;
            }

            if (!_node._isWalkale)
            {
                continue;
            }

            float _dist = Vector3.Distance(pos, _node.transform.position);
            if (_dist < _minDist)
            {
                _minDist = _dist;
                _closestNode = _node;
            }
        } 

        if (_closestNode == null)
            Debug.LogError("GetClosestNode(): ��ȿ�� ��尡 ����");

        return _closestNode;
    }
    #endregion
}
