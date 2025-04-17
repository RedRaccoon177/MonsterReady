using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("매장 문")] public Door _isOpenDoor;

    //[Header("그릴 활성화 비활성화")] public bool[] _isOpenTable;
    //[Header("그릴 오브젝트")][SerializeField] GameObject[] _grill;
    
    //[Header("계산대 활성화 비활성화")][SerializeField] bool[] _isOpenCounter;
    //[Header("계산대 오브젝트")][SerializeField] GameObject[] _counter;

    //[Header("NPC 종업원 활성화 비활성화")][SerializeField] bool[] _isOpenNpc;
    //[Header("NPC 종업원 오브젝트")][SerializeField] GameObject[] _npc;

    //[Header("기타 오브젝트들 활성화 비활성화")][SerializeField] bool[] _isOpenOtherObj;
    //[Header("기타 오브젝트")][SerializeField] GameObject[] _otherObj;

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
        //문 활성화
        _isOpenDoor.SetActiveObj(true);

    }
}
