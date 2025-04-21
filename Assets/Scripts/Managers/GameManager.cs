using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("그릴")] public Grill[] _grill;
    [Header("테이블")] public Table[] _table;
    [Header("테이블,카운터,화로 등 정보 관리")] public InteractionObject[] _interactObjs; // 카운터 , 테이블 , 화로들을 담는 배열 - level 등 정보 관리를 위해 사용
    [Header("땅에 떨어진 돈 오브젝트")] public int[] _GroundMoney;

    [Header("모든 오브젝트 mono타입")] public MonoBehaviour[] _monoObjectArr; // 모든 해금시 활성화 되는 오브젝트들
    public List<IActivable> _isActiveObjectArr = new List<IActivable>(); // 모든 해금시 활성화 되는 오브젝트들

    [Header("해금 오브젝트")] public ObjectsActivator[] _activator; // 해금 오브젝트 관리 배열
    [Header("현재 해금 진행 상태")] public int _progress = 0; // 현재 해금 진행 상황

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } 
    }
    private void Start()
    {
        SettingIsActiveObjectArr(); // mono배열 바꾸기
        SettingActivatorArray(); // 해금 오브젝트 순서대로 오름차순 정렬
        //OnUnlockObject(_progress);
        //DataManager._Instance.LoadActivatorData();
    }

    /// <summary>
    /// 다음 해금 오브젝트 활서오하
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
    /// 각 해금 오브젝트가 가지고 있는 step 변수를 이용해 오름차순 정렬하는 함수
    ///  - 오름차순 정렬 IntroSort 방식
    ///  - 버블,퀵,삽입 중 상황에 따라 사용되는 정렬 로직 변경됨
    /// </summary>  
    public void SettingActivatorArray()
    {
        Array.Sort(_activator, (a, b) => a._step.CompareTo(b._step));    
    }
      

    /// <summary>
    /// MonoBehaviour 타입 배열 요소들을 List<IActivable> 로 옮김
    /// 사용 이유: 인터페이스 배열,리스트는 인스펙터에 나오지 않아서 monovihaviour 배열로 받아 놓고
    /// 인게임 때 변경
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
                Debug.LogWarning($"{_monoObjectArr[i].name}은 IActivable을 구현하지 않았습니다.");
            }
           // _isActiveObjectArr.Add((IActivable)_monoObjectArr[i]);
        }

        //foreach (var item in _monoObjectArr)
        //{
        //    _isActiveObjectArr.Add((IActivable)item);
        //}
    }
    //모든 오브젝트 활성화 및 비활성화
    // 상호 작용 오브젝트 배열들을 돌면서 활성화 , 비활성화 해줌
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