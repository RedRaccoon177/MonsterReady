using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatToBoxTrigger : MonoBehaviour
{
    [Header("������Ʈ Ǯ�� ����")]
    [SerializeField] ObjectPooling _boxPool; // �ڽ��� �����ϴ� ������Ʈ Ǯ

    [Header("�ڽ� ������")]
    [SerializeField] GameObject _boxPrefab;

    [Header("�ڽ� ��ġ�ϴ� ��")]
    [SerializeField] Transform _boxSpawnLocation;

    [Header("���� �ڽ� ����")]
    [SerializeField] public int _currentBoxCount = 0;

    [Header("�ڽ� ���� ���� ����")]
    [SerializeField] float _stackHeight = 0.333f;
    [SerializeField] float _counterY = 1.9f;

    [Header("��� ���̺� ����")]
    [SerializeField] MeatInputTrigger _meatInputTrigger;

    // ������ �ڽ� ������Ʈ���� ��� ����Ʈ
    List<GameObject> _boxList = new List<GameObject>();

    // �÷��̾� ����
    PlayerController _player;

    // ��⸦ �ڽ��� ��ȯ ��Ű�� �ڷ�ƾ
    Coroutine _meatChangeToBoxC;

    //��⸦ �ڽ��� ��ȯ��Ű�� �ִ� ���ΰ�?
    bool _isActive = false;

    //��⸦ �ڽ��� ��ȯ��Ű�� �ð�
    WaitForSeconds _waitDelay;
    [SerializeField] float _waitTime = 1;

    void Start()
    {
        _waitDelay = new WaitForSeconds(_waitTime);
        _player = PlayerController._instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isActive && _meatInputTrigger._currentMeatCount > 0)
        {
            _meatChangeToBoxC = StartCoroutine(MeatChangeToBox());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _meatChangeToBoxC != null)
        {
            StopCoroutine(_meatChangeToBoxC);
            _meatChangeToBoxC = null;
            _isActive = false;
        }
    }

    #region ��⸦ �ڽ��� ��ȯ�ϴ� �ڷ�ƾ
    IEnumerator MeatChangeToBox()
    {
        _isActive = true;

        while (_meatInputTrigger._currentMeatCount > 0)
        {
            ChangeMeatTOBox();            // ��� �ϳ� ��ȯ
            yield return _waitDelay;      // 1�� ���
        }

        _isActive = false;
    }

    public void ChangeMeatTOBox()
    {
        //1���� ��⸦ ����.
        _meatInputTrigger.MinusMeat(1);

        //1���� �ڽ��� ���Ѵ�.
        AddBox(1);
    }
    #endregion

    #region �ڽ� ���� �� ����
    /// <summary>
    /// ī���� �ڽ� ���� �Լ�
    /// </summary>
    /// <param name="_meatCount"></param>
    void AddBox(int _meatCount)
    {
        _currentBoxCount = Mathf.Max(0, _currentBoxCount + _meatCount);
        UpdateBoxDisplay(_currentBoxCount);
    }

    /// <summary>
    /// ī���� �ڽ� ����
    /// </summary>
    /// <param name="_minusMeat"></param>
    public int MinusBox(int _minusMeat)
    {
        int _someoneGetMeat;

        if (_currentBoxCount < _minusMeat)
        {
            _someoneGetMeat = _currentBoxCount;
            _currentBoxCount = 0;
        }
        else
        {
            _someoneGetMeat = _minusMeat;
            _currentBoxCount -= _minusMeat;
        }

        UpdateBoxDisplay(_currentBoxCount);
        return _someoneGetMeat;
    }
    #endregion

    #region �ڽ� �ð�ȭ �ϴ� �ڵ� ����
    //�ڽ� �ð�ȭ �ϴ� �ڵ�
    public void UpdateBoxDisplay(int currentMeat)
    {
        // 1. ��� ������ �����ϸ� ä����
        while (_boxList.Count < currentMeat)
        {
            GameObject box = _boxPool.GetBox(); // ������Ʈ Ǯ���� ����

            // ���� ��ġ��, ȸ����, ũ�Ⱚ
            box.transform.localPosition = GetStackPosition(_boxList.Count);
            box.transform.localRotation = Quaternion.identity;
            box.transform.localScale = _boxPrefab.transform.localScale;

            _boxList.Add(box);
        }

        // 2. ��� ������ �ʰ��Ǹ� ���� (���������� �ϳ���)
        while (_boxList.Count > currentMeat)
        {
            GameObject lastMeat = _boxList[_boxList.Count - 1];
            _boxList.RemoveAt(_boxList.Count - 1);
            _boxPool.ReturnToPool(lastMeat);
        }
    }
    Vector3 GetStackPosition(int index)
    {
        return new Vector3
        (
            _boxSpawnLocation.position.x,
            _boxSpawnLocation.position.y + index * _stackHeight + _counterY,
            _boxSpawnLocation.position.z
        );
    }
    #endregion

}
