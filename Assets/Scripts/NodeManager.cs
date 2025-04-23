using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // 싱글톤 인스턴스 (전역 접근을 위한 고정 참조)
    public static NodeManager _instance;

    // 2차원 노드 배열 [행, 열] = [z, x]
    Node[,] nodeList = new Node[23, 25];

    // 노드 프리팹 참조 (에디터에 연결 필요)
    [SerializeField] public Node nodePrefab;

    // 인스턴스화한 노드에 접근하기 위한 임시 변수
    [SerializeField] public Node nodeScript;

    // 맵 좌측 하단 기준 위치 오프셋
    int minX = -12;
    int minY = -1;

    private void Awake()
    {
        // 싱글톤 설정: 최초 1회만 할당
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        // 맵 생성 시작
        CreateNodeMap();
    }

    /// <summary>
    /// 전체 노드 맵을 생성하고, 위치에 따라 배치
    /// </summary>
    public void CreateNodeMap()
    {
        for (int i = 0; i < nodeList.GetLength(0); i++) // Z축 방향
        {
            for (int j = 0; j < nodeList.GetLength(1); j++) // X축 방향
            {
                // 각 노드를 (X, Y, Z) = (minX + j*2, 0, minY + i*2) 위치에 생성
                nodeScript = Instantiate(
                    nodePrefab,
                    new Vector3(minX + j * 2, 0, minY + i * 2),
                    Quaternion.identity
                );

                // 생성된 노드를 배열에 저장
                nodeList[i, j] = nodeScript;
            }
        }
    }
}
