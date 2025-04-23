using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // �̱��� �ν��Ͻ� (���� ������ ���� ���� ����)
    public static NodeManager _instance;

    // 2���� ��� �迭 [��, ��] = [z, x]
    Node[,] nodeList = new Node[23, 25];

    // ��� ������ ���� (�����Ϳ� ���� �ʿ�)
    [SerializeField] public Node nodePrefab;

    // �ν��Ͻ�ȭ�� ��忡 �����ϱ� ���� �ӽ� ����
    [SerializeField] public Node nodeScript;

    // �� ���� �ϴ� ���� ��ġ ������
    int minX = -12;
    int minY = -1;

    private void Awake()
    {
        // �̱��� ����: ���� 1ȸ�� �Ҵ�
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        // �� ���� ����
        CreateNodeMap();
    }

    /// <summary>
    /// ��ü ��� ���� �����ϰ�, ��ġ�� ���� ��ġ
    /// </summary>
    public void CreateNodeMap()
    {
        for (int i = 0; i < nodeList.GetLength(0); i++) // Z�� ����
        {
            for (int j = 0; j < nodeList.GetLength(1); j++) // X�� ����
            {
                // �� ��带 (X, Y, Z) = (minX + j*2, 0, minY + i*2) ��ġ�� ����
                nodeScript = Instantiate(
                    nodePrefab,
                    new Vector3(minX + j * 2, 0, minY + i * 2),
                    Quaternion.identity
                );

                // ������ ��带 �迭�� ����
                nodeList[i, j] = nodeScript;
            }
        }
    }
}
