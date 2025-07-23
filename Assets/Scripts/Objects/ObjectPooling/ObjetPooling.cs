using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [Header("부모 오브젝트 (정리용)")]
    [SerializeField] Transform _goldParent;    // 골드 오브젝트들을 정리할 부모 Transform
    [SerializeField] Transform _meatParent;    // 고기 오브젝트들을 정리할 부모 Transform
    [SerializeField] Transform _boneParent;    // 뼈 오브젝트들을 정리할 부모 Transform
    [SerializeField] Transform _boxParent;     // 박스 오브젝트들을 정리할 부모 Transform
    [SerializeField] Transform _boatParent;     // 보트 오브젝트들을 정리할 부모 Transform

    [Header("골드바")]
    [SerializeField] GameObject _goldPrefab;   // 골드 프리팹
    [SerializeField] int _goldInitialSize = 100;   // 미리 생성할 골드 개수

    [Header("고기")]
    [SerializeField] GameObject _meatPrefab;   // 고기 프리팹
    [SerializeField] int _meatInitialSize = 100;   // 미리 생성할 고기 개수

    [Header("뼈")]
    [SerializeField] GameObject _bonePrefab;   // 뼈 프리팹
    [SerializeField] int _boneInitialSize = 100;   // 미리 생성할 뼈 개수

    [Header("박스")]
    [SerializeField] GameObject _boxPrefab;    
    [SerializeField] int _boxInitialSize = 100;

    [Header("박스")]
    [SerializeField] GameObject _boat01;
    [SerializeField] int _boat01InitialSize = 5;
    [Header("박스")]
    [SerializeField] GameObject _boat02;
    [SerializeField] int _boat02InitialSize = 5;
    [Header("박스")]
    [SerializeField] GameObject _boat03;
    [SerializeField] int _boat03InitialSize = 5;


    // 각 풀을 관리할 큐 (Queue 자료구조 사용: 선입선출)
    Queue<GameObject> _goldPool = new Queue<GameObject>();
    Queue<GameObject> _meatPool = new Queue<GameObject>();
    Queue<GameObject> _bonePool = new Queue<GameObject>();
    Queue<GameObject> _boxPool = new Queue<GameObject>();
    Queue<GameObject> _boat01Pool = new Queue<GameObject>();
    Queue<GameObject> _boat02Pool = new Queue<GameObject>();
    Queue<GameObject> _boat03Pool = new Queue<GameObject>();

    /// <summary>
    /// 게임 시작 시 풀 초기화, 각 오브젝트들을 미리 생성하여 큐에 채워둔다.
    /// </summary>
    void Awake()
    {
        InitPool(_goldPrefab, _goldInitialSize, _goldPool, _goldParent);
        InitPool(_meatPrefab, _meatInitialSize, _meatPool, _meatParent);
        InitPool(_bonePrefab, _boneInitialSize, _bonePool, _boneParent);
        InitPool(_boxPrefab, _boxInitialSize, _boxPool, _boxParent);
       //InitPool(_boat01, _boat01InitialSize, _boat01Pool, _boatParent);
       //InitPool(_boat02, _boat02InitialSize, _boat02Pool, _boatParent);
       //InitPool(_boat03, _boat03InitialSize, _boat03Pool, _boatParent);
    }

    /// <summary>
    /// 특정 풀을 초기화하여 지정한 개수만큼 미리 생성
    /// </summary>
    void InitPool(GameObject prefab, int size, Queue<GameObject> pool, Transform parent)
    {
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(prefab, parent);   // 부모 밑으로 생성
            obj.SetActive(false);                          // 비활성화 후
            pool.Enqueue(obj);                             // 큐에 등록
        }
    }

    /// <summary>
    /// 골드 오브젝트 가져오기
    /// </summary>
    public GameObject GetGoldBar() => GetFromPool(_goldPrefab, _goldPool, _goldParent);

    /// <summary>
    /// 고기 오브젝트 가져오기
    /// </summary>
    public GameObject GetMeat() => GetFromPool(_meatPrefab, _meatPool, _meatParent);

    /// <summary>
    /// 뼈 오브젝트 가져오기
    /// </summary>
    public GameObject GetBone() => GetFromPool(_bonePrefab, _bonePool, _boneParent);

    /// <summary>
    /// 박스 오브젝트 가져오기
    /// </summary>
    public GameObject GetBox() => GetFromPool(_boxPrefab, _boxPool, _boxParent);
    public GameObject GetBoat01() => GetFromPool(_boat01, _boat01Pool, _boatParent);
    public GameObject GetBoat02() => GetFromPool(_boat02, _boat02Pool, _boatParent);
    public GameObject GetBoat03() => GetFromPool(_boat03, _boat03Pool, _boatParent);

    /// <summary>
    /// 특정 풀에서 오브젝트 꺼내오기 (없으면 새로 생성)
    /// </summary>
    GameObject GetFromPool(GameObject prefab, Queue<GameObject> pool, Transform parent)
    {
        // 풀이 비어있다면 새로 생성
        if (pool.Count == 0)
        {
            GameObject obj = Instantiate(prefab, parent);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        // 큐에서 꺼내서 활성화 후 반환
        GameObject pooledObj = pool.Dequeue();
        pooledObj.SetActive(true);
        return pooledObj;
    }

    /// <summary>
    /// 오브젝트를 다시 풀에 반환하여 재사용 가능하게 만든다.
    /// </summary>
    /// <param name="obj">재사용할 오브젝트</param>
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);  // 비활성화

        // tag로 풀 분류
        switch (obj.tag)
        {
            case "Gold":
                obj.transform.SetParent(_goldParent);   // 부모 지정 (혹시라도 떨어진 경우 대비)
                _goldPool.Enqueue(obj);
                break;

            case "Meat":
                obj.transform.SetParent(_meatParent);
                _meatPool.Enqueue(obj);
                break;

            case "Bone":
                obj.transform.SetParent(_boneParent);
                _bonePool.Enqueue(obj);
                break;

            case "Box":
                obj.transform.SetParent(_boxParent);
                _boxPool.Enqueue(obj);
                break;
            case "Boat01":
                obj.transform.SetParent(_boatParent);
                _boat01Pool.Enqueue(obj);
                break;
            case "Boat02":
                obj.transform.SetParent(_boatParent);
                _boat02Pool.Enqueue(obj);
                break;
            case "Boat03":
                obj.transform.SetParent(_boatParent);
                _boat03Pool.Enqueue(obj);
                break;
            default:
                Debug.LogWarning($"알 수 없는 오브젝트 반환 시도: {obj.name}");
                break;
        }
    }
}
