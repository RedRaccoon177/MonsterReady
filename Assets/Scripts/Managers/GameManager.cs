using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("���� ������ �� ������Ʈ �迭")] public GoldObject[] _groundMoneyArr;      // ���� ������ �� ������Ʈ
    [Header("�ر� ������Ʈ")] public ObjectsActivator[] _activator; // �ر� ������Ʈ ���� �迭

    [Header("���̺� ������Ʈ")] public Table[] _tables;
    [Header("ī���� ������Ʈ")] public Counter[] _counters;
    [Header("�׸� ������Ʈ")] public Grill[] _grills;

    public List<INpcDestination> _warkableObjectList = new List<INpcDestination>();// npc�� �������� ���� ������ ������Ʈ ����Ʈ
    // ���̺�,ī����3,�׸�1,2�� ���ΰ� �ִ��� 
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
        SettingActivatorArray(); // �ر� ������Ʈ ������� �������� ����
        SettingWarableList();
        SettingGame();


    }

    /// <summary>
    /// ���� ó�� �����Ҷ� �ʱ�ȭ , ó���� �ƴ϶�� ������ �ε�
    /// </summary>
    public void SettingGame()
    {
        //OnUnlockObject(0); // ó�� ���� ����
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
    /// npc�� ������ �ִ��� ��ȸ �ؾ��ϴ� ����Ʈ �ʱ�ȭ
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