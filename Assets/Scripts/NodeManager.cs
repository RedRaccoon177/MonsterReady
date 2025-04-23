using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager _instance;
    NodeHam[,] nodeList = new NodeHam[23,25]; 
    [SerializeField] public NodeHam nodePrefab;
    [SerializeField] public NodeHam nodeScript;
    int minX = -12;
    int minY = -1;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }
    private void Start()
    {
        CreateNodeMap();
    }

    public void CreateNodeMap()
    {
        for (int i = 0; i < nodeList.GetLength(0); i++)
        {
            for (int j = 0; j < nodeList.GetLength(1); j++)
            {
                nodeScript = Instantiate(nodePrefab, new Vector3(minX + j * 2, 0, minY + i * 2), Quaternion.identity);
                nodeList[i, j] = nodeScript;
            }
        }
    }
}
