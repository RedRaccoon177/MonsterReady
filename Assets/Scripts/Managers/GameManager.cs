using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("플레이어")] public GameObject _player;
    [Header("그릴")] public Grill[] _grill;
    [Header("테이블")] public Table[] _table;
    [Header("상호 가능 오브젝트")] public InteractionObject[] _interactObjs;
    //[Header("땅에 떨어진 돈 오브젝트")] public InteractionObject[] _GroundMoney;

    [Header("매장 문")] public Door _isOpenDoor;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    //모든 오브젝트 활성화 및 비활성화
    public void ActiveAllObject()
    {
        //문 활성화
        _isOpenDoor.SetActiveObj(true);
    }
}