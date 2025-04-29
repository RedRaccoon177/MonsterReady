using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̺� ������Ʈ (��� �ױ�, �� �ױ�, ���� ���� ��)
/// </summary>
public class Table : BaseObject, ILevelable, INpcDestination
{
    #region ���� ����
    [Header("���̺� �⺻ ����")]
    [SerializeField] public int _level;                   // ���̺� ����
    [SerializeField] public int _currentTrashCount;       // ���� ���� ������(��) ��
    [SerializeField] public Vector2 _nodeGridNum;         // �ش� ���̺��� �׸��� ��ǥ

    [Header("������Ʈ Ǯ�� ����")]
    [SerializeField] ObjectPooling _meatPool;             // ��� Ǯ
    [SerializeField] ObjectPooling _bonePool;             // �� Ǯ

    [Header("���, �� ������")]
    [SerializeField] GameObject _meatPrefab;              // ��� ������
    [SerializeField] GameObject _bonePrefab;              // �� ������

    [Header("���, �� ��ġ ��ġ �� �ױ� ����")]
    [SerializeField] Transform _meatSpawnLocation;        // ���/�� ���� ���� ��ġ
    [SerializeField] float _stackHeight = 0.11f;          // ���̴� ���� ����

    [Header("���� ���/�� ����")]
    [SerializeField] int _meatNum;                        // ��� ����
    [SerializeField] int _boneNum;                        // �� ����

    List<GameObject> _meatList = new List<GameObject>();  // ���� ��� ������Ʈ ����Ʈ
    List<GameObject> _boneList = new List<GameObject>();  // ���� �� ������Ʈ ����Ʈ
    #endregion

    #region Unity �̺�Ʈ �Լ�
    void Start()
    {
        _currentTrashCount = 0;
        SettingNode();        // ���̺� ��ġ�� ��忡 ���
        SettingGMBaseDict();  // ���̺��� GameManager�� ���
    }
    #endregion

    #region ��� ���� ��� �Լ�
    /// <summary>
    /// ���̺� ��� �߰�
    /// </summary>
    public void AddMeat(int amount)
    {
        _meatNum += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject meat = _meatPool.GetMeat();                      // ��� Ǯ���� ������
            meat.transform.SetParent(_meatSpawnLocation);
            meat.transform.localPosition = GetStackAndBonePosition(_meatList.Count); // ���� ��� ����ŭ ���� ����
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

        _meatNum -= 1;
        GameObject lastMeat = _meatList[_meatList.Count - 1];
        _meatList.RemoveAt(_meatList.Count - 1);
        _meatPool.ReturnToPool(lastMeat); // ���ŵ� ��� Ǯ�� ��ȯ
    }

    #endregion

    #region �� ���� ��� �Լ�
    /// <summary>
    /// ��⸦ �� ���� �� ���� �����ؼ� ���̺� �ױ�
    /// </summary>
    public void AddBones(int amount)
    {
        _boneNum += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject bone = _bonePool.GetBone();                     // �� Ǯ���� ������
            bone.transform.SetParent(_meatSpawnLocation);
            bone.transform.localPosition = GetStackAndBonePosition(_meatList.Count + _boneList.Count); // ���+�� ���� ����
            bone.transform.localRotation = Quaternion.identity;
            bone.transform.localScale = _bonePrefab.transform.localScale;

            _boneList.Add(bone);
        }
    }

    /// <summary>
    /// ���̺��� ��� �� ����
    /// </summary>
    public void RemoveBones()
    {
        _boneNum -= 1;
        while (_boneList.Count > 0)
        {
            GameObject bone = _boneList[_boneList.Count - 1];
            _boneList.RemoveAt(_boneList.Count - 1);
            _bonePool.ReturnToPool(bone); // ���ŵ� ���� Ǯ�� ��ȯ
        }
    }
    #endregion

    #region ��� �� ���� �ױ� ��ġ ���� �Լ�
    /// <summary>
    /// ���/���� ���̴� ��ġ�� ���
    /// </summary>
    Vector3 GetStackAndBonePosition(int index)
    {
        float baseHeight = 0.94f; // ���� ���� ��ġ
        return new Vector3
        (
            0f,
            baseHeight + index * _stackHeight,
            0f
        );
    }
    #endregion

    #region ILevelable ����
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

    #region INpcDestination ����
    public bool HasStack()
    {
        return _currentTrashCount > 0;
    }

    public int GetStackCount()
    {
        return _currentTrashCount;
    }
    #endregion

    #region GameManager ��� ���
    /// <summary>
    /// ���̺��� ��ġ�� NodeManager�� ���
    /// </summary>
    public void SettingNode()
    {
        Node _tempNode = NodeManager._instance._nodeList[(int)_nodeGridNum.x, (int)_nodeGridNum.y];
        GameManager._instance._npcObjectNodeDict.TryAdd(_keyName, _tempNode);
    }

    /// <summary>
    /// ���̺� ������Ʈ�� GameManager�� ���
    /// </summary>
    public void SettingGMBaseDict()
    {
        GameManager._instance._baseObjectDict.TryAdd(_keyName, this);
    }
    #endregion
}


