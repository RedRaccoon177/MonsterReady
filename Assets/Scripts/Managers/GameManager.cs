using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("그릴")] public Grill[] _grill;
    [Header("테이블")] public Table[] _table;
    [Header("상호 가능 오브젝트")] public InteractionObject[] _interactObjs; // 카운터 , 테이블 , 화로들을 담는 배열 - level 등 정보 관리를 위해 사용
    [Header("땅에 떨어진 돈 오브젝트")] public int[] _GroundMoney;

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

    // 상호 작용 오브젝트 배열들을 돌면서 활성화 , 비활성화 해줌
    public void CreateInteractionObject()
    {
        for (int i=0; i< _interactObjs.Length; i++)
        {
            if (_interactObjs[i].objectIsActive == true)
            {
                _interactObjs[i].gameObject.SetActive(true);
            }
            else
            {
                _interactObjs[i].gameObject.SetActive(false);
            }
        }
    }
}