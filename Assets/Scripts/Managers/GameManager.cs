using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("�÷��̾�")] public GameObject _player;
    [Header("�׸�")] public Grill[] _grill;
    [Header("���̺�")] public Table[] _table;
    [Header("��ȣ ���� ������Ʈ")] public InteractionObject[] _interactObjs;
    //[Header("���� ������ �� ������Ʈ")] public InteractionObject[] _GroundMoney;

    [Header("���� ��")] public Door _isOpenDoor;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    //��� ������Ʈ Ȱ��ȭ �� ��Ȱ��ȭ
    public void ActiveAllObject()
    {
        //�� Ȱ��ȭ
        _isOpenDoor.SetActiveObj(true);
    }
}