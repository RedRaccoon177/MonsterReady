using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 테이블 오브젝트 (고기 쌓기, 뼈 쌓기, 레벨 관리 등)
/// </summary>
public class Table : BaseObject, ILevelable, INpcDestination
{
    #region 변수 모음
    [Header("테이블 기본 변수")]
    [SerializeField] public int _level;                   // 테이블 레벨
    [SerializeField] public int _currentTrashCount;       // 현재 쌓인 쓰레기(뼈) 수
    [SerializeField] public Vector2 _nodeGridNum;         // 해당 테이블의 그리드 좌표

    [Header("오브젝트 풀링 연결")]
    [SerializeField] ObjectPooling _meatPool;             // 고기 풀
    [SerializeField] ObjectPooling _bonePool;             // 뼈 풀

    [Header("고기, 뼈 프리팹")]
    [SerializeField] GameObject _meatPrefab;              // 고기 프리팹
    [SerializeField] GameObject _bonePrefab;              // 뼈 프리팹

    [Header("고기, 뼈 배치 위치 및 쌓기 간격")]
    [SerializeField] Transform _meatSpawnLocation;        // 고기/뼈 스폰 기준 위치
    [SerializeField] float _stackHeight = 0.11f;          // 쌓이는 간격 높이

    [Header("현재 고기/뼈 개수")]
    [SerializeField] int _meatNum;                        // 고기 개수
    [SerializeField] int _boneNum;                        // 뼈 개수

    List<GameObject> _meatList = new List<GameObject>();  // 쌓인 고기 오브젝트 리스트
    List<GameObject> _boneList = new List<GameObject>();  // 쌓인 뼈 오브젝트 리스트
    #endregion

    #region Unity 이벤트 함수
    void Start()
    {
        _currentTrashCount = 0;
        SettingNode();        // 테이블 위치를 노드에 등록
        SettingGMBaseDict();  // 테이블을 GameManager에 등록
    }
    #endregion

    #region 고기 관련 기능 함수
    /// <summary>
    /// 테이블에 고기 추가
    /// </summary>
    public void AddMeat(int amount)
    {
        _meatNum += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject meat = _meatPool.GetMeat();                      // 고기 풀에서 꺼내기
            meat.transform.SetParent(_meatSpawnLocation);
            meat.transform.localPosition = GetStackAndBonePosition(_meatList.Count); // 현재 고기 수만큼 높이 조정
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
        _meatPool.ReturnToPool(lastMeat); // 제거된 고기 풀로 반환
    }

    #endregion

    #region 뼈 관련 기능 함수
    /// <summary>
    /// 고기를 다 먹은 후 뼈를 생성해서 테이블에 쌓기
    /// </summary>
    public void AddBones(int amount)
    {
        _boneNum += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject bone = _bonePool.GetBone();                     // 뼈 풀에서 꺼내기
            bone.transform.SetParent(_meatSpawnLocation);
            bone.transform.localPosition = GetStackAndBonePosition(_meatList.Count + _boneList.Count); // 고기+뼈 기준 높이
            bone.transform.localRotation = Quaternion.identity;
            bone.transform.localScale = _bonePrefab.transform.localScale;

            _boneList.Add(bone);
        }
    }

    /// <summary>
    /// 테이블의 모든 뼈 제거
    /// </summary>
    public void RemoveBones()
    {
        _boneNum -= 1;
        while (_boneList.Count > 0)
        {
            GameObject bone = _boneList[_boneList.Count - 1];
            _boneList.RemoveAt(_boneList.Count - 1);
            _bonePool.ReturnToPool(bone); // 제거된 뼈를 풀로 반환
        }
    }
    #endregion

    #region 고기 뼈 관련 쌓기 위치 지정 함수
    /// <summary>
    /// 고기/뼈가 쌓이는 위치를 계산
    /// </summary>
    Vector3 GetStackAndBonePosition(int index)
    {
        float baseHeight = 0.94f; // 최초 스폰 위치
        return new Vector3
        (
            0f,
            baseHeight + index * _stackHeight,
            0f
        );
    }
    #endregion

    #region ILevelable 구현
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

    #region INpcDestination 구현
    public bool HasStack()
    {
        return _currentTrashCount > 0;
    }

    public int GetStackCount()
    {
        return _currentTrashCount;
    }
    #endregion

    #region GameManager 등록 기능
    /// <summary>
    /// 테이블의 위치를 NodeManager에 등록
    /// </summary>
    public void SettingNode()
    {
        Node _tempNode = NodeManager._instance._nodeList[(int)_nodeGridNum.x, (int)_nodeGridNum.y];
        GameManager._instance._npcObjectNodeDict.TryAdd(_keyName, _tempNode);
    }

    /// <summary>
    /// 테이블 오브젝트를 GameManager에 등록
    /// </summary>
    public void SettingGMBaseDict()
    {
        GameManager._instance._baseObjectDict.TryAdd(_keyName, this);
    }
    #endregion
}


