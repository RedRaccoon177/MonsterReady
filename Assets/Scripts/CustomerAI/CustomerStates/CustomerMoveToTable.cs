using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �մ��� ������ ���ɹް� ���ڱ��� �̵��ϴ� ���� Ŭ����
/// </summary>
public class CustomerMoveToTable : ICustomerState
{
    #region ������
    List<Node> _path;       // A*�� ���� ��θ� ������ ����Ʈ
    int _currentIndex;      // ���� ���󰡰� �ִ� ��� �ε���

    // �̵� ���� ���� ���
    Node _startNode;                   

    // �رݵ� ���� üũ��
    Dictionary<Vector2Int, Node> _emptyChairsCheck = new Dictionary<Vector2Int, Node>();

    // ���̺� �� ���� ��ġ ���
    List<Vector2Int>[] _chairPositions = new List<Vector2Int>[12];

    // ���� ��ġ -> ���̺� ��ȣ ����
    Dictionary<Vector2Int, int> _chairToTableIndex = new Dictionary<Vector2Int, int>();

    // ���̺� ������Ʈ �迭
    Table[] _tables = new Table[12];
    #endregion

    #region  Enter, Update, Exit��
    public void Enter(CustomerAI customer)
    {
        InitChairGridPos();             // ���� ��ġ �ʱ�ȭ
        RegisterChairNodes();           // ���� ��� ���
        InitTables();                   // ���̺� ��ü �ʱ�ȭ
        MoveCustomerToChair(customer);  // �մ� �̵� ����
    }

    public void Update(CustomerAI customer)
    {
        // ��ΰ� ���ų� �̹� �����ߴٸ� ���� �Դ� ���·� ��ȯ
        if (_path == null || _currentIndex >= _path.Count)
        {
            customer.SetState(new CustomerEating());
            return;
        }

        Node _targetNode = _path[_currentIndex];
        Vector3 _targetPos = _targetNode.transform.position;
        float _step = 5f * Time.deltaTime; // �̵� �ӵ�

        customer.transform.position = Vector3.MoveTowards(customer.transform.position, _targetPos, _step);

        if (Vector3.Distance(customer.transform.position, _targetPos) < 0.1f)
        {
            if (_currentIndex == 0)
            {
                _startNode._isCustomerWaiting = false;   // ��� ��忡 ����� �մ� ����
            }
            _currentIndex++; // ���� ��η� �̵�
        }
    }

    public void Exit(CustomerAI customer) { }
    #endregion

    #region ���� ���� �ʱ�ȭ �� ���� �Լ���
    /// <summary>
    /// ���̺��� �رݵǾ� �ִ��� Ȯ��
    /// </summary>
    bool IsTableUnlocked(int tableIndex)
    {
        string tableKey = $"���̺�{tableIndex + 1}";

        if (GameManager._instance._baseObjectDict[tableKey])
        {
            BaseObject tableObj = GameManager._instance._baseObjectDict[tableKey];
            Debug.Log($"���̺� {tableKey} �ر� ����: {tableObj._isActive}");
            return tableObj._isActive;
        }
        else
        {
            Debug.LogWarning($"���̺� {tableKey}��(��) _baseObjectDict�� ����");
            return false;
        }
    }

    /// <summary>
    /// �رݵ� ���̺� �� ��� ������ ���� ��� ����Ʈ�� ������
    /// </summary>
    List<Node> GetAvailableChairNodes()
    {
        List<Node> availableChairs = new List<Node>();

        for (int i = 0; i < _chairPositions.Length; i++)
        {
            if (IsTableUnlocked(i))
            {
                foreach (Vector2Int chairPos in _chairPositions[i])
                {
                    if (_emptyChairsCheck.TryGetValue(chairPos, out Node chairNode))
                    {
                        availableChairs.Add(chairNode);
                    }
                    else
                    {
                        Debug.LogWarning($"GetAvailableChairNodes(): �� ���� üũ ���� - ��ġ {chairPos}");
                    }
                }
            }
        }

        Debug.Log($"GetAvailableChairNodes �Ϸ�: �̵� ������ ���� �� = {availableChairs.Count}");

        return availableChairs;
    }

