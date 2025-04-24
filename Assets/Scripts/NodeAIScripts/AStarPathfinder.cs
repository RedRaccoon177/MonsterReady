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
    private class NodeDataFGH
    {
        public float _G;
        public float _H;
        public Node _parent;
        public float _F => _G + _H;
    }

    /// <summary>
    /// ������ ���� ����ϴ� ���� �Լ�
    /// </summary>
    /// <param name="start">������ ��� (��: �մ��� �� �ִ� ��ġ ��ó ���)</param>
    /// <param name="goal">������ ��� (��: ī���� ��ġ ��ó ���)</param>
    /// <returns>���ۺ��� �������� �� �� �ִ� ��� ����Ʈ (���), ������ null</returns>
    public static List<Node> FindPath(Node start, Node goal)
    {
        // ��� ���� �������� ������ ��ųʸ� (Node���� ���� G/H/F/parent�� ������)
        Dictionary<Node, NodeDataFGH> nodeDataMap = new Dictionary<Node, NodeDataFGH>();

        // ���� �湮���� ���� ��� ����Ʈ (Ž�� �ĺ�)
        List<Node> openList = new List<Node>();

        // �̹� �湮 �Ϸ�� ��� ���
        HashSet<Node> closedList = new HashSet<Node>();

        // ���� ��� �ʱ� ����
        nodeDataMap[start] = new NodeDataFGH
        {
            _G = 0, // �����̴ϱ� 0���� ����
            _H = GetDistance(start, goal), // �������� �󸶳� ���Ҵ��� ����
            _parent = null
        };
        openList.Add(start);

        // ���������� ��� Ž�� ����
        while (openList.Count > 0)
        {
            // ������� ���� ������ ���(F ���� ���� ���� ���)�� ã�´�
            Node current = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (nodeDataMap[openList[i]]._F < nodeDataMap[current]._F)
                    current = openList[i];
            }

            // ���� ��忡 ���������� ��θ� �������ؼ� ��ȯ
            if (current == goal)
                return RetracePath(start, goal, nodeDataMap);

            // ���� ��带 Ž�� �Ϸ� ó��
            openList.Remove(current);
            closedList.Add(current);

            // ���� ��忡 ����� ��� �̿� ������ Ȯ��
            foreach (Node neighbor in current._connectionNodes)
            {
                if (closedList.Contains(neighbor))
                    continue; // �̹� �� ���� �ٽ� ���� ����

                // ���ݱ��� �ɾ�� �Ÿ� + �̿� ������ �Ÿ�
                float tentativeG = nodeDataMap[current]._G + Vector3.Distance(current.transform.position, neighbor.transform.position);

                // ó�� �湮�ϰų� �� ª�� ��� �߰� �� ����
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

        // ��θ� ã�� �� ���� ��� (��: ���� ���� ���)
        return null;
    }

    /// <summary>
    /// ���� ��忡�� ���� ������ ������ ���󰡸� ��� ����Ʈ�� ����� �Լ�
    /// ���� ���: ���� -> �θ� -> �θ��� �θ� -> ... -> ���� ���� Ÿ�� ���� ������
    /// ���������δ� Reverse()�� ����� �ݴ� �������� �ٲ�� ��
    /// </summary>
    static List<Node> RetracePath(Node start, Node goal, Dictionary<Node, NodeDataFGH> map)
    {
        List<Node> path = new List<Node>();
        Node current = goal;

        while (current != start)
        {
            path.Add(current);
            current = map[current]._parent; // ����� �θ� ���� �ö�
        }

        path.Reverse(); // ���� �� ���� ������ ������
        return path;
    }

    /// <summary>
    /// _H -> �޸���ƽ ��� (���� ��忡�� ���� �������� ���� �Ÿ�)
    /// A*���� �ٽ����� ����ġ��, �� ���� �������� �� ����� ���δ�
    /// Vector3.Distance�� ���� ��Ŭ����� �Ÿ�(�밢�� ���� ���� �Ÿ�)�� �������� �Ѵ�
    /// </summary>
    static float GetDistance(Node a, Node b)
    {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }
}
