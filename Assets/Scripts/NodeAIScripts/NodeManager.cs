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

    // Node�� �����ϴ� 2���� �迭 (Z��, X�� ������ ����)
    Node[,] _nodeList = new Node[23, 25];

    // ��� ������ (�����Ϳ��� ���� �ʼ�)
    [SerializeField] public Node _nodePrefab;

    // ��带 ���� �� �ӽ÷� �����ϴ� ���� (�ʱ�ȭ �뵵)
    [SerializeField] public Node _nodeScript;

    // ���� ���� �ϴ� ���� ��ġ ������
    int _minX = -12;
    int _minY = -1;

    private void Awake()
    {
        // �̱��� ���� ����
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        // ���� ���� �� �� ����
        CreateNodeMap();
    }

    /// <summary>
    /// Node �������� ������ �������� ��ġ�Ͽ�
    /// ���� ������ ���� �����ϰ�, �� ������ �迭�� ����
    /// </summary>
    public void CreateNodeMap()
    {
        for (int i = 0; i < _nodeList.GetLength(0); i++) // Z�� ���� �ݺ�
        {
            for (int j = 0; j < _nodeList.GetLength(1); j++) // X�� ���� �ݺ�
            {
                // ����� ���� �� ��ġ ���
                Vector3 pos = new Vector3(_minX + j * 2, 0f, _minY + i * 2);

                // ��� ������ ���� �� ��ġ
                Node node = Instantiate(_nodePrefab, pos, Quaternion.identity);

                // ��忡 �ڽ��� �׸��� ��ǥ�� ����
                node.Init(new Vector2Int(j, i));

                // �迭�� �ش� ��� ����
                _nodeList[i, j] = node;
            }
        }
    }
}
