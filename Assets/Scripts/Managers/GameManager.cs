using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("�׸�")] public Grill[] _grill;
    [Header("���̺�")] public Table[] _table;
    [Header("���̺�,ī����,ȭ�� �� ���� ����")] public InteractionObject[] _interactObjs; // ī���� , ���̺� , ȭ�ε��� ��� �迭 - level �� ���� ������ ���� ���
    [Header("���� ������ �� ������Ʈ")] public int[] _GroundMoney;

    [Header("��� ������Ʈ monoŸ��")] public MonoBehaviour[] _monoObjectArr; // ��� �رݽ� Ȱ��ȭ �Ǵ� ������Ʈ��
    public List<IActivable> _isActiveObjectArr = new List<IActivable>(); // ��� �رݽ� Ȱ��ȭ �Ǵ� ������Ʈ��

    [Header("�ر� ������Ʈ")] public ObjectsActivator[] _activator; // �ر� ������Ʈ ���� �迭
    [Header("���� �ر� ���� ����")] public int _progress = 0; // ���� �ر� ���� ��Ȳ

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
        //OnUnlockObject(_progress);
        //DataManager._Instance.LoadActivatorData();
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
        if (_activator ==null)
        {
            Debug.Log("null");
        }
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
      

    /// <summary>
    /// MonoBehaviour Ÿ�� �迭 ��ҵ��� List<IActivable> �� �ű�
    /// ��� ����: �������̽� �迭,����Ʈ�� �ν����Ϳ� ������ �ʾƼ� monovihaviour �迭�� �޾� ����
    /// �ΰ��� �� ����
    /// </summary>
    /// 
    public void SettingIsActiveObjectArr()
    {
        for (int i=0; i< _monoObjectArr.Length; i++)
        {
            if (_monoObjectArr[i] is IActivable activable)
            {
                _isActiveObjectArr.Add(activable);
            }
            else
            {
                Debug.LogWarning($"{_monoObjectArr[i].name}�� IActivable�� �������� �ʾҽ��ϴ�.");
            }
           // _isActiveObjectArr.Add((IActivable)_monoObjectArr[i]);
        }

        //foreach (var item in _monoObjectArr)
        //{
        //    _isActiveObjectArr.Add((IActivable)item);
        //}
    }
    //��� ������Ʈ Ȱ��ȭ �� ��Ȱ��ȭ
    // ��ȣ �ۿ� ������Ʈ �迭���� ���鼭 Ȱ��ȭ , ��Ȱ��ȭ ����
    public void createinteractionobject()
    {
        for (int i = 0; i < _isActiveObjectArr.Count; i++)
        {
            if (_isActiveObjectArr[i].isActive() == true)
            {
                _isActiveObjectArr[i].OnActive();
            }
            else
            {
                _isActiveObjectArr[i].DeActive();
            }
        }
    }
}