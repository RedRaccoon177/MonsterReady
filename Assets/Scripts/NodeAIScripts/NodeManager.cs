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

    [SerializeField] public int _XWidth = 23;
    [SerializeField] public int _YLength = 25;

    // Node를 저장하는 2차원 배열 (Z축, X축 순서로 구성)
    public Node[,] _nodeList;

    // 노드 프리팹 (에디터에서 연결 필수)
    [SerializeField] public Node _nodePrefab;

    // 맵의 좌측 하단 기준 위치 오프셋
    int _minX = -12;
    int _minY = -1;

    void Awake()
    {
        // 싱글톤 패턴 설정
        if (_instance == null)
        {
            _instance = this;
        }

        _nodeList = new Node[_XWidth, _YLength];
    }

    void Start()
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
        for (int i = 0; i < _nodeList.GetLength(0); i++)
        {
            for (int j = 0; j < _nodeList.GetLength(1); j++)
            {
                Node node = Instantiate(_nodePrefab, new Vector3(_minX + j * 2, 0, _minY + i * 2), Quaternion.identity);

                node.Init(new Vector2Int(i, j)); // 좌표 설정

                _nodeList[i, j] = node;
            }
        }
        StartCoroutine(DelayedConnection());
    }

    IEnumerator DelayedConnection()
    {
        yield return null;

        // 이제 연결 시작
        for (int i = 0; i < _nodeList.GetLength(0); i++)
        {
            for (int j = 0; j < _nodeList.GetLength(1); j++)
            {
                _nodeList[i, j].ConnectionNodes();
            }
        }
    }
}
