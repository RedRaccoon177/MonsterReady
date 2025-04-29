using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 손님이 음식을 수령받고 의자까지 이동하는 상태 클래스
/// </summary>
public class CustomerMoveToTable : ICustomerState
{
    #region 변수들
    List<Node> _path;       // A*로 계산된 경로를 저장할 리스트
    int _currentIndex;      // 현재 따라가고 있는 경로 인덱스

    // 이동 시작 지점 노드
    Node _startNode;                   

    // 해금된 의자 체크용
    Dictionary<Vector2Int, Node> _emptyChairsCheck = new Dictionary<Vector2Int, Node>();

    // 테이블 별 의자 위치 목록
    List<Vector2Int>[] _chairPositions = new List<Vector2Int>[12];

    // 의자 위치 -> 테이블 번호 매핑
    Dictionary<Vector2Int, int> _chairToTableIndex = new Dictionary<Vector2Int, int>();

    // 테이블 오브젝트 배열
    Table[] _tables = new Table[12];
    #endregion

    #region  Enter, Update, Exit문
    public void Enter(CustomerAI customer)
    {
        InitChairGridPos();             // 의자 위치 초기화
        RegisterChairNodes();           // 의자 노드 등록
        InitTables();                   // 테이블 객체 초기화
        MoveCustomerToChair(customer);  // 손님 이동 시작
    }

    public void Update(CustomerAI customer)
    {
        // 경로가 없거나 이미 도착했다면 음식 먹는 상태로 전환
        if (_path == null || _currentIndex >= _path.Count)
        {
            customer.SetState(new CustomerEating());
            return;
        }

        Node _targetNode = _path[_currentIndex];
        Vector3 _targetPos = _targetNode.transform.position;
        float _step = 5f * Time.deltaTime; // 이동 속도

        customer.transform.position = Vector3.MoveTowards(customer.transform.position, _targetPos, _step);

        if (Vector3.Distance(customer.transform.position, _targetPos) < 0.1f)
        {
            if (_currentIndex == 0)
            {
                _startNode._isCustomerWaiting = false;   // 출발 노드에 대기중 손님 없음
            }
            _currentIndex++; // 다음 경로로 이동
        }
    }

    public void Exit(CustomerAI customer) { }
    #endregion

    #region 의자 관련 초기화 및 설정 함수들
    /// <summary>
    /// 테이블이 해금되어 있는지 확인
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

    /// <summary>
    /// 해금된 테이블 내 사용 가능한 의자 노드 리스트를 가져옴
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
    /// 손님의 이동 시작 (목표 의자 선택 + 경로 찾기)
    /// </summary>
    void MoveCustomerToChair(CustomerAI customer)
    {
        List<Node> availableChairs = GetAvailableChairNodes();

        if (availableChairs.Count == 0)
        {
            Debug.LogWarning("MoveCustomerToChair: 이동 가능한 의자가 없음");
            return;
        }

        Node targetChairNode = availableChairs[Random.Range(0, availableChairs.Count)]; // 랜덤 의자 선택
        _startNode = GetClosestNode(customer.transform.position);                       // 현재 위치 기준 가장 가까운 노드 찾기

        if (_startNode == null)
        {
            Debug.LogError("MoveCustomerToChair: 시작 노드가 없음!");
            return;
        }

        _path = AStarPathfinder.FindPath(_startNode, targetChairNode); // A* 경로 찾기

        if (_path == null || _path.Count == 0)
        {
            Debug.LogError("MoveCustomerToChair: 경로 계산 실패!");
            return;
        }

        _currentIndex = 0;
        Debug.Log($"MoveCustomerToChair: 경로 생성 성공! 총 경로 길이 = {_path.Count}");

        // 이동하는 테이블 저장
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

    /// <summary>
    /// 테이블 오브젝트 초기화
    /// </summary>
    void InitTables()
    {
        for (int i = 0; i < _tables.Length; i++)
        {
            string tableName = $"테이블{i + 1}";

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
    #endregion

    #region 가장 가까운 노드 찾기
    /// <summary>
    /// 현재 위치에서 가장 가까운 이동 가능한 Node를 반환
    /// </summary>
    Node GetClosestNode(Vector3 pos)
    {
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
                continue; // 이동 불가능한 노드는 스킵
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

    #region 의자 좌표 설정 및 등록
    /// <summary>
    /// 각 테이블별 의자들의 그리드 위치 설정
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
    /// 의자 노드들을 등록하고, 의자 위치와 테이블 매핑
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