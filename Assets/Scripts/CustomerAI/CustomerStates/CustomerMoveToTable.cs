using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMoveToTable : ICustomerState
{
    #region 변수들
    List<Node> _path;           // A*로 계산된 경로를 저장할 리스트
    int _currentIndex;          // 현재 따라가고 있는 경로 인덱스

    //해금된 의자 리스트
    Dictionary<Vector2Int, Node> _emptyChairsCheck = new Dictionary<Vector2Int, Node>();

    //각 테이블안의 의자 노드 그리드
    List<Vector2Int>[] _chairPositions = new List<Vector2Int>[12];
    #endregion


    #region  Enter, Update, Exit문
    public void Enter(CustomerAI customer)
    {
        InitChairGridPos();
        RegisterChairNodes();

        //해금된 의자 리스트 노드들 앉을 수 있는지 확인하기
        //1. 앉을 수 있으면 거기로 이동하게 할거임
        //2. 앉을 수 있는 자리 없으면 대기하기

        // 현재 위치에서 가장 가까운 시작 노드 찾기
        Node _startNode = GetClosestNode(customer.transform.position);

        //의자 좌표들 쫙 확인 후 앉을 수 있는 지 확인


        // 도착지 노드 설정 (예: 카운터 위치의 특정 좌표)
        //Node _goalNode = NodeManager._instance._nodeList[_counterNodeGridPos.x, _counterNodeGridPos.y];



        // A* 알고리즘으로 경로 계산
        _path = AStarPathfinder.FindPath(_startNode, _goalNode);
        _currentIndex = 0; // 경로 시작 인덱스 초기화
    }

    /// <summary>
    /// 상태 업데이트. 손님이 A* 경로를 따라 한 칸씩 이동함
    /// </summary>
    public void Update(CustomerAI customer)
    {
        // 경로가 없거나 이미 도착했다면 다음 상태로 전환
        if (_path == null || _currentIndex >= _path.Count)
        {
            //손님이 줄에 도착했는것
            if (_path != null)
            {
                _path[_currentIndex - 1]._isCustomerWaiting = true;
            }

            customer.SetState(new CustomerOrderAndWait()); // 다음 행동 상태로 전환 (주문 대기 등)
            return;
        }

        // 다음 이동 목표 노드 설정
        Node _targetNode = _path[_currentIndex];
        Vector3 _targetPos = _targetNode.transform.position;
        float _step = 5f * Time.deltaTime; // 프레임 기반 이동 거리 계산

        //앞에 노드에 손님이 줄서고 있다면?
        if (_targetNode._isCustomerWaiting)
        {
            if (_currentIndex != 0)
            {
                Node _currentNode = _path[_currentIndex - 1];

                // 현재 위치한 노드에 아무도 못오게 막기
                _currentNode._isCustomerWaiting = true;
            }
        }
        else
        {
            // 손님을 다음 노드 위치로 이동시킴
            customer.transform.position = Vector3.MoveTowards(customer.transform.position, _targetPos, _step);
            // 목표 위치에 거의 도착했다면 다음 노드로 전환
            if (Vector3.Distance(customer.transform.position, _targetPos) < 0.1f)
            {
                _currentIndex++;
            }
        }
    }

    public void Exit(CustomerAI customer) { }
    #endregion

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

                if (!_emptyChairsCheck.ContainsKey(pos))
                {
                    _emptyChairsCheck.Add(pos, _chairNode);
                }
            }
        }
    }
    #endregion
}