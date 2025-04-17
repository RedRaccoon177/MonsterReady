using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance {  get; private set; }
    [Header("�÷��̾�")] public GameObject _player;
    [Header("�׸�")] public Grill[] _grill;
    [Header("���̺�")] public Table[] _table;
    [Header("��ȣ ���� ������Ʈ")] public InteractionObject[] _interactObjs;
    //[Header("���� ������ �� ������Ʈ")] public InteractionObject[] _GroundMoney;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
    }
}
