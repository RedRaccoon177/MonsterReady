using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSpawaner : MonoBehaviour
{
    Queue<Boat> boatQueue = new Queue<Boat>();
    [SerializeField] Transform pointParant;
    [SerializeField] Transform boatParnat;
    [SerializeField] Transform endPoint;
    public List<Transform> pointList { get;private set; }
    [SerializeField] public List<bool> isVisited {  get; private set; }
    [SerializeField] int maxBoatCount;
    [SerializeField] GameObject[] boatPrefabs;
    [SerializeField] GameObject[] customerPrefabs;
    private WaitForSeconds waitForSeconds;
    [SerializeField] float spawnTime;


    private void Start()
    {
        waitForSeconds = new WaitForSeconds(spawnTime);
        isVisited = new List<bool>();
        pointList = new List<Transform>();
        foreach (Transform child in pointParant.GetComponentsInChildren<Transform>())
        {
            if (child != pointParant.transform)
            {
                isVisited.Add(false);
                pointList.Add(child);
            }
        }
        StartCoroutine(SpawnBoat());
    }

    IEnumerator SpawnBoat()
    {
        while (true)
        {
            yield return waitForSeconds;  
            if (boatQueue.Count < pointList.Count) 
            { 
                CreateBoat();
            }
        }
    }

    void CreateBoat()
    {
        int randBoat = Random.Range(0, boatPrefabs.Length);
        int randCustermer = Random.Range(0, customerPrefabs.Length);
        var boatScript = Instantiate(boatPrefabs[randBoat], boatParnat).GetComponent<Boat>();
        boatScript.Init(this, customerPrefabs[randCustermer],endPoint);
        boatQueue.Enqueue(boatScript);
    }

    private void OnDrawGizmos()
    {
        if (pointList == null)
        {
            pointList = new List<Transform>();
            foreach (Transform child in pointParant.GetComponentsInChildren<Transform>())
            {
                if (child != pointParant.transform)
                {
                    pointList.Add(child);
                }
            }
        }
        Gizmos.color = Color.blue;
        for (int i = 0; i < pointList.Count - 1; i++)
        {
            if (pointList[i] != null && pointList[i + 1] != null)
            {
                Gizmos.DrawLine(pointList[i].position, pointList[i + 1].position);
            }
        }
        foreach (Transform point in pointList)
        {
            if (point != null)
            {
                Gizmos.DrawWireSphere(point.position, 0.3f);
            }
        }

    }
}
