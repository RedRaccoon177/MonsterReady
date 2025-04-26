using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGoingHome : ICustomerState
{
    List<Node> _path;       // A* 경로 저장
    int _currentIndex;      // 현재 목표 노드 인덱스
    Vector2Int _exitNodeGridPos = new Vector2Int(0, 0); // 출구 위치 (예: 0,0) ★출구 좌표 설정 필요

    public void Enter(CustomerAI customer)
    {
        // 현재 위치에서 가장 가까운 시작 노드 찾기
        Node _startNode = GetClosestNode(customer.transform.position);

        // 도착 지점 노드 설정 (출구)
        Node _goalNode = NodeManager._instance._nodeList[_exitNodeGridPos.x, _exitNodeGridPos.y];

        if (_startNode == null || _goalNode == null)
        {
            Debug.LogError("CustomerGoingHome: 시작 노드 또는 목표 노드를 찾을 수 없습니다.");
            return;
        }

        // A*로 경로 계산
        _path = AStarPathfinder.FindPath(_startNode, _goalNode);
        _currentIndex = 0;
    }

    public void Update(CustomerAI customer)
    {
        if (_path == null || _currentIndex >= _path.Count)
        {
            // 도착 완료 → 손님 제거
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

    // 현재 위치에서 가장 가까운 노드를 찾는 함수
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
            Debug.LogError("GetClosestNode(): 유효한 노드를 찾지 못했습니다.");

        return _closestNode;
    }
}
