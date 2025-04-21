using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("����(����)�� ���� ��� ������Ʈ")] public MonoBehaviour[] _monoObjectArr; // ������ ���� ��� ������Ʈ
    [Header("���� ������ �� ������Ʈ �迭")] public GoldObject[] _groundMoneyArr;      // ���� ������ �� ������Ʈ
    [Header("Ȱ��ȭ ������ ��� ������Ʈ �迭")] public BaseObject[] _isActiveObjectArr;// Ȱ��ȭ ������ ��� ������Ʈ
    [Header("�ر� ������Ʈ")] public ObjectsActivator[] _activator; // �ر� ������Ʈ ���� �迭
    [Header("���� �ر� ���� ����")] public int _progress = 0; // ���� �ر� ���� ��Ȳ
    public List<ILevelable> _iLevelObject = new List<ILevelable>(); // ī���� , ���̺� , ȭ�ε��� ��� �迭 - level �� ���� ������ ���� ���

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } 
    }
    private void Start()
    {
        SettingIsActiveObjectArr(); // mono�迭 �ٲٱ�
        SettingActivatorArray(); // �ر� ������Ʈ ������� �������� ����
        DataManager._Instance.LoadActivatorData();
        DataManager._Instance.LoadActiveObjectData();
        DataManager._Instance.LoadObjectData();
        //OnUnlockObject(0);
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
        DataManager._Instance.SaveActivatorData(_activator);
        DataManager._Instance.SaveActiveObjectData(_isActiveObjectArr);
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

    /// <summary>
    /// MonoBehaviour Ÿ�� �迭 ��ҵ��� List<IActivable> �� �ű�
    /// ��� ����: �������̽� �迭,����Ʈ�� �ν����Ϳ� ������ �ʾƼ� monovihaviour �迭�� �޾� ����
    /// �ΰ��� �� ����
    /// </summary>
    /// 
    public void SettingIsActiveObjectArr()
    {
        for (int i = 0; i < _monoObjectArr.Length; i++)
        {
            _iLevelObject.Add((ILevelable)_monoObjectArr[i]);
        }
    }
    //��� ������Ʈ Ȱ��ȭ �� ��Ȱ��ȭ
    // ��ȣ �ۿ� ������Ʈ �迭���� ���鼭 Ȱ��ȭ , ��Ȱ��ȭ ����
    //public void createinteractionobject()
    //{
    //    for (int i = 0; i < _isActiveObjectArr.Count; i++)
    //    {
    //        if (_isActiveObjectArr[i].isActive() == true)
    //        {
    //            _isActiveObjectArr[i].OnActive();
    //        }
    //        else
    //        {
    //            _isActiveObjectArr[i].DeActive();
    //        }
    //    }
    //}
}