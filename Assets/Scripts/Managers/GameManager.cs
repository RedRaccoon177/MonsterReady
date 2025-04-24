using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("���� ������ �� ������Ʈ �迭")] public GoldObject[] _groundMoneyArr;      // ���� ������ �� ������Ʈ
    [Header("�ر� ������Ʈ")] public ObjectsActivator[] _activator; // �ر� ������Ʈ ���� �迭

    public Table[] _tables;
    public Counter[] _counters;
    public Grill[] _grills;
    // ���̺�,ī����,�׸��� ���ΰ� �ִ��� 
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
        SettingActivatorArray(); // �ر� ������Ʈ ������� �������� ����
        //OnUnlockObject(0); // ó�� ���� ����

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
    /// ���� �ر� ������Ʈ Ȱ��ȭ
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
    /// �� �ر� ������Ʈ�� ������ �ִ� step ������ �̿��� �������� �����ϴ� �Լ�
    ///  - �������� ���� IntroSort ���
    ///  - ����,��,���� �� ��Ȳ�� ���� ���Ǵ� ���� ���� �����
    /// </summary>  
    public void SettingActivatorArray()
    {
        Array.Sort(_activator, (a, b) => a._step.CompareTo(b._step));    
    }

}