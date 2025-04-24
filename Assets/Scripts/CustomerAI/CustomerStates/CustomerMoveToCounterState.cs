using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerMoveToCounterState : ICustomerState
{
    List<Node> _path;           // A*로 계산된 경로를 저장할 리스트
    int _currentIndex;          // 현재 따라가고 있는 경로 인덱스

    /// <summary>
    /// 상태에 진입할 때 호출됨. 시작 노드와 도착 노드를 기준으로 A* 경로를 계산함
    /// </summary>
    public void Enter(CustomerAI customer)
    {
        // 현재 위치에서 가장 가까운 시작 노드 찾기
        Node start = GetClosestNode(customer.transform.position);

        // 도착지 노드 설정 (예: 카운터 위치의 특정 좌표)
        Node goal = NodeManager._instance._nodeList[15, 5];

        if (start == null || goal == null)
            return;

        // A* 알고리즘으로 경로 계산
        _path = AStarPathfinder.FindPath(start, goal);
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
            customer.SetState(new CustomerOrderAndWait()); // 다음 행동 상태로 전환 (주문 대기 등)
            return;
        }

        // 다음 이동 목표 노드 설정
        Node targetNode = _path[_currentIndex];
        Vector3 targetPos = targetNode.transform.position;
        float step = 3f * Time.deltaTime; // 프레임 기반 이동 거리 계산

        // 손님을 다음 노드 위치로 이동시킴
        customer.transform.position = Vector3.MoveTowards(customer.transform.position, targetPos, step);

        // 목표 위치에 거의 도착했다면 다음 노드로 전환
        if (Vector3.Distance(customer.transform.position, targetPos) < 0.1f)
        {
            _currentIndex++;
        }
    }

    /// <summary>
    /// 상태 종료 시 호출. 현재는 처리 없음
    /// </summary>
    public void Exit(CustomerAI customer) { }

    /// <summary>
    /// 주어진 위치에서 가장 가까운 유효한 Node를 찾아 반환
    /// </summary>
    /// <param name="pos">현재 손님의 위치</param>
    /// <returns>가장 가까운 Node</returns>
    private Node GetClosestNode(Vector3 pos)
    {
        float minDist = float.MaxValue;
        Node closest = null;

        foreach (Node node in NodeManager._instance._nodeList)
        {
            if (node == null)
            {
                Debug.Log("null node 발견됨");
                continue;
            }

            if (!node._isWalkale)
            {
                Debug.DrawLine(pos, node.transform.position, Color.red, 2f); // 빨간선
                continue;
            }

            Debug.DrawLine(pos, node.transform.position, Color.green, 2f); // 유효한 노드: 초록선

            float dist = Vector3.Distance(pos, node.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = node;
            }
        }

        if (closest == null)
            Debug.LogError("GetClosestNode(): 유효한 노드가 없음");

        return closest;
    }

}
