 using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;


public class ObjectPooling : MonoBehaviour
{
    [Header("����")]
    [SerializeField] GameObject _goldPrefab;        // ��� ������
    [SerializeField] int _goldInitialSize = 100;   // �̸� ������ ��� ������

    [Header("���")]
    [SerializeField] GameObject _meatPrefab;        // ��� ������
    [SerializeField] int _meatInitialSize = 100;    // �̸� ������ ��� ������

    [Header("��")]
    [SerializeField] GameObject _bonePrefab;        // ��� ������
    [SerializeField] int _boneInitialSize = 100;    // �̸� ������ ��� ������

    // Ǯ ���� ť (���� ��� ������Ʈ)
    private Queue<GameObject> _goldPool = new Queue<GameObject>();
    private Queue<GameObject> _meatPool = new Queue<GameObject>(); 
    private Queue<GameObject> _bonePool = new Queue<GameObject>(); 

    /// <summary>
    /// ���� ���� �� Ǯ�� �ʱ�ȭ�ϰ� ������Ʈ���� �̸� ������ �д�.
    /// </summary>
    void Awake()
    {
        //���
        for (int i = 0; i < _goldInitialSize; i++)
        {
            GameObject obj = Instantiate(_goldPrefab);      // ������ �ν��Ͻ� ����
            obj.SetActive(false);                           // ��Ȱ��ȭ ��
            _goldPool.Enqueue(obj);                         // Ǯ�� ���
        }

        //���
        for (int i = 0; i < _meatInitialSize; i++)
        {
            GameObject obj = Instantiate(_meatPrefab);      
            obj.SetActive(false);                          
            _meatPool.Enqueue(obj);                   
        }

        //��
        for (int i = 0; i < _boneInitialSize; i++)
        {
            GameObject obj = Instantiate(_bonePrefab);      
            obj.SetActive(false);                          
            _bonePool.Enqueue(obj);                      
        }

    }

    /// <summary>
    /// ���Ǯ���� ������Ʈ�� ������ ��ȯ��. ������ ���� ����
    /// </summary>
    /// <returns>��� ������ ������Ʈ</returns>
    public GameObject GetGoldBar()
    {
        // Ǯ�� ��������� �ڵ����� 1�� ���� �� �߰�
        if (_goldPool.Count == 0)
        {
            GameObject obj = Instantiate(_goldPrefab);
            obj.SetActive(false);
            _goldPool.Enqueue(obj);
        }

        // Ǯ���� �ϳ� ������ ��ȯ
        GameObject pooledObj = _goldPool.Dequeue();
        pooledObj.SetActive(true);
        return pooledObj;
    }

    /// <summary>
    /// ���Ǯ���� ������Ʈ�� ������ ��ȯ��. ������ ���� ����
    /// </summary>
    /// <returns>��� ������ ������Ʈ</returns>
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
    /// ��Ǯ���� ������Ʈ�� ������ ��ȯ��. ������ ���� ����
    /// </summary>
    /// <returns>��� ������ ������Ʈ</returns>
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
    /// ������Ʈ�� �ٽ� Ǯ�� ��ȯ�Ͽ� ���� �����ϵ��� �����.
    /// </summary>
    /// <param name="obj">������ ������Ʈ</param>
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
                Debug.LogWarning($"�� �� ���� ������Ʈ ��ȯ �õ�: {obj.name}");
                break;
        }
    }
}
