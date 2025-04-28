using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Table : BaseObject, ILevelable,INpcDestination
{
    [SerializeField] public int _level;
    [SerializeField] public int _currentTrashCount;
    [SerializeField] public Vector2 _nodeGridNum;

    #region 고기, 뼈 시각화 변수
    [Header("오브젝트 풀링 연결")]
    [SerializeField] ObjectPooling _meatPool;  // 고기 풀
    [SerializeField] ObjectPooling _bonePool;  // 뼈 풀

    [Header("고기, 뼈 프리팹")]
    [SerializeField] GameObject _meatPrefab;
    [SerializeField] GameObject _bonePrefab;

    [Header("고기, 뼈 배치하는 위치")]
    [SerializeField] Transform _meatSpawnLocation; // 기준 위치 (고기, 뼈 공용)

    [Header("쌓을 높이 간격")]
    [SerializeField] float _stackHeight = 0.11f;

    List<GameObject> _meatList = new List<GameObject>(); // 고기 리스트
    List<GameObject> _boneList = new List<GameObject>(); // 뼈 리스트

    [Header("고기와 뼈 숫자")]
    [SerializeField] int _meatNum;
    [SerializeField] int _boneNum;
    #endregion

    private void Start()
    {
        _currentTrashCount = 0;
        SettingNode();
        SettingGMBaseDict();
    }

    /// <summary>
    /// 테이블에 고기 추가
    /// </summary>
    public void AddMeat(int amount)
    {
        _meatNum += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject meat = _meatPool.GetMeat(); // 풀에서 꺼냄
            meat.transform.SetParent(_meatSpawnLocation);
            meat.transform.localPosition = GetStackAndBonePosition(_meatList.Count);
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }
    }

    /// <summary>
    /// 테이블에서 고기 제거
    /// </summary>
    public void RemoveMeat()
    {
        if (_meatList.Count == 0) return;

        _meatNum -= 1;
        GameObject lastMeat = _meatList[_meatList.Count - 1];
        _meatList.RemoveAt(_meatList.Count - 1);
        _meatPool.ReturnToPool(lastMeat);
    }

    /// <summary>
    /// 고기를 다 먹은 후 뼈를 생성해서 테이블에 쌓는다
    /// </summary>
    public void AddBones(int amount)
    {
        _boneNum += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject bone = _bonePool.GetBone();
            bone.transform.SetParent(_meatSpawnLocation);
            bone.transform.localPosition = GetStackAndBonePosition(_meatList.Count + _boneList.Count); // 현재 고기+뼈 개수 기준으로
            bone.transform.localRotation = Quaternion.identity;
            bone.transform.localScale = _bonePrefab.transform.localScale;

            _boneList.Add(bone);
        }
    }

    public void RemoveBones()
    {
        _boneNum -=1 ;
        while (_boneList.Count > 0)
        {
            GameObject bone = _boneList[_boneList.Count - 1];
            _boneList.RemoveAt(_boneList.Count - 1);
            _bonePool.ReturnToPool(bone);
        }
    }

    Vector3 GetStackAndBonePosition(int index)
    {
        float baseHeight = 0.94f; // 시작 y위치
        return new Vector3
        (
            0f,
            baseHeight + index * _stackHeight,
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
