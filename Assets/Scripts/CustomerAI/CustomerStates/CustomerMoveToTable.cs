using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CustomerMoveToTable : ICustomerState
{
    #region 변수들
    List<Node> _path;           // A*로 계산된 경로를 저장할 리스트
    int _currentIndex;          // 현재 따라가고 있는 경로 인덱스

    Node _startNode;             //시작 노드

    //해금된 의자 리스트
    Dictionary<Vector2Int, Node> _emptyChairsCheck = new Dictionary<Vector2Int, Node>();

    //각 테이블안의 의자 노드 그리드
    List<Vector2Int>[] _chairPositions = new List<Vector2Int>[12];

    //테이블 관련 변수
    Dictionary<Vector2Int, int> _chairToTableIndex = new Dictionary<Vector2Int, int>();
    Table[] _tables = new Table[12];

    GameManager _gameManager;
    #endregion

    #region  Enter, Update, Exit문
    public void Enter(CustomerAI customer)
    {
        InitChairGridPos();
        RegisterChairNodes();
        InitTables();
        MoveCustomerToChair(customer);
    }

    public void Update(CustomerAI customer)
    {
        // 경로가 없거나 이미 도착했다면 다음 상태로 전환
        if (_path == null || _currentIndex >= _path.Count)
        {
            customer.SetState(new CustomerEating());
            return;
        }

        Node _targetNode = _path[_currentIndex];
        Vector3 _targetPos = _targetNode.transform.position;
        float _step = 5f * Time.deltaTime;

        customer.transform.position = Vector3.MoveTowards(customer.transform.position, _targetPos, _step);

        if (Vector3.Distance(customer.transform.position, _targetPos) < 0.1f)
        {
            if (_currentIndex == 0)
            {
                _startNode._isCustomerWaiting = false;   
            }

            _currentIndex++;
        }
    }

    public void Exit(CustomerAI customer) { }
    #endregion

    /// <summary>
    /// 테이블이 해금되어 있는지 체크하는 함수
    /// </summary>
    bool IsTableUnlocked(int tableIndex)
    {
        string tableKey = $"테이블{tableIndex + 1}";

        if (GameManager._instance._baseObjectDict[tableKey])
        {
            BaseObject tableObj = GameManager._instance._baseObjectDict[tableKey];
            Debug.Log($"테이블 {tableKey} 해금 상태: {tableObj._isActive}");
            return tableObj._isActive;
        }
        else
        {
            Debug.LogWarning($"테이블 {tableKey}이(가) _baseObjectDict에 없음");
            return false;
        }
    }

    /*
    bool IsTableUnlocked(int tableIndex)
    {
        string tableKey = $"테이블_{tableIndex + 1}";

        if (GameManager._instance._baseObjectDict[tableKey])
        {
            GameObject tableobj = GameManager._instance._baseObjectDict[tableKey].gameObject;

            Debug.Log($"테이블 {tableKey} 해금 상태: {tableobj._isActive}");
            return tableobj._isActive;
        }
        else
        {   
            Debug.LogWarning($"테이블 {tableKey}이(가) _baseObjectDict에 없음");
            return false;
        }
    }
    */

    /// <summary>
    /// 해금된 테이블 안의 의자만 모아 반환하는 함수
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
                        Debug.LogWarning($"GetAvailableChairNodes(): 빈 의자 체크 실패 - 위치 {chairPos}");
                    }
                }
            }
        }

        Debug.Log($"GetAvailableChairNodes 완료: 이동 가능한 의자 수 = {availableChairs.Count}");

        return availableChairs;
    }


    /// <summary>
    /// 손님이 이동할 목표 의자 노드를 선택해서 A* 경로를 만드는 함수
    /// </summary>
    void MoveCustomerToChair(CustomerAI customer)
    {
        List<Node> availableChairs = GetAvailableChairNodes();

        if (availableChairs.Count == 0)
        {
            Debug.LogWarning("MoveCustomerToChair: 이동 가능한 의자가 없음");
            return;
        }

        Node targetChairNode = availableChairs[Random.Range(0, availableChairs.Count)];
        _startNode = GetClosestNode(customer.transform.position);

        if (_startNode == null)
        {
            Debug.LogError("MoveCustomerToChair: 시작 노드가 없음!");
            return;
        }

        _path = AStarPathfinder.FindPath(_startNode, targetChairNode);

        if (_path == null || _path.Count == 0)
        {
            Debug.LogError("MoveCustomerToChair: 경로 계산 실패!");
            return;
        }

        _currentIndex = 0;
        Debug.Log($"MoveCustomerToChair: 경로 생성 성공! 총 경로 길이 = {_path.Count}");

        // 손님의 _table 할당
        if (_chairToTableIndex.TryGetValue(targetChairNode._gridPos, out int tableIndex))
        {
            customer._table = _tables[tableIndex];
            Debug.Log($"손님이 이동할 테이블: {tableIndex + 1}번 테이블");
        }
        else
        {
            Debug.LogWarning("MoveCustomerToChair: 의자에 해당하는 테이블을 찾을 수 없음");
        }
    }

    void InitTables()
    {
        for (int i = 0; i < _tables.Length; i++)
        {
            string tableName = $"테이블{i + 1}"; // "테이블1", "테이블2" 이런 식
            if (GameManager._instance._baseObjectDict.TryGetValue(tableName, out BaseObject tableObj))
            {
                _tables[i] = tableObj.GetComponent<Table>();

                if (_tables[i] == null)
                {
                    Debug.LogError($"{tableName}에 Table 스크립트가 없음!");
                }
            }
            else
            {
                Debug.LogWarning($"{tableName} 오브젝트를 찾을 수 없음.");
            }
        }
    }


    #region 가장 가까운 노드 찾기
    /// <summary>
    /// 주어진 위치에서 가장 가까운 유효한 Node를 찾아 반환
    /// </summary>
    /// <param name="pos">현재 손님의 위치</param>
    /// <returns>가장 가까운 Node</returns>
    Node GetClosestNode(Vector3 pos)
    {
        //초기값 최대로
        float _minDist = float.MaxValue;
        Node _closestNode = null;

        foreach (Node _node in NodeManager._instance._nodeList)
        {
            if (_node == null)
            {
                Debug.Log("null node 발견됨");
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
            Debug.LogError("GetClosestNode(): 유효한 노드가 없음");

        return _closestNode;
    }
    #endregion

    #region 의자 노드 좌표 등록 함수들
    /// <summary>
    /// 의자 노드의 각 좌표
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
    /// 의자 노드 그리드 좌표랑 등록시키기
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
                    Debug.LogError($"RegisterChairNodes(): Node가 null임! 위치: {pos}");
                    continue;
                }

                if (!_emptyChairsCheck.ContainsKey(pos))
                {
                    _emptyChairsCheck.Add(pos, _chairNode);
                }

                // 좌표 -> 테이블 번호 매핑
                if (!_chairToTableIndex.ContainsKey(pos))
                {
                    _chairToTableIndex.Add(pos, i);
                }
            }
        }
        Debug.Log($"RegisterChairNodes 완료: 총 등록된 의자 수 = {_emptyChairsCheck.Count}");
    }


    #endregion
}