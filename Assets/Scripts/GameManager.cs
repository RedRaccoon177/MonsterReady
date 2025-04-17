using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance {  get; private set; }
    [Header("플레이어")] public GameObject _player;
    [Header("그릴")] public Grill[] _grill;
    [Header("테이블")] public Table[] _table;
    [Header("상호 가능 오브젝트")] public InteractionObject[] _interactObjs;
    //[Header("땅에 떨어진 돈 오브젝트")] public InteractionObject[] _GroundMoney;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
    }
}
