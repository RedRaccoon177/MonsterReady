using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : BaseObject, ILevelable,INpcDestination
{
    [SerializeField] public int _level;
    [SerializeField] public int _currentTrashCount;
    [SerializeField] public Vector2 _nodeGridNum;

    #region ���, �� �ð�ȭ ����
    [Header("������Ʈ Ǯ�� ����")]
    [SerializeField] ObjectPooling _meatPool;  // ��� Ǯ
    [SerializeField] ObjectPooling _bonePool;  // �� Ǯ

    [Header("���, �� ������")]
    [SerializeField] GameObject _meatPrefab;
    [SerializeField] GameObject _bonePrefab;

    [Header("���, �� ��ġ�ϴ� ��ġ")]
    [SerializeField] Transform _meatSpawnLocation; // ���� ��ġ (���, �� ����)

    [Header("���� ���� ����")]
    [SerializeField] float _stackHeight = 0.11f;

    List<GameObject> _meatList = new List<GameObject>(); // ��� ����Ʈ
    List<GameObject> _boneList = new List<GameObject>(); // �� ����Ʈ
    #endregion


    private void Start()
    {
        _currentTrashCount = 0;
        SettingNode();
        SettingGMBaseDict();
    }

    /// <summary>
    /// ���̺� ��� �߰�
    /// </summary>
    public void AddMeat(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject meat = _meatPool.GetMeat(); // Ǯ���� ����
            meat.transform.SetParent(_meatSpawnLocation);
            meat.transform.localPosition = GetStackPosition(_meatList.Count);
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }
    }

    /// <summary>
    /// ���̺��� ��� ����
    /// </summary>
    public void RemoveMeat()
    {
        if (_meatList.Count == 0) return;

        GameObject lastMeat = _meatList[_meatList.Count - 1];
        _meatList.RemoveAt(_meatList.Count - 1);
        _meatPool.ReturnToPool(lastMeat);
    }

    /// <summary>
    /// ��⸦ �� ���� �� ���� �����ؼ� ���̺� �״´�
    /// </summary>
    public void AddBones(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject bone = _bonePool.GetBone();
            bone.transform.SetParent(_meatSpawnLocation);
            bone.transform.localPosition = GetStackPosition(_meatList.Count + _boneList.Count); // ���� ���+�� ���� ��������
            bone.transform.localRotation = Quaternion.identity;
            bone.transform.localScale = _bonePrefab.transform.localScale;

            _boneList.Add(bone);
        }
    }

    public void RemoveBones()
    {
        while (_boneList.Count > 0)
        {
            GameObject bone = _boneList[_boneList.Count - 1];
            _boneList.RemoveAt(_boneList.Count - 1);
            _bonePool.ReturnToPool(bone);
        }
    }

    Vector3 GetStackPosition(int index)
    {
        return new Vector3
        (
            0f,
            index * _stackHeight,
            0f
        );
    }

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
    public bool HasStack()
    {
        return _currentTrashCount > 0;
    }
    public int GetStackCount()
    {
        return _currentTrashCount;
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
