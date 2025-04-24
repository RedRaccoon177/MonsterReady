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
    class NodeDataFGH
    {
        public float _G;
        public float _H;
        public Node _parent;
        public float _F => _G + _H;
    }

    /// <summary>
    /// 실제로 길을 계산하는 메인 함수
    /// </summary>
    /// <param name="_start">시작할 노드 (예: 손님이 서 있는 위치 근처 노드)</param>
    /// <param name="_goal">도착할 노드 (예: 카운터 위치 근처 노드)</param>
    /// <returns>시작부터 도착까지 갈 수 있는 노드 리스트 (경로), 없으면 null</returns>
    public static List<Node> FindPath(Node _start, Node _goal)
    {
        // 노드 상태 정보들을 저장할 딕셔너리 (Node별로 각각 G/H/F/parent를 저장함)
        Dictionary<Node, NodeDataFGH> _nodeDataMap = new Dictionary<Node, NodeDataFGH>();

        // 아직 방문하지 않은 노드 리스트 (탐색 후보)
        List<Node> _openList = new List<Node>();

        // 이미 방문 완료된 노드 목록
        HashSet<Node> _closedList = new HashSet<Node>();

        // 시작 노드 초기 설정
        _nodeDataMap[_start] = new NodeDataFGH
        {
            _G = 0, // 시작이니까 0부터 시작
            _H = GetDistance(_start, _goal), // 도착까지 얼마나 남았는지 예측
            _parent = null
        };
        _openList.Add(_start);

        // 본격적으로 경로 탐색 시작
        while (_openList.Count > 0)
        {
            // 현재까지 가장 유망한 노드(F 값이 가장 낮은 노드)를 찾는다
            Node _current = _openList[0];
            for (int i = 1; i < _openList.Count; i++)
            {
                if (_nodeDataMap[_openList[i]]._F < _nodeDataMap[_current]._F)
                    _current = _openList[i];
            }

            // 도착 노드에 도달했으면 경로를 역추적해서 반환
            if (_current == _goal)
                return RetracePath(_start, _goal, _nodeDataMap);

            // 현재 노드를 탐색 완료 처리
            _openList.Remove(_current);
            _closedList.Add(_current);

            // 현재 노드에 연결된 모든 이웃 노드들을 확인
            foreach (Node _neighbor in _current._connectionNodes)
            {
                if (_closedList.Contains(_neighbor))
                    continue; // 이미 본 노드는 다시 보지 않음

                // 지금까지 걸어온 거리 + 이웃 노드까지 거리
                float _tentativeG = _nodeDataMap[_current]._G + Vector3.Distance(_current.transform.position, _neighbor.transform.position);

                // 처음 방문하거나 더 짧은 경로 발견 시 갱신
                if (!_nodeDataMap.ContainsKey(_neighbor) || _tentativeG < _nodeDataMap[_neighbor]._G)
                {
                    if (!_nodeDataMap.ContainsKey(_neighbor))
                        _nodeDataMap[_neighbor] = new NodeDataFGH();

                    _nodeDataMap[_neighbor]._G = _tentativeG;
                    _nodeDataMap[_neighbor]._H = GetDistance(_neighbor, _goal);
                    _nodeDataMap[_neighbor]._parent = _current;

                    if (!_openList.Contains(_neighbor))
                        _openList.Add(_neighbor);
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
    static List<Node> RetracePath(Node _start, Node _goal, Dictionary<Node, NodeDataFGH> _map)
    {
        List<Node> _path = new List<Node>();
        Node _current = _goal;

        while (_current != _start)
        {
            _path.Add(_current);
            _current = _map[_current]._parent; // 경로의 부모를 따라 올라감
        }

        _path.Reverse(); // 시작 → 도착 순서로 뒤집기
        return _path;
    }

    /// <summary>
    /// _H -> 휴리스틱 계산 (현재 노드에서 도착 노드까지의 예상 거리)
    /// A*에서 핵심적인 추정치로, 이 값이 작을수록 더 가까워 보인다
    /// Vector3.Distance를 쓰면 유클리디안 거리(대각선 포함 직선 거리)를 기준으로 한다
    /// </summary>
    static float GetDistance(Node _a, Node _b)
    {
        return Vector3.Distance(_a.transform.position, _b.transform.position);
    }
}
