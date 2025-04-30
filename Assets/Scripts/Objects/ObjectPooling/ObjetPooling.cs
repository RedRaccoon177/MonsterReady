 using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;


public class ObjectPooling : MonoBehaviour
{
    [Header("골드바")]
    [SerializeField] GameObject _goldPrefab;        // 골드 프리팹
    [SerializeField] int _goldInitialSize = 100;   // 미리 생성할 골드 프리팹

    [Header("고기")]
    [SerializeField] GameObject _meatPrefab;        // 고기 프리팹
    [SerializeField] int _meatInitialSize = 100;    // 미리 생성할 고기 프리팹

    [Header("뼈")]
    [SerializeField] GameObject _bonePrefab;        // 고기 프리팹
    [SerializeField] int _boneInitialSize = 100;    // 미리 생성할 고기 프리팹

    // 풀 내부 큐 (재사용 대기 오브젝트)
    private Queue<GameObject> _goldPool = new Queue<GameObject>();
    private Queue<GameObject> _meatPool = new Queue<GameObject>(); 
    private Queue<GameObject> _bonePool = new Queue<GameObject>(); 

    /// <summary>
    /// 게임 시작 시 풀을 초기화하고 오브젝트들을 미리 생성해 둔다.
    /// </summary>
    void Awake()
    {
        //골드
        for (int i = 0; i < _goldInitialSize; i++)
        {
            GameObject obj = Instantiate(_goldPrefab);      // 프리팹 인스턴스 생성
            obj.SetActive(false);                           // 비활성화 후
            _goldPool.Enqueue(obj);                         // 풀에 등록
        }

        //고기
        for (int i = 0; i < _meatInitialSize; i++)
        {
            GameObject obj = Instantiate(_meatPrefab);      
            obj.SetActive(false);                          
            _meatPool.Enqueue(obj);                   
        }

        //뼈
        for (int i = 0; i < _boneInitialSize; i++)
        {
            GameObject obj = Instantiate(_bonePrefab);      
            obj.SetActive(false);                          
            _bonePool.Enqueue(obj);                      
        }

    }

    /// <summary>
    /// 골드풀에서 오브젝트를 꺼내서 반환함. 없으면 새로 생성
    /// </summary>
    /// <returns>사용 가능한 오브젝트</returns>
    public GameObject GetGoldBar()
    {
        // 풀이 비어있으면 자동으로 1개 생성 후 추가
        if (_goldPool.Count == 0)
        {
            GameObject obj = Instantiate(_goldPrefab);
            obj.SetActive(false);
            _goldPool.Enqueue(obj);
        }

        // 풀에서 하나 꺼내서 반환
        GameObject pooledObj = _goldPool.Dequeue();
        pooledObj.SetActive(true);
        return pooledObj;
    }

    /// <summary>
    /// 고기풀에서 오브젝트를 꺼내서 반환함. 없으면 새로 생성
    /// </summary>
    /// <returns>사용 가능한 오브젝트</returns>
    public GameObject GetMeat()
    {
        if (_meatPool.Count == 0)
        {
            GameObject obj = Instantiate(_meatPrefab);
            obj.SetActive(false);
            _meatPool.Enqueue(obj);
        }

        GameObject pooledObj = _meatPool.Dequeue();
        pooledObj.SetActive(true);
        return pooledObj;
    }

    /// <summary>
    /// 뼈풀에서 오브젝트를 꺼내서 반환함. 없으면 새로 생성
    /// </summary>
    /// <returns>사용 가능한 오브젝트</returns>
    public GameObject GetBone()
    {
        if (_bonePool.Count == 0)
        {
            GameObject obj = Instantiate(_bonePrefab);
            obj.SetActive(false);
            _bonePool.Enqueue(obj);
        }

        GameObject pooledObj = _bonePool.Dequeue();
        pooledObj.SetActive(true);
        return pooledObj;
    }

    /// <summary>
    /// 오브젝트를 다시 풀에 반환하여 재사용 가능하도록 만든다.
    /// </summary>
    /// <param name="obj">재사용할 오브젝트</param>
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);

        switch (obj.tag)
        {
            case "Gold":
                _goldPool.Enqueue(obj);
                break;

            case "Meat":
                _meatPool.Enqueue(obj);
                break;

            case "Bone":
                _bonePool.Enqueue(obj);
                break;

            default:
                Debug.LogWarning($"알 수 없는 오브젝트 반환 시도: {obj.name}");
                break;
        }
    }
}
