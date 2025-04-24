using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A* �˰����� �����ϰ� ��Ƽ ĳ���Ϳ� ����� �� �ֵ��� ������ Ŭ����
/// �� ȣ�⸶�� ��� ����(G, H, F, parent)�� ������ �����ϱ� ������ �浹 ����
/// �մ��� 100���� ���ÿ� �������� ���� �ڱ� ���� ����ϰ� �ȴ�
/// </summary>
public static class AStarPathfinder
{
    /// <summary>
    /// A* ��� �߿� ����ϴ� ��� ���� ���� Ŭ���� (��� ��ü���� �ƹ� ������ �������� ����)
    /// G: ���� ���κ��� ���� �������� ���� �Ÿ� (���ݱ��� �ɾ�� �Ÿ�)
    /// H: ���� ��忡�� ���� �������� ���� �Ÿ� (�޸���ƽ)
    /// F: G + H (�� ��带 �������� �� ���� �Ÿ�)
    /// parent: ��θ� ���󰡱� ���� �θ� ���
    class NodeDataFGH
    {
        public float _G;
        public float _H;
        public Node _parent;
        public float _F => _G + _H;
    }

    /// <summary>
    /// ������ ���� ����ϴ� ���� �Լ�
    /// </summary>
    /// <param name="_start">������ ��� (��: �մ��� �� �ִ� ��ġ ��ó ���)</param>
    /// <param name="_goal">������ ��� (��: ī���� ��ġ ��ó ���)</param>
    /// <returns>���ۺ��� �������� �� �� �ִ� ��� ����Ʈ (���), ������ null</returns>
    public static List<Node> FindPath(Node _start, Node _goal)
    {
        // ��� ���� �������� ������ ��ųʸ� (Node���� ���� G/H/F/parent�� ������)
        Dictionary<Node, NodeDataFGH> _nodeDataMap = new Dictionary<Node, NodeDataFGH>();

        // ���� �湮���� ���� ��� ����Ʈ (Ž�� �ĺ�)
        List<Node> _openList = new List<Node>();

        // �̹� �湮 �Ϸ�� ��� ���
        HashSet<Node> _closedList = new HashSet<Node>();

        // ���� ��� �ʱ� ����
        _nodeDataMap[_start] = new NodeDataFGH
        {
            _G = 0, // �����̴ϱ� 0���� ����
            _H = GetDistance(_start, _goal), // �������� �󸶳� ���Ҵ��� ����
            _parent = null
        };
        _openList.Add(_start);

        // ���������� ��� Ž�� ����
        while (_openList.Count > 0)
        {
            // ������� ���� ������ ���(F ���� ���� ���� ���)�� ã�´�
            Node _current = _openList[0];
            for (int i = 1; i < _openList.Count; i++)
            {
                if (_nodeDataMap[_openList[i]]._F < _nodeDataMap[_current]._F)
                    _current = _openList[i];
            }

            // ���� ��忡 ���������� ��θ� �������ؼ� ��ȯ
            if (_current == _goal)
                return RetracePath(_start, _goal, _nodeDataMap);

            // ���� ��带 Ž�� �Ϸ� ó��
            _openList.Remove(_current);
            _closedList.Add(_current);

            // ���� ��忡 ����� ��� �̿� ������ Ȯ��
            foreach (Node _neighbor in _current._connectionNodes)
            {
                if (_closedList.Contains(_neighbor))
                    continue; // �̹� �� ���� �ٽ� ���� ����

                // ���ݱ��� �ɾ�� �Ÿ� + �̿� ������ �Ÿ�
                float _tentativeG = _nodeDataMap[_current]._G + Vector3.Distance(_current.transform.position, _neighbor.transform.position);

                // ó�� �湮�ϰų� �� ª�� ��� �߰� �� ����
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

        // ��θ� ã�� �� ���� ��� (��: ���� ���� ���)
        return null;
    }

    /// <summary>
    /// ���� ��忡�� ���� ������ ������ ���󰡸� ��� ����Ʈ�� ����� �Լ�
    /// ���� ���: ���� -> �θ� -> �θ��� �θ� -> ... -> ���� ���� Ÿ�� ���� ������
    /// ���������δ� Reverse()�� ����� �ݴ� �������� �ٲ�� ��
    /// </summary>
    static List<Node> RetracePath(Node _start, Node _goal, Dictionary<Node, NodeDataFGH> _map)
    {
        List<Node> _path = new List<Node>();
        Node _current = _goal;

        while (_current != _start)
        {
            _path.Add(_current);
            _current = _map[_current]._parent; // ����� �θ� ���� �ö�
        }

        _path.Reverse(); // ���� �� ���� ������ ������
        return _path;
    }

    /// <summary>
    /// _H -> �޸���ƽ ��� (���� ��忡�� ���� �������� ���� �Ÿ�)
    /// A*���� �ٽ����� ����ġ��, �� ���� �������� �� ����� ���δ�
    /// Vector3.Distance�� ���� ��Ŭ����� �Ÿ�(�밢�� ���� ���� �Ÿ�)�� �������� �Ѵ�
    /// </summary>
    static float GetDistance(Node _a, Node _b)
    {
        return Vector3.Distance(_a.transform.position, _b.transform.position);
    }
}
