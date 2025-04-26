using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : BaseObject, ILevelable, INpcDestination
{
    #region Ű�� �� ����
    [SerializeField] public int _level;
    [SerializeField] public Vector2 _nodeGridNum;
    public string GetKey()
    {
        return _keyName;
    }

    public int SetLevel(int level)
    {
        _level = level;
        return level;
    }

    public void LevelUp()
    {
        _level++;
    }

    public int GetLevel()
    {
        return _level;
    }
    #endregion

    #region ������
    [Header("������Ʈ Ǯ�� ����")]
    [SerializeField] ObjectPooling _meatPool; // ��⸦ �����ϴ� ������Ʈ Ǯ

    [Header("��� ������")]
    [SerializeField] GameObject _meatPrefab;

    [Header("��� ��ġ�ϴ� ��")]
    [SerializeField] Transform _meatSpawnLocation;

    [Header("���� ��� ����")]
    [SerializeField] public int _currentMeatCount = 0;

    [Header("��� ���� ���� ����")]
    [SerializeField] float _stackHeight = 0.11f;
    [SerializeField] float _counterY = 1.65f;

    // ������ ��� ������Ʈ���� ��� ����Ʈ
    List<GameObject> _meatList = new List<GameObject>();

    [Header("NPCAI ī���� ������")] public Node _myNode; //npc �������� ������ ī���� ��� 

    //�÷��̾� ����
    PlayerController _player;

    [Header("ī���� ���� �޸� ����")]
    [SerializeField] GoldObject _goldObject;

    public Vector2 NodePosition => throw new System.NotImplementedException();

    #endregion

    void Start()
    {
        _player = PlayerController._instance;
        SettingNode();
        SettingGMBaseDict();
    }

    #region ��� ���� �� ����
    /// <summary>
    /// ī���� ��� ���� �Լ�
    /// </summary>
    /// <param name="_meatCount"></param>
    void AddMeat(int _meatCount)
    {
        _currentMeatCount = Mathf.Max(0, _currentMeatCount + _meatCount);
        UpdateMeatDisplay(_currentMeatCount);
    }

    /// <summary>
    /// ī���� ��� ����
    /// </summary>
    /// <param name="_minusMeat"></param>
    public int MinusMeat(int _minusMeat)
    {
        int _someoneGetMeat;

        if (_currentMeatCount < _minusMeat)
        {
            _someoneGetMeat = _currentMeatCount;
            _currentMeatCount = 0;
        }
        else
        {
            _someoneGetMeat = _minusMeat;
            _currentMeatCount -= _minusMeat;
        }

        UpdateMeatDisplay(_currentMeatCount);
        return _someoneGetMeat;
    }
    #endregion

    #region ��� �ð�ȭ �ϴ� �ڵ� ����
    //��� �ð�ȭ �ϴ� �ڵ�
    public void UpdateMeatDisplay(int currentMeat)
    {
        // 1. ��� ������ �����ϸ� ä����
        while (_meatList.Count < currentMeat)
        {
            GameObject meat = _meatPool.GetMeat(); // ������Ʈ Ǯ���� ����

            // ���� ��ġ��, ȸ����, ũ�Ⱚ
            meat.transform.localPosition = GetStackPosition(_meatList.Count);
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }

        // 2. ��� ������ �ʰ��Ǹ� ���� (���������� �ϳ���)
        while (_meatList.Count > currentMeat)
        {
            GameObject lastMeat = _meatList[_meatList.Count - 1];
            _meatList.RemoveAt(_meatList.Count - 1);
            _meatPool.ReturnToPool(lastMeat);
        }
    }
    Vector3 GetStackPosition(int index)
    {
        return new Vector3
        (
            _meatSpawnLocation.position.x,
            _meatSpawnLocation.position.y + index * _stackHeight + _counterY,
            _meatSpawnLocation.position.z
        );
    }
    #endregion

    // �÷��̾ ������ ������ �� ��� �ڵ� ����
    private void OnTriggerEnter(Collider other)
    {
        // �±װ� Player�� �ƴ� ��� ����
        if (!other.CompareTag("Player")) return;

        //�÷��̾��� ������ �������� ���ؾ� �� ���
        if (other.CompareTag("Player"))
        {
            if (0 != _player._CurrentMeat)
            {
                AddMeat( _player._CurrentMeat );
                _player.MinusMeat(_currentMeatCount);
            }
        }
        else if (other.CompareTag("NPC"))
        {
            //TODO: NPC ĳ���͵� ��� ȹ��
        }
    }

    public bool HasStack()
    {
        return _currentMeatCount > 0;
    }

    public int GetStackCount()
    {
        return _currentMeatCount;
    }
    public void SettingNode()
    {
        Node _tempNode = NodeManager._instance._nodeList[(int)_nodeGridNum.x, (int)_nodeGridNum.y];
        GameManager._instance._npcObjectNodeDict.TryAdd(_keyName, _tempNode);
    }
    public void SettingGMBaseDict()
    {
        GameManager._instance._baseObjectDict.TryAdd(_keyName, this);
    }
}
