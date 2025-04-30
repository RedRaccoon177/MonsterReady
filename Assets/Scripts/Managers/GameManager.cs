using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("땅에 떨어진 돈 오브젝트 배열")] public GoldObject[] _groundMoneyArr;      // 땅에 떨어진 돈 오브젝트
    [Header("해금 오브젝트")] public ObjectsActivator[] _activator; // 해금 오브젝트 관리 배열

    [Header("테이블 오브젝트")] public Table[] _tables;
    [Header("카운터 오브젝트")] public Counter[] _counters;
    [Header("그릴 오브젝트")] public Grill[] _grills;

    public List<INpcDestination> _warkableObjectList = new List<INpcDestination>();// npc가 목적지로 설정 가능한 오브젝트 리스트
    // 테이블,카운터3,그릴1,2에 쌓인게 있는지 
    public Dictionary<string,Node> _npcObjectNodeDict = new Dictionary<string  , Node>();
    public Dictionary<string,BaseObject> _baseObjectDict = new Dictionary<string  , BaseObject>();
    public Expen[] _expens;
    

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } 
    }
    private void Start()
    {
        SettingActivatorArray(); // 해금 오브젝트 순서대로 오름차순 정렬
        SettingWarableList();
        SettingGame();


    }

    /// <summary>
    /// 게임 처음 시작할때 초기화 , 처음이 아니라면 데이터 로딩
    /// </summary>
    public void SettingGame()
    {
        //OnUnlockObject(0); // 처음 부터 시작
        //foreach (var a in _tables)
        //{
        //    a.DeActive();
        //}
        //foreach (var a in _grills)
        //{
        //    a.DeActive();
        //}
        //foreach (var a in _counters)
        //{
        //    a.DeActive();
        //}
        //foreach (var a in _expens)
        //{
        //    a.OnActive();
        //}

        //DataManager._Instance.LoadActivatorData();
        //DataManager._Instance.LoadObjectData(ObjectType.Table);
        //DataManager._Instance.LoadObjectData(ObjectType.Grill);
        //DataManager._Instance.LoadObjectData(ObjectType.Counter);
        //DataManager._Instance.LoadObjectData(ObjectType.Expand);
        //DataManager._Instance.LoadGroundMoney();
    }
    /// <summary>
    /// npc가 할일이 있는지 조회 해야하는 리스트 초기화
    /// </summary>
    public void SettingWarableList()
    {
        for (int i = 0; i < _tables.Length; i++)
        {
            _warkableObjectList.Add(_tables[i]);
        }
        for (int i = 0; i < _counters.Length; i++)
        {
            _warkableObjectList.Add(_counters[i]);
        }
        for (int i = 0; i < _grills.Length; i++)
        {
            _warkableObjectList.Add(_grills[i]);
        }
    }
    /// <summary>
    /// 다음 해금 오브젝트 활성화
    /// </summary>
    /// <param name="_step"></param>
    public void OnUnlockObject(int _step)
    {
        if (_step >= _activator.Length) { return; }
        _activator[_step].gameObject.SetActive(true);
        _activator[_step]._isActive = true;
        Debug.Log(_activator[_step].name);
        Debug.Log(_activator[_step]._isActive);
        //DataManager._Instance.SaveGroundMoney(_groundMoneyArr);
        //DataManager._Instance.SaveObjectData(_tables,ObjectType.Table);
        //DataManager._Instance.SaveObjectData(_grills, ObjectType.Grill);
        //DataManager._Instance.SaveObjectData(_counters, ObjectType.Counter);
        //DataManager._Instance.SaveObjectData(_expens, ObjectType.Expand);
        //DataManager._Instance.SaveActivatorData(_activator);
    }
    
    /// <summary>
    /// 각 해금 오브젝트가 가지고 있는 step 변수를 이용해 오름차순 정렬하는 함수
    ///  - 오름차순 정렬 IntroSort 방식
    ///  - 버블,퀵,삽입 중 상황에 따라 사용되는 정렬 로직 변경됨
    /// </summary>  
    public void SettingActivatorArray()
    {
        Array.Sort(_activator, (a, b) => a._step.CompareTo(b._step));    
    }

}