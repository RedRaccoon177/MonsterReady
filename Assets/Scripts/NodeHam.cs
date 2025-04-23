using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeHam : MonoBehaviour
{
    // 주변 방향 벡터들 (상하좌우 + 대각선 총 8방향)
    Vector3[] directions;

    // 이 노드가 이동 가능한지 여부
    [SerializeField] public bool _isWalkale;

    // 연결된 인접 노드 목록
    public List<NodeHam> _connectionNodes = new List<NodeHam>();

    private void Start()
    {
        // 8방향 정의 (정규화 포함)
        directions = new Vector3[]
        {
            Vector3.right,                         // 오른쪽
            Vector3.left,                          // 왼쪽
            Vector3.forward,                       // 앞
            Vector3.back,                          // 뒤
            new Vector3(1, 0, 1).normalized,       // 오른쪽 위 대각선
            new Vector3(1, 0, -1).normalized,      // 오른쪽 아래 대각선
            new Vector3(-1, 0, 1).normalized,      // 왼쪽 위 대각선
            new Vector3(-1, 0, -1).normalized      // 왼쪽 아래 대각선
        };

        _isWalkale = true; // 기본적으로는 이동 가능하다고 설정

        // OverlapBox를 이용해 주변 충돌체 감지 (0.7 x 1 x 0.7 크기)
        Vector3 halfExtents = new Vector3(0.7f, 1f, 0.7f);
        Collider[] hits = Physics.OverlapBox(transform.position, halfExtents);

        int count = 0;
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue; // 자기 자신은 제외

            // 특정 태그는 예외 처리
            if (hit.CompareTag("Player") || hit.CompareTag("Door") || hit.CompareTag("Plane") || hit.CompareTag("ExpandWall"))
                continue;

            count++; // 예외가 아닌 충돌체가 하나라도 있다면
        }

        // 충돌체가 있으면 이동 불가능한 노드로 표시
        if (count >= 1)
        {
            _isWalkale = false;
        }

        // 주변 노드 탐색을 위한 Raycast 수행
        RaycastHit hitS;
        foreach (var a in directions)
        {
            // 지정된 방향으로 Ray를 쏴서 Node 레이어에 충돌했는지 확인
            if (Physics.Raycast(transform.position, a, out hitS, Mathf.Infinity, LayerMask.GetMask("Node")))
            {
                NodeHam node = hitS.collider.GetComponent<NodeHam>();
                if (node != null)
                {
                    // 충돌한 오브젝트에 NodeHam이 있다면 연결 노드로 등록
                    _connectionNodes.Add(node);
                }
            }
        }
    }


    /// <summary>
    /// 노드 halfExtents 시각화 (겹치면 빨강, 안겹치면 연두)
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            _isWalkale = true;

        Gizmos.color = _isWalkale ? new Color(0f, 1f, 0f, 0.3f) : new Color(1f, 0f, 0f, 0.3f);
        Vector3 halfExtents = new Vector3(0.7f, 1f, 0.7f);
        Gizmos.DrawCube(transform.position, halfExtents * 2);
    }
}
