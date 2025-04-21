using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance {  get; private set; }
    [Header("정보(레벨)를 가진 모든 오브젝트")] public MonoBehaviour[] _monoObjectArr; // 정보를 가진 모든 오브젝트
    [Header("땅에 떨어진 돈 오브젝트 배열")] public GoldObject[] _groundMoneyArr;      // 땅에 떨어진 돈 오브젝트
    [Header("활성화 가능한 모든 오브젝트 배열")] public BaseObject[] _isActiveObjectArr;// 활성화 가능한 모든 오브젝트
    [Header("해금 오브젝트")] public ObjectsActivator[] _activator; // 해금 오브젝트 관리 배열
    [Header("현재 해금 진행 상태")] public int _progress = 0; // 현재 해금 진행 상황
    public List<ILevelable> _iLevelObject = new List<ILevelable>(); // 카운터 , 테이블 , 화로들을 담는 배열 - level 등 정보 관리를 위해 사용

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
        DataManager._Instance.LoadActivatorData();
        DataManager._Instance.LoadActiveObjectData();
        DataManager._Instance.LoadObjectData();
        //OnUnlockObject(0);
    }

    /// <summary>
    /// 다음 해금 오브젝트 활성화
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
        for (int i = 0; i < _monoObjectArr.Length; i++)
        {
            _iLevelObject.Add((ILevelable)_monoObjectArr[i]);
        }
    }
    //모든 오브젝트 활성화 및 비활성화
    // 상호 작용 오브젝트 배열들을 돌면서 활성화 , 비활성화 해줌
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