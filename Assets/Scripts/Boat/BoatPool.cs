using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BoatPool : MonoBehaviour
{
    [SerializeField] private GameObject _boatPrefab;
    [SerializeField] private Transform _parant;
    [SerializeField] private int _initialCount = 5;
    private Queue<Boat> _pool = new Queue<Boat>();

    // 초기화 시점에 미리 풀 생성
    public void Create()
    {
        for (int i = 0; i < _initialCount; i++)
        {
            Boat boat = Instantiate(_boatPrefab, _parant).GetComponent<Boat>();
            boat.SetOriginPool(this);
            boat.gameObject.SetActive(false);
            _pool.Enqueue(boat);
        }
    }

    // 오브젝트 꺼내기
    public Boat Get()
    {
        Boat boat;
        if (_pool.Count > 0)
        {
            boat = _pool.Dequeue();
        }
        else
        {
            boat = Instantiate(_boatPrefab, _parant).GetComponent<Boat>();
            boat.SetOriginPool(this);
        }
        boat.gameObject.SetActive(true);
        return boat;
    }

    // 오브젝트 반환
    public void ReturnObject(Boat boat)
    {
        boat.gameObject.SetActive(false);
        _pool.Enqueue(boat);
    }
}
