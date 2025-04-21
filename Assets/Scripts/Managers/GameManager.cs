using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("���� ������ �� ������Ʈ �迭")] public GoldObject[] _groundMoneyArr;      // ���� ������ �� ������Ʈ
    [Header("�ر� ������Ʈ")] public ObjectsActivator[] _activator; // �ر� ������Ʈ ���� �迭
    [Header("���� �ر� ���� ����")] public int _progress = 0; // ���� �ر� ���� ��Ȳ

    public Table[] _tables;
    public Counter[] _counters;
    public Grill[] _grills;
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
    }

    /// <summary>
    /// ���� �ر� ������Ʈ Ȱ������
    /// </summary>
    /// <param name="_step"></param>
    public void OnUnlockObject(int _step)
    {
        if (_step >= _activator.Length) { return; }
        _activator[_step].gameObject.SetActive(true);
        _activator[_step]._isActive = true;
        Debug.Log(_activator[_step].name);
        Debug.Log(_activator[_step]._isActive);
        DataManager._Instance.SaveTableData(_tables,ObjectType.Table);
        DataManager._Instance.SaveTableData(_grills, ObjectType.Grill);
        DataManager._Instance.SaveTableData(_counters, ObjectType.Counter);
        DataManager._Instance.SaveTableData(_expens, ObjectType.Expand);
        DataManager._Instance.SaveActivatorData(_activator);
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