    /// <summary>
    /// �մ��� �̵� ���� (��ǥ ���� ���� + ��� ã��)
    /// </summary>
    void MoveCustomerToChair(CustomerAI customer)
    {
        List<Node> availableChairs = GetAvailableChairNodes();

        if (availableChairs.Count == 0)
        {
            Debug.LogWarning("MoveCustomerToChair: �̵� ������ ���ڰ� ����");
            return;
        }

        Node targetChairNode = availableChairs[Random.Range(0, availableChairs.Count)]; // ���� ���� ����
        _startNode = GetClosestNode(customer.transform.position);                       // ���� ��ġ ���� ���� ����� ��� ã��

        if (_startNode == null)
        {
            Debug.LogError("MoveCustomerToChair: ���� ��尡 ����!");
            return;
        }

        _path = AStarPathfinder.FindPath(_startNode, targetChairNode); // A* ��� ã��

        if (_path == null || _path.Count == 0)
        {
            Debug.LogError("MoveCustomerToChair: ��� ��� ����!");
            return;
        }

        _currentIndex = 0;
        Debug.Log($"MoveCustomerToChair: ��� ���� ����! �� ��� ���� = {_path.Count}");

        // �̵��ϴ� ���̺� ����
        if (_chairToTableIndex.TryGetValue(targetChairNode._gridPos, out int tableIndex))
        {
            customer._table = _tables[tableIndex];
            Debug.Log($"�մ��� �̵��� ���̺�: {tableIndex + 1}�� ���̺�");
        }
        else
        {
            Debug.LogWarning("MoveCustomerToChair: ���ڿ� �ش��ϴ� ���̺��� ã�� �� ����");
        }
    }

    /// <summary>
    /// ���̺� ������Ʈ �ʱ�ȭ
    /// </summary>
    void InitTables()
    {
        for (int i = 0; i < _tables.Length; i++)
        {
            string tableName = $"���̺�{i + 1}";

            if (GameManager._instance._baseObjectDict.TryGetValue(tableName, out BaseObject tableObj))
            {
                _tables[i] = tableObj.GetComponent<Table>();

                if (_tables[i] == null)
                {
                    Debug.LogError($"{tableName}�� Table ��ũ��Ʈ�� ����!");
                }
            }
            else
            {
                Debug.LogWarning($"{tableName} ������Ʈ�� ã�� �� ����.");
            }
        }
    }
    #endregion

    #region ���� ����� ��� ã��
    /// <summary>
    /// ���� ��ġ���� ���� ����� �̵� ������ Node�� ��ȯ
    /// </summary>
    Node GetClosestNode(Vector3 pos)
    {
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
                continue; // �̵� �Ұ����� ���� ��ŵ
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

    #region ���� ��ǥ ���� �� ���
    /// <summary>
    /// �� ���̺� ���ڵ��� �׸��� ��ġ ����
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
    /// ���� ������ ����ϰ�, ���� ��ġ�� ���̺� ����
    /// </summary>
    void RegisterChairNodes()
    {
        for (int i = 0; i < _chairPositions.Length; i++)
        {
            foreach (Vector2Int pos in _chairPositions[i])
            {
                Node _chairNode = NodeManager._instance._nodeList[pos.x, pos.y];

                if (_chairNode == null)
                {
                    Debug.LogError($"RegisterChairNodes(): Node�� null��! ��ġ: {pos}");
                    continue;
                }

                if (!_emptyChairsCheck.ContainsKey(pos))
                {
                    _emptyChairsCheck.Add(pos, _chairNode);
                }

                if (!_chairToTableIndex.ContainsKey(pos))
                {
                    _chairToTableIndex.Add(pos, i);
                }
            }
        }

        Debug.Log($"RegisterChairNodes �Ϸ�: �� ��ϵ� ���� �� = {_emptyChairsCheck.Count}");
    }
    #endregion
}