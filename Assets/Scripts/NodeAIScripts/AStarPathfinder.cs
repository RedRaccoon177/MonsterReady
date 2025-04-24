using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A* 알고리즘을 안전하게 멀티 캐릭터에 사용할 수 있도록 구현한 클래스
/// 각 호출마다 노드 상태(G, H, F, parent)를 별도로 관리하기 때문에 충돌 없음
/// 손님이 100명이 동시에 움직여도 각각 자기 길을 계산하게 된다
/// </summary>
public static class AStarPathfinder
{
    /// <summary>
    /// A* 계산 중에 사용하는 노드 전용 상태 클래스 (노드 자체에는 아무 정보도 저장하지 않음)
    /// G: 시작 노드로부터 현재 노드까지의 실제 거리 (지금까지 걸어온 거리)
    /// H: 현재 노드에서 도착 노드까지의 예상 거리 (휴리스틱)
    /// F: G + H (이 노드를 지나가는 총 예상 거리)
    /// parent: 경로를 따라가기 위한 부모 노드
    private class NodeDataFGH
    {
        public float _G;
        public float _H;
        public Node _parent;
        public float _F => _G + _H;
    }

    /// <summary>
    /// 실제로 길을 계산하는 메인 함수
    /// </summary>
    /// <param name="start">시작할 노드 (예: 손님이 서 있는 위치 근처 노드)</param>
    /// <param name="goal">도착할 노드 (예: 카운터 위치 근처 노드)</param>
    /// <returns>시작부터 도착까지 갈 수 있는 노드 리스트 (경로), 없으면 null</returns>
    public static List<Node> FindPath(Node start, Node goal)
    {
        // 노드 상태 정보들을 저장할 딕셔너리 (Node별로 각각 G/H/F/parent를 저장함)
        Dictionary<Node, NodeDataFGH> nodeDataMap = new Dictionary<Node, NodeDataFGH>();

        // 아직 방문하지 않은 노드 리스트 (탐색 후보)
        List<Node> openList = new List<Node>();

        // 이미 방문 완료된 노드 목록
        HashSet<Node> closedList = new HashSet<Node>();

        // 시작 노드 초기 설정
        nodeDataMap[start] = new NodeDataFGH
        {
            _G = 0, // 시작이니까 0부터 시작
            _H = GetDistance(start, goal), // 도착까지 얼마나 남았는지 예측
            _parent = null
        };
        openList.Add(start);

        // 본격적으로 경로 탐색 시작
        while (openList.Count > 0)
        {
            // 현재까지 가장 유망한 노드(F 값이 가장 낮은 노드)를 찾는다
            Node current = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (nodeDataMap[openList[i]]._F < nodeDataMap[current]._F)
                    current = openList[i];
            }

            // 도착 노드에 도달했으면 경로를 역추적해서 반환
            if (current == goal)
                return RetracePath(start, goal, nodeDataMap);

            // 현재 노드를 탐색 완료 처리
            openList.Remove(current);
            closedList.Add(current);

            // 현재 노드에 연결된 모든 이웃 노드들을 확인
            foreach (Node neighbor in current._connectionNodes)
            {
                if (closedList.Contains(neighbor))
                    continue; // 이미 본 노드는 다시 보지 않음

                // 지금까지 걸어온 거리 + 이웃 노드까지 거리
                float tentativeG = nodeDataMap[current]._G + Vector3.Distance(current.transform.position, neighbor.transform.position);

                // 처음 방문하거나 더 짧은 경로 발견 시 갱신
                if (!nodeDataMap.ContainsKey(neighbor) || tentativeG < nodeDataMap[neighbor]._G)
                {
                    if (!nodeDataMap.ContainsKey(neighbor))
                        nodeDataMap[neighbor] = new NodeDataFGH();

                    nodeDataMap[neighbor]._G = tentativeG;
                    nodeDataMap[neighbor]._H = GetDistance(neighbor, goal);
                    nodeDataMap[neighbor]._parent = current;

                    if (!openList.Contains(neighbor))
                        openList.Add(neighbor);
                }
            }
        }

        // 경로를 찾을 수 없는 경우 (예: 벽에 막힌 경우)
        return null;
    }

    /// <summary>
    /// 도착 노드에서 시작 노드까지 역으로 따라가며 경로 리스트를 만드는 함수
    /// 실제 경로: 도착 -> 부모 -> 부모의 부모 -> ... -> 시작 으로 타고 가기 때문에
    /// 최종적으로는 Reverse()를 사용해 반대 방향으로 바꿔야 함
    /// </summary>
    static List<Node> RetracePath(Node start, Node goal, Dictionary<Node, NodeDataFGH> map)
    {
        List<Node> path = new List<Node>();
        Node current = goal;

        while (current != start)
        {
            path.Add(current);
            current = map[current]._parent; // 경로의 부모를 따라 올라감
        }

        path.Reverse(); // 시작 → 도착 순서로 뒤집기
        return path;
    }

    /// <summary>
    /// _H -> 휴리스틱 계산 (현재 노드에서 도착 노드까지의 예상 거리)
    /// A*에서 핵심적인 추정치로, 이 값이 작을수록 더 가까워 보인다
    /// Vector3.Distance를 쓰면 유클리디안 거리(대각선 포함 직선 거리)를 기준으로 한다
    /// </summary>
    static float GetDistance(Node a, Node b)
    {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }
}
