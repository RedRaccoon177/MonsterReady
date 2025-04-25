using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// ���� ��� ���� �����ϴ� Node���� �����ϰ�,
/// ��ü �� ������ 2���� �迭�� �����ϴ� �Ŵ��� Ŭ����
/// </summary>
public class NodeManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ� (�������� ���� �����ϵ��� ����)
    public static NodeManager _instance;

    [SerializeField] public int _XWidth = 23;
    [SerializeField] public int _YLength = 25;

    // Node�� �����ϴ� 2���� �迭 (Z��, X�� ������ ����)
    public Node[,] _nodeList;

    // ��� ������ (�����Ϳ��� ���� �ʼ�)
    [SerializeField] public Node _nodePrefab;

    // ���� ���� �ϴ� ���� ��ġ ������
    int _minX = -12;
    int _minY = -1;

    void Awake()
    {
        // �̱��� ���� ����
        if (_instance == null)
        {
            _instance = this;
        }

        _nodeList = new Node[_XWidth, _YLength];
        CreateNodeMap();
    }

    void Start()
    {
        // ���� ���� �� �� ����
    }

    /// <summary>
    /// Node �������� ������ �������� ��ġ�Ͽ�
    /// ���� ������ ���� �����ϰ�, �� ������ �迭�� ����
    /// </summary>
    public void CreateNodeMap()
    {
        for (int i = 0; i < _nodeList.GetLength(0); i++)
        {
            for (int j = 0; j < _nodeList.GetLength(1); j++)
            {
                //TODO: ���� ������Ʈ Ǯ������ ���
                Node node = Instantiate(_nodePrefab, new Vector3(_minX + j * 2, 0, _minY + i * 2), Quaternion.identity);

                node.Init(new Vector2Int(i, j)); // ��ǥ ����

                _nodeList[i, j] = node;
            }
        }
        StartCoroutine(DelayedConnection());
    }

    IEnumerator DelayedConnection()
    {
        yield return null;

        // ���� ���� ����
        for (int i = 0; i < _nodeList.GetLength(0); i++)
        {
            for (int j = 0; j < _nodeList.GetLength(1); j++)
            {
                _nodeList[i, j].ConnectionNodes();
            }
        }
    }
    public Node GetNearestNodeOptimized(Vector3 worldPos)
    {
        // ��ǥ �������� �ε��� ����
        int x = Mathf.RoundToInt((worldPos.z - _minY) / 2f);
        int y = Mathf.RoundToInt((worldPos.x - _minX) / 2f);

        // ���� Ŭ����
        x = Mathf.Clamp(x, 0, _nodeList.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, _nodeList.GetLength(1) - 1);

        return _nodeList[x, y];
    }
}
