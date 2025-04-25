using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMoveToTable : ICustomerState
{
    #region ������
    List<Node> _path;           // A*�� ���� ��θ� ������ ����Ʈ
    int _currentIndex;          // ���� ���󰡰� �ִ� ��� �ε���

    //�رݵ� ���� ����Ʈ
    Dictionary<Vector2Int, Node> _emptyChairsCheck = new Dictionary<Vector2Int, Node>();

    //�� ���̺���� ���� ��� �׸���
    List<Vector2Int>[] _chairPositions = new List<Vector2Int>[12];
    #endregion


    #region  Enter, Update, Exit��
    public void Enter(CustomerAI customer)
    {
        InitChairGridPos();
        RegisterChairNodes();

        //�رݵ� ���� ����Ʈ ���� ���� �� �ִ��� Ȯ���ϱ�
        //1. ���� �� ������ �ű�� �̵��ϰ� �Ұ���
        //2. ���� �� �ִ� �ڸ� ������ ����ϱ�

        // ���� ��ġ���� ���� ����� ���� ��� ã��
        Node _startNode = GetClosestNode(customer.transform.position);

        //���� ��ǥ�� �� Ȯ�� �� ���� �� �ִ� �� Ȯ��


        // ������ ��� ���� (��: ī���� ��ġ�� Ư�� ��ǥ)
        //Node _goalNode = NodeManager._instance._nodeList[_counterNodeGridPos.x, _counterNodeGridPos.y];



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
            if (_path != null)
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
        if (_targetNode._isCustomerWaiting)
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

    #region ���� ��� ��ǥ ��� �Լ���
    /// <summary>
    /// ���� ����� �� ��ǥ
    /// </summary>
    void InitChairGridPos()
    {
        _chairPositions[0] = new List<Vector2Int> { new Vector2Int(15, 8), new Vector2Int(15, 10) };

        _chairPositions[1] = new List<Vector2Int> { new Vector2Int(15, 12), new Vector2Int(15, 14) };

        _chairPositions[2] = new List<Vector2Int> { new Vector2Int(12, 12), new Vector2Int(12, 14) };

        _chairPositions[3] = new List<Vector2Int> { new Vector2Int(12, 8), new Vector2Int(12, 10) };

        _chairPositions[4] = new List<Vector2Int> { new Vector2Int(9, 13), new Vector2Int(8, 13), new Vector2Int(9, 15), new Vector2Int(8, 15) };

        _chairPositions[5] = new List<Vector2Int> { new Vector2Int(9, 8), new Vector2Int(8, 8), new Vector2Int(9, 10), new Vector2Int(8, 10) };

        _chairPositions[6] = new List<Vector2Int> { new Vector2Int(5, 16) };

        _chairPositions[7] = new List<Vector2Int> { new Vector2Int(5, 12) };

        _chairPositions[8] = new List<Vector2Int> { new Vector2Int(5, 8) };

        _chairPositions[9] = new List<Vector2Int> { new Vector2Int(6, 3) };

        _chairPositions[10] = new List<Vector2Int> { new Vector2Int(9, 3) };

        _chairPositions[11] = new List<Vector2Int> { new Vector2Int(12, 3) };
    }

    /// <summary>
    /// ���� ��� �׸��� ��ǥ�� ��Ͻ�Ű��
    /// </summary>
    void RegisterChairNodes()
    {
        for (int i = 0; i < _chairPositions.Length; i++)
        {
            foreach (Vector2Int pos in _chairPositions[i])
            {
                Node _chairNode = NodeManager._instance._nodeList[pos.x, pos.y];

                if (!_emptyChairsCheck.ContainsKey(pos))
                {
                    _emptyChairsCheck.Add(pos, _chairNode);
                }
            }
        }
    }
    #endregion
}