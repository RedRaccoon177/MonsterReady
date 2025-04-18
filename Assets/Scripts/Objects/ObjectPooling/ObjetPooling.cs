using System.Collections.Generic;
using UnityEngine;


public class ObjectPooling : MonoBehaviour
{
    [SerializeField] GameObject _goldPrefab;      // ��� ������
    [SerializeField] int _initialSize = 6400;     // �̸� ������ ��� ������

    private Queue<GameObject> _pool = new Queue<GameObject>(); // Ǯ ���� ť (���� ��� ������Ʈ)

    /// <summary>
    /// ���� ���� �� Ǯ�� �ʱ�ȭ�ϰ� ������Ʈ���� �̸� ������ �д�.
    /// </summary>
    void Awake()
    {
        for (int i = 0; i < _initialSize; i++)
        {
            GameObject obj = Instantiate(_goldPrefab); // ������ �ν��Ͻ� ����
            obj.SetActive(false);                      // ��Ȱ��ȭ ��
            _pool.Enqueue(obj);                        // Ǯ�� ���
        }
    }

    /// <summary>
    /// Ǯ���� ������Ʈ�� ������ ��ȯ�Ѵ�.
    /// ������ ���� �����ؼ� ����Ѵ�.
    /// </summary>
    /// <returns>��� ������ ������Ʈ</returns>
    public GameObject GetGoldBar()
    {
        // Ǯ�� ��������� �ڵ����� 1�� ���� �� �߰�
        if (_pool.Count == 0)
        {
            GameObject obj = Instantiate(_goldPrefab);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }

        // Ǯ���� �ϳ� ������ ��ȯ
        GameObject pooledObj = _pool.Dequeue();
        pooledObj.SetActive(true);
        return pooledObj;
    }

    /// <summary>
    /// ������Ʈ�� �ٽ� Ǯ�� ��ȯ�Ͽ� ���� �����ϵ��� �����.
    /// </summary>
    /// <param name="obj">������ ������Ʈ</param>
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);     // ��Ȱ��ȭ
        _pool.Enqueue(obj);       // ť�� �ٽ� �ֱ�
    }
}
