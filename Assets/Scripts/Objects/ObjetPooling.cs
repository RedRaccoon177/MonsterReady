using System.Collections.Generic;
using UnityEngine;


public class ObjectPooling : MonoBehaviour
{
    [SerializeField] GameObject _goldPrefab;      // 골드 프리팹
    [SerializeField] int _initialSize = 6400;     // 미리 생성할 골드 프리팹

    private Queue<GameObject> _pool = new Queue<GameObject>(); // 풀 내부 큐 (재사용 대기 오브젝트)

    /// <summary>
    /// 게임 시작 시 풀을 초기화하고 오브젝트들을 미리 생성해 둔다.
    /// </summary>
    void Awake()
    {
        for (int i = 0; i < _initialSize; i++)
        {
            GameObject obj = Instantiate(_goldPrefab); // 프리팹 인스턴스 생성
            obj.SetActive(false);                      // 비활성화 후
            _pool.Enqueue(obj);                        // 풀에 등록
        }
    }

    /// <summary>
    /// 풀에서 오브젝트를 꺼내서 반환한다.
    /// 없으면 새로 생성해서 사용한다.
    /// </summary>
    /// <returns>사용 가능한 오브젝트</returns>
    public GameObject GetGoldBar()
    {
        // 풀이 비어있으면 자동으로 1개 생성 후 추가
        if (_pool.Count == 0)
        {
            GameObject obj = Instantiate(_goldPrefab);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }

        // 풀에서 하나 꺼내서 반환
        GameObject pooledObj = _pool.Dequeue();
        pooledObj.SetActive(true);
        return pooledObj;
    }

    /// <summary>
    /// 오브젝트를 다시 풀에 반환하여 재사용 가능하도록 만든다.
    /// </summary>
    /// <param name="obj">재사용할 오브젝트</param>
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);     // 비활성화
        _pool.Enqueue(obj);       // 큐에 다시 넣기
    }
}
