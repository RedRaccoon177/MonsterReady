using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BoatSpawaner : MonoBehaviour
{
    public int _boatCount;
    [SerializeField] Transform _pointParant;
    [SerializeField] Transform _boatParnat;
    [SerializeField] Transform _endPoint;
    public List<Transform> _pointList;
    [SerializeField] public List<bool> _isVisited {  get; private set; }
    [SerializeField] int _maxBoatCount;
    [SerializeField] GameObject[] _customerPrefabs;
    private WaitForSeconds _waitForSeconds;
    [SerializeField] float _spawnTime;
    [SerializeField] ObjectPooling _objectPool;
    [SerializeField] private BoatPool _boat01Pool;
    [SerializeField] private BoatPool _boat02Pool;
    [SerializeField] private BoatPool _boat03Pool;



    private void Start()
    {
        _waitForSeconds = new WaitForSeconds(_spawnTime);

        _isVisited = new List<bool>();
        _pointList = new List<Transform>();
        foreach (Transform child in _pointParant.transform)
        {
            _pointList.Add(child);
            _isVisited.Add(false);
        }

        StartCoroutine(SpawnBoat());
        _boat01Pool.Create();
        _boat02Pool.Create();
        _boat03Pool.Create();
    }

    IEnumerator SpawnBoat()
    {
        while (true)
        {
            yield return _waitForSeconds;  
            if (_boatCount < _pointList.Count) 
            { 
                CreateBoat();
            }
        }
    }
    Boat BoatPool()
    {
        int randBoat = Random.Range(0, 3);
        switch (randBoat)
        {
            case 0:
                return _boat01Pool.Get();
            case 1:
                return _boat02Pool.Get();
            case 2:
                return _boat03Pool.Get();
            default:
                return null;
        }
    }
    void CreateBoat()
    {
        var boatScript = BoatPool();
        boatScript.Init(this,_endPoint, _objectPool);
        _boatCount++;
    }

    private void OnDrawGizmos()
    {
        if (_pointList == null)
        {
            _pointList = new List<Transform>();
            foreach (Transform child in _pointParant.GetComponentsInChildren<Transform>())
            {
                if (child != _pointParant.transform)
                {
                    _pointList.Add(child);
                }
            }
        }
        Gizmos.color = Color.blue;
        for (int i = 0; i < _pointList.Count - 1; i++)
        {
            if (_pointList[i] != null && _pointList[i + 1] != null)
            {
                Gizmos.DrawLine(_pointList[i].position, _pointList[i + 1].position);
            }
        }
        foreach (Transform point in _pointList)
        {
            if (point != null)
            {
                Gizmos.DrawWireSphere(point.position, 0.3f);
            }
        }

    }
}
