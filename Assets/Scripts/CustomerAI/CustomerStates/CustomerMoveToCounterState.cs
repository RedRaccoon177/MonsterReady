using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMoveToCounterState : ICustomerState
{
    #region 변수들
    List<Node> _path;           // A*로 계산된 경로를 저장할 리스트
    int _currentIndex;          // 현재 따라가고 있는 경로 인덱스
    
    //최종 목적지인 카운터 위치
    Vector2Int _counterNodeGridPos = new Vector2Int(15, 5);
    #endregion

    #region  Enter, Update, Exit문
    public void Enter(CustomerAI customer)
    {
        // 현재 위치에서 가장 가까운 시작 노드 찾기
        Node _startNode = GetClosestNode(customer.transform.position);

        // 도착지 노드 설정 (예: 카운터 위치의 특정 좌표)
        Node _goalNode = NodeManager._instance._nodeList[_counterNodeGridPos.x, _counterNodeGridPos.y];

        if (_startNode == null || _goalNode == null)
            return;

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
            if(_path != null)
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
        if(_targetNode._isCustomerWaiting)
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
}
