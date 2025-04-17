using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("���� ��")] public Door _isOpenDoor;

    //[Header("�׸� Ȱ��ȭ ��Ȱ��ȭ")] public bool[] _isOpenTable;
    //[Header("�׸� ������Ʈ")][SerializeField] GameObject[] _grill;
    
    //[Header("���� Ȱ��ȭ ��Ȱ��ȭ")][SerializeField] bool[] _isOpenCounter;
    //[Header("���� ������Ʈ")][SerializeField] GameObject[] _counter;

    //[Header("NPC ������ Ȱ��ȭ ��Ȱ��ȭ")][SerializeField] bool[] _isOpenNpc;
    //[Header("NPC ������ ������Ʈ")][SerializeField] GameObject[] _npc;

    //[Header("��Ÿ ������Ʈ�� Ȱ��ȭ ��Ȱ��ȭ")][SerializeField] bool[] _isOpenOtherObj;
    //[Header("��Ÿ ������Ʈ")][SerializeField] GameObject[] _otherObj;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void ActiveAllObject()
    {
        //�� Ȱ��ȭ
        _isOpenDoor.SetActiveObj(true);

    }
}
