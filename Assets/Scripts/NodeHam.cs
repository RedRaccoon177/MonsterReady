using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeHam : MonoBehaviour
{
    Vector3[] directions;
    [SerializeField] public bool _isWalkale;
    public List<NodeHam> _connectionNodes = new List<NodeHam>();


    private void Start()
    {
        directions = new Vector3[]
        {
            Vector3.right,
            Vector3.left,
            Vector3.forward,
            Vector3.back,
            new Vector3(1, 0, 1).normalized,
            new Vector3(1, 0, -1).normalized,
            new Vector3(-1, 0, 1).normalized,
            new Vector3(-1, 0, -1).normalized
        };
        _isWalkale = true;
        Vector3 halfExtents = new Vector3(0.7f, 1f, 0.7f);
        Collider[] hits = Physics.OverlapBox(transform.position, halfExtents);
        int count = 0;
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue; // 자기 자신 제외

            if (hit.CompareTag("Player") || hit.CompareTag("Door") || hit.CompareTag("Plane") || hit.CompareTag("ExpandWall"))
                continue; // 예외 대상 제외

            count++;
        }

        if (count >= 1) // 1개라도 예외가 아닌 충돌체가 있으면 비활성화
        { 
            _isWalkale = false;
        }
        RaycastHit hitS;
        foreach (var a in directions)
        {
            if (Physics.Raycast(transform.position, a,out hitS,LayerMask.GetMask("Node")))
            {
                NodeHam node = hitS.collider.GetComponent<NodeHam>();
                if (node != null)
                {
                    _connectionNodes.Add(node);
                }
            }
        }
    }
}
