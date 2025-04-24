using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("땅에 떨어진 돈 오브젝트 배열")] public GoldObject[] _groundMoneyArr;      // 땅에 떨어진 돈 오브젝트
    [Header("해금 오브젝트")] public ObjectsActivator[] _activator; // 해금 오브젝트 관리 배열

    public Table[] _tables;
    public Counter[] _counters;
    public Grill[] _grills;
    // 테이블,카운터,그릴에 쌓인게 있는지 
    public List<IStackChecker> _stackableObjectList = new List<IStackChecker>();
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
        //OnUnlockObject(0); // 처음 부터 시작

        DataManager._Instance.LoadActivatorData();
        DataManager._Instance.LoadObjectData(ObjectType.Table);
        DataManager._Instance.LoadObjectData(ObjectType.Grill);
        DataManager._Instance.LoadObjectData(ObjectType.Counter);
        DataManager._Instance.LoadObjectData(ObjectType.Expand);
        DataManager._Instance.LoadGroundMoney();
        SettingStackableObjectList();
    }
    public void SettingStackableObjectList()
    {
        for (int i= 0; i< _tables.Length; i++)
        {
            _stackableObjectList.Add(_tables[i]);
        }
        for (int i = 0; i < _counters.Length; i++)
        {
            _stackableObjectList.Add(_counters[i]);
        }
        for (int i = 0; i < _grills.Length; i++)
        {
            _stackableObjectList.Add(_grills[i]);
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