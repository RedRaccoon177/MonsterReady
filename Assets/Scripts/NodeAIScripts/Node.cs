using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Node : MonoBehaviour
{
    #region 노드 변수들
    // 8방향 방향 벡터들 (상하좌우 + 대각선)
    Vector3[] _directions;

    // 해당 노드가 이동 가능한지 여부 (장애물 여부 판단)
    [SerializeField] public bool _isWalkale;

    // 인접한 연결 가능한 노드 목록
    public List<Node> _connectionNodes = new List<Node>();

    // 노드의 그리드 상 좌표 (A*에서 거리 계산용으로 사용)
    [SerializeField] Vector2Int _gridPos;

    #endregion

    void Start()
    {
        // 이동 가능으로 초기값 설정
        _isWalkale = true;

        // 해당 노드 위치에 장애물이 있는지 검사
        Vector3 halfExtents = new Vector3(0.7f, 1f, 0.7f); // 검사 박스의 크기
        Collider[] hits = Physics.OverlapBox(transform.position, halfExtents);

        int count = 0;
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue; // 자기 자신 제외

            // 예외 태그는 충돌체로 판단하지 않음
            if (hit.CompareTag("Player") || hit.CompareTag("Door") || hit.CompareTag("Plane") || hit.CompareTag("ExpandWall"))
                continue;

            count++; // 무시할 수 없는 장애물 존재
        }

        // 충돌체가 하나라도 있으면 이동 불가능 처리
        if (count >= 1)
        {
            _isWalkale = false;
        }
    }

    /// <summary>
    /// 노드의 좌표 정보 초기화 (NodeManager에서 할당)
    /// </summary>
    /// <param name="gridPos">그리드 상 좌표</param>
    public void Init(Vector2Int gridPos)
    {
        _gridPos = gridPos;
    }

    /// <summary>
    /// 에디터에서 노드 시각화
    /// 겹치는 장애물이 있으면 빨간색, 이동 가능하면 연두색
    /// </summary>
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            _isWalkale = true; // 편집 중에는 항상 초록으로 보기 위해 초기화

        Gizmos.color = _isWalkale ? new Color(0f, 1f, 0f, 0.3f) : new Color(1f, 0f, 0f, 0.3f);
        Vector3 halfExtents = new Vector3(0.7f, 1f, 0.7f);
        Gizmos.DrawCube(transform.position, halfExtents * 2); // 시각화 크기 = full size
    }

    public void ConnectionNodes()
    {
        // 8방향 오프셋
        Vector2Int[] offsets = new Vector2Int[]
        {
        new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1),
        new Vector2Int(0, -1),                     new Vector2Int(0, 1),
        new Vector2Int(1, -1),  new Vector2Int(1, 0),  new Vector2Int(1, 1)
        };

        foreach (var offset in offsets)
        {
            int newX = _gridPos.x + offset.x;
            int newY = _gridPos.y + offset.y;

            // 배열 범위 체크
            if (newX >= 0 && newX < NodeManager._instance._nodeList.GetLength(0) &&
                newY >= 0 && newY < NodeManager._instance._nodeList.GetLength(1))
            {
                Node neighbor = NodeManager._instance._nodeList[newX, newY];

                //이동 가능한 노드만 연결
                if (neighbor != null && neighbor._isWalkale && !_connectionNodes.Contains(neighbor))
                {
                    _connectionNodes.Add(neighbor);
                }
            }
        }
    }
}
