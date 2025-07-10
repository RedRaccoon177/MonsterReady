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

    // 자리 대기 변수들
    float _checkDelay = 1f;
    float _checkTimer = 0f;
    bool _waitingForSeat = false;
    #endregion

    #region  Enter, Update, Exit문
    public void Enter(CustomerAI customer)
    {
        InitChairGridPos();             // 의자 위치 초기화
        RegisterChairNodes();           // 의자 노드 등록
        InitTables();                   // 테이블 객체 초기화
        MoveCustomerToChair(customer);  // 손님 이동 시작

        customer.SetExclusiveAnimation("IsCarryingAndWalking");
    }

    public void Update(CustomerAI customer)
    {
        // 1. 자리 대기 중이면 자리 체크 로직 실행
        if (_waitingForSeat)
        {
            HandleSeatWaiting(customer);
            return;
        }

        // 2. 경로 따라 이동
        MoveAlongPath(customer);
    }

    public void Exit(CustomerAI customer) { }
    #endregion

    #region 실질적인 움직임 함수들
    /// <summary>
    /// 자리 대기 상태 처리:
    /// 일정 시간마다 빈 자리 있는지 확인, 있으면 이동 시작
    /// </summary>
    void HandleSeatWaiting(CustomerAI customer)
    {
        _checkTimer += Time.deltaTime;

        if (_checkTimer >= _checkDelay)
        {
            _checkTimer = 0f;

            List<Node> availableChairs = GetAvailableChairNodes();
            if (availableChairs.Count > 0)
            {
                Debug.Log("자리가 생겨서 이동 재시도!");
                _waitingForSeat = false;
                MoveCustomerToChair(customer); // 경로 재계산
            }
        }
    }

    /// <summary>
    /// A* 경로를 따라 이동하고, 회전 처리:
    /// 목적지에 도달하면 상태 전환
    /// </summary>
    void MoveAlongPath(CustomerAI customer)
    {
        // 경로가 없거나 도착 완료 → 다음 상태로 전환
        if (_path == null || _currentIndex >= _path.Count)
        {
            customer.SetState(new CustomerEating());
            return;
        }

        // 현재 목표 노드 정보
        Node _targetNode = _path[_currentIndex];
        Vector3 _targetPos = _targetNode.transform.position;
        float _step = 5f * Time.deltaTime; // 이동 속도 계산

        // 1. 현재 위치에서 목표 위치로 이동
        customer.transform.position = Vector3.MoveTowards(customer.transform.position, _targetPos, _step);

        // 2. 이동 방향으로 부드럽게 회전
        Vector3 direction = (_targetPos - customer.transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            customer.transform.rotation = Quaternion.Slerp(customer.transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        // 3. 목표 위치에 거의 도착했으면 다음 노드로 진행
        if (Vector3.Distance(customer.transform.position, _targetPos) < 0.1f)
        {
            if (_currentIndex == 0)
            {
                // 출발 노드의 손님 상태 해제
                _startNode._isCustomerThere = false;
            }

            _currentIndex++;

            // 목적이데 도착했는지 확인 (a* 알고리즘 활용)
            if (_currentIndex >= _path.Count)
            {
                // 현재 앉아있는 의자의 Grid좌표 가져오기
                Vector2Int chairPos = customer._currentChairNode._gridPos;

                // 특정 의자들만 반대 방향으로 회전
                if (
                    chairPos == new Vector2Int(15, 10) || chairPos == new Vector2Int(12, 10) ||
                    chairPos == new Vector2Int(9, 10) || chairPos == new Vector2Int(8, 10) ||
                    chairPos == new Vector2Int(15, 14) || chairPos == new Vector2Int(12, 14) ||
                    chairPos == new Vector2Int(9, 15) || chairPos == new Vector2Int(8, 15) ||
                    chairPos == new Vector2Int(6, 3) || chairPos == new Vector2Int(9, 3) ||
                    chairPos == new Vector2Int(12, 3)
                )
                {
                    // 월드 좌표 기준 y축 -90도 (카운터에서 바라본 입구 기준 왼쪽)
                    customer.transform.rotation = Quaternion.Euler(0, -90f, 0);
                }
                else if (chairPos == new Vector2Int(5, 16) || chairPos == new Vector2Int(5, 12) ||
                    chairPos == new Vector2Int(5, 8))
                {
                    // 월드 좌표 기준 y축 0도 (동일 기준 정면)
                    customer.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    // 월드 좌표 기준 y축 90도 (동일 기준 우측)
                    customer.transform.rotation = Quaternion.Euler(0, 90f, 0);
                }
            }
        }
    }
    #endregion

    #region 의자 관련 초기화 및 설정 함수들
    /// <summary>
    /// 테이블이 해금되어 있는지 확인
    /// </summary>
    bool IsTableUnlocked(int tableIndex)
    {
        string tableKey = $"테이블{tableIndex + 1}";

        if (GameManager._instance._baseObjectDict.TryGetValue(tableKey, out BaseObject tableObj))
        {
            Debug.Log($"테이블 {tableKey} 해금 상태: {tableObj._isActive}");
            return tableObj._isActive;
        }
        else
        {
            Debug.LogWarning($"[IsTableUnlocked] {tableKey} 키가 _baseObjectDict에 없음");
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
                        if (chairNode._isWalkale)
                        {
                            availableChairs.Add(chairNode);
                        }
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
            Debug.Log("자리가 없어서 기다리는 중...");
            _waitingForSeat = true;
            _checkTimer = 0f;
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

        // 현재 앉고자 하는 의자 위치 노드 정보 삽입
        customer._currentChairNode = targetChairNode;
        // 노드 비활성호 다른 손님 막기
        targetChairNode._isWalkale = false;

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
        _chairPositions[9] = new List<Vector2Int> { new Vector2Int(12, 3) };
        _chairPositions[10] = new List<Vector2Int> { new Vector2Int(9, 3) };
        _chairPositions[11] = new List<Vector2Int> { new Vector2Int(6, 3) };
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