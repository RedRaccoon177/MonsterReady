using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    // 노드를 보관하는 게임 오브젝트
    [SerializeField] private GameObject Nodes;

    void Awake()
    {
        // 싱글톤 패턴 설정
        if (_instance == null)
        {
            _instance = this;
        }

        _nodeList = new Node[_XWidth, _YLength];
        CreateNodeMap();
    }

    void Start()
    {
        // 게임 시작 시 맵 생성
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
                //TODO: 노드들 오브젝트 풀링으로 담기
                Node node = Instantiate(_nodePrefab, new Vector3(_minX + j * 2, 0, _minY + i * 2), Quaternion.identity, Nodes.transform);

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
    public Node GetNearestNodeOptimized(Vector3 worldPos)
    {
        // 좌표 기준으로 인덱스 추정
        int x = Mathf.RoundToInt((worldPos.z - _minY) / 2f);
        int y = Mathf.RoundToInt((worldPos.x - _minX) / 2f);

        // 범위 클램프
        x = Mathf.Clamp(x, 0, _nodeList.GetLength(0) - 1);
        y = Mathf.Clamp(y, 0, _nodeList.GetLength(1) - 1);

        return _nodeList[x, y];
    }

    public void SettingFirstExpand()
    {
        _nodeList[11,9]._isWalkale = true;
        _nodeList[11,10]._isWalkale = true;
        _nodeList[11,11]._isWalkale = true;
        _nodeList[11,12]._isWalkale = true;
        _nodeList[11,13]._isWalkale = true;
        _nodeList[11,14]._isWalkale = true;
        _nodeList[11,15]._isWalkale = true;
        _nodeList[11,16]._isWalkale = true;
        _nodeList[11,17]._isWalkale = true;
        _nodeList[11,18]._isWalkale = true;
        _nodeList[10,9]._isWalkale = true;
    }
    public void SettingSecondExpand()
    {
        _nodeList[9, 9]._isWalkale = true;
        _nodeList[9, 10]._isWalkale = true;
        _nodeList[9, 11]._isWalkale = true;
        _nodeList[9, 12]._isWalkale = true;
        _nodeList[9, 13]._isWalkale = true;
        _nodeList[9, 14]._isWalkale = true;
        _nodeList[9, 15]._isWalkale = true;
        _nodeList[9, 16]._isWalkale = true;
        _nodeList[9, 17]._isWalkale = true;
        _nodeList[9, 18]._isWalkale = true;
        _nodeList[8,9]._isWalkale = true;
        _nodeList[7,9]._isWalkale = true;
        _nodeList[6,9]._isWalkale = true;
        _nodeList[5,9]._isWalkale = true;
    }
}
