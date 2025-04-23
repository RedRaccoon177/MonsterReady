using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 격자 기반 맵을 구성하는 Node들을 생성하고,
/// 전체 맵 구조를 2차원 배열로 관리하는 매니저 클래스
/// </summary>
public class NodeManager : MonoBehaviour
{
    // 싱글톤 인스턴스 (전역에서 접근 가능하도록 설정)
    public static NodeManager _instance;

    // Node를 저장하는 2차원 배열 (Z축, X축 순서로 구성)
    Node[,] _nodeList = new Node[23, 25];

    // 노드 프리팹 (에디터에서 연결 필수)
    [SerializeField] public Node _nodePrefab;

    // 노드를 생성 후 임시로 보관하는 변수 (초기화 용도)
    [SerializeField] public Node _nodeScript;

    // 맵의 좌측 하단 기준 위치 오프셋
    int _minX = -12;
    int _minY = -1;

    private void Awake()
    {
        // 싱글톤 패턴 설정
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        // 게임 시작 시 맵 생성
        CreateNodeMap();
    }

    /// <summary>
    /// Node 프리팹을 일정한 간격으로 배치하여
    /// 격자 형태의 맵을 생성하고, 그 정보를 배열에 저장
    /// </summary>
    public void CreateNodeMap()
    {
        for (int i = 0; i < _nodeList.GetLength(0); i++) // Z축 방향 반복
        {
            for (int j = 0; j < _nodeList.GetLength(1); j++) // X축 방향 반복
            {
                // 노드의 월드 상 위치 계산
                Vector3 pos = new Vector3(_minX + j * 2, 0f, _minY + i * 2);

                // 노드 프리팹 생성 및 배치
                Node node = Instantiate(_nodePrefab, pos, Quaternion.identity);

                // 노드에 자신의 그리드 좌표를 전달
                node.Init(new Vector2Int(j, i));

                // 배열에 해당 노드 저장
                _nodeList[i, j] = node;
            }
        }
    }
}
