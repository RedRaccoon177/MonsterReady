using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public enum PlayerPickUpObject
{
    None,Meat,Bone,MeatBox
}

public class PlayerController : MonoBehaviour
{
    #region 싱글톤 및 이벤트
    public static PlayerController _instance { get; private set; }  // 플레이어 싱글톤
    public static event Action OnJoystickReleased;                  //조이 스틱 이벤트
    public static event Action OnJoystickRelePerformed;                  //조이 스틱 이벤트
    public static event Action<int> OnGoldChanged;                  // 골드 변화 이벤트
    #endregion

    #region 플레이어 변수들
    // 플레이어가 들 수 있는 최대 아이템 개수
    int _holdMaxAmount;

    // 수익을 저장하는 변수
    float _revenue;

    // 조이스틱 입력 참조용 (FloatingJoystick 연결)
    public FloatingJoystick _joy;
    public PlayerPickUpObject playerPickUpObject;
    // 이동 속도 조절 변수
    public float _moveSpeed;
    public float _baseMoveSpeed;

    // Rigidbody 컴포넌트 참조
    Rigidbody _rg;

    // 이동할 방향 벡터
    public Vector3 _moveVec;

    Vector3 _playerPos;              // 플레이어 위치
    public int _playerGold = 0;             // 플레이어 골드
    int _playerGem = 0;              // 플레이어 보석
    int _playerPassLevel = 1;        // 배틀 패스 레벨
    public int _playerSpeedLevel = 1;       // 이동 속도 레벨
    public int _playerHoldMaxLevel = 1;     // 드는 용량 레벨
    public int _playerMakeMoneyLevel = 1;   // 수익률 레벨

    [Header("플레이어의 고기")]
    [SerializeField] int _maxMeat;              //현재 들수 있는 고기 최대 수
    [SerializeField] int _baseMaxMeat;              //현재 들수 있는 고기 최대 수
    [SerializeField] int _currentMeat;          //현재 들고 있는 고기 수
    List<GameObject> _meatList = new List<GameObject>();    //생성된 고기 오브젝트들 담는 리스트

    [Header("플레이어의 뼈다귀")]
    [SerializeField] int _maxBone;              //현재 들수 있는 뼈 최대 수
    [SerializeField] int _currentBone;          //현재 들고 있는 뼈 수
    List<GameObject> _boneList = new List<GameObject>();    //생성된 뼈 오브젝트들 담는 리스트

    [Header("플레이어의 박스")]
    [SerializeField] int _maxBox;
    [SerializeField] int _currentBox;
    List<GameObject> _boxList = new List<GameObject>();    //생성된 뼈 오브젝트들 담는 리스트

    [SerializeField] GameObject board;
    [Header("고기 프리펩")]
    [SerializeField] GameObject _meatPrefab;

    [Header("뼈다귀 프리펩")]
    [SerializeField] GameObject _bonePrefab;

    [Header("박스 프리펩")]
    [SerializeField] GameObject _boxPrefab;

    [Header("오브젝트 풀링 연결")]
    [SerializeField] ObjectPooling _meatPool; // 고기를 관리하는 오브젝트 풀

    [Header("고기 쌓일 높이 간격")]
    [SerializeField] float _stackHeight = 0.11f;

    [Header("고기 배치하는 곳")]
    [SerializeField] Transform _meatSpawnLocation;
    Animator _animationController;

    [Header("박스 프리펩 높이 길이")]
    [SerializeField] float _boxStackHeight = 0.325f;  // 박스 쌓일 높이 간격
    #endregion

    #region 변수들 프로퍼티
    public Vector3 _MoveVec 
    {
        get => _moveVec;
        set => _moveVec = value;
    }

    public Vector3 _PlayerPos
    {
        get => _playerPos;
        set => _playerPos = value;
    }

    public int _Gold
    {
        get => _playerGold;
        set
        {
            _playerGold = Mathf.Max(0, value);
            OnGoldChanged?.Invoke(_playerGold); // 옵저버 패턴 이벤트 호출
        }
    }

    public int _Gem
    {
        get => _playerGem;
        set => _playerGem = Mathf.Max(0, value);
        //TODO: 추후 잼도 옵저버 패턴으로 이벤트 호출해야함
    }

    public int _PassLevel
    {
        get => _playerPassLevel;
        set => _playerPassLevel = Mathf.Max(1, value);
    }

    public int _SpeedLevel
    {
        get => _playerSpeedLevel;
        set => _playerSpeedLevel = Mathf.Max(1, value);
    }

    public int _HoldMaxLevel
    {
        get => _playerHoldMaxLevel;
        set => _playerHoldMaxLevel = Mathf.Max(1, value);
    }

    public int _MakeMoneyLevel
    {
        get => _playerMakeMoneyLevel;
        set => _playerMakeMoneyLevel = Mathf.Max(1, value);
    }

    public int _MaxMeat
    {
        get => _maxMeat;
        set => _maxMeat = value;
    }

    public int _CurrentMeat
    {
        get => _currentMeat;
        set
        {
            _currentMeat = Mathf.Clamp(0, value, _MaxMeat);
        }
    }
    public int _MaxBone
    {
        get => _maxBone;
        set => _maxBone = value;
    }

    public int _CurrentBone
    {
        get => _currentBone;
        set
        {
            _currentBone = Mathf.Clamp(0, value, _MaxBone);
        }
    }

    public int _MaxBox
    {
        get => _maxBox;
        set => _maxBox = value;
    }

    public int _CurrentBox
    {
        get => _currentBox;
        set
        {
            _currentBox = Mathf.Clamp(0, value, _MaxBox);
        }
    }
    #endregion

    #region Awake, FixedUpdata, LateUpdate
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _rg = GetComponent<Rigidbody>();
        _animationController = GetComponent<Animator>();
        _MaxMeat = 4;
        _MaxBone = 3;
        _MaxBox = 4;
        _baseMaxMeat = 4;
        _baseMoveSpeed = 5;
        _CurrentMeat = 0;
    }
    void FixedUpdate()
    {
        // 조이스틱 입력값 받아옴
        float x = _joy.Horizontal;
        float z = _joy.Vertical;
        // 이동 벡터 생성
        if (x == 0 && z == 0) return;
        _moveVec = new Vector3(x, 0, z) * _moveSpeed * Time.deltaTime;
        // 플레이어 이동
        _rg.MovePosition(_rg.position + _moveVec);
        // 회전 처리
        RotateToMoveDirection();
    }
    void LateUpdate()
    {
        //TODO : 애니메이션 추후 추가할 예정.
    }
    #endregion
    IEnumerator Start()
    {
        yield return null;  
        SetAbility();
    }

    #region 플레이어 움직임 회전 함수
    void RotateToMoveDirection()
    {
        if (_moveVec.sqrMagnitude < 0.0001f) return; // 방향 벡터가 거의 0이라면 회전하지 않음

        // 이동 방향으로 회전
        Quaternion _dirQuat = Quaternion.LookRotation(_moveVec);
        Quaternion _moveQuat = Quaternion.Slerp(_rg.rotation, _dirQuat, 0.3f);
        _rg.MoveRotation(_moveQuat);
    }
    #endregion

    #region 골드 관련 메서드
    /// <summary>
    /// 골드 증가
    /// </summary>
    /// <param name="gold">증가량</param>
    public int AddGold(int gold)
    {
        _Gold += gold;
        return 0;
    }

    /// <summary>
    /// 골드 감소
    /// </summary>
    /// <param name="gold">사용할 골드</param>
    /// <returns> 0 혹은 (변수 값 = 변수 값 - _gold) </returns>
    public int MinusGold(int gold)
    {
        if (_Gold >= gold)
        {
            _Gold -= gold;

        }
        else if(_Gold < gold)
        {
            _Gold = 0;
            gold -= _Gold;
            return gold;
        }
        return 0;
    }
    #endregion

    #region 고기 관련 메서드
    /// <summary>
    /// 고기 증가. 넘칠 경우, 넘치는 양을 반환
    /// </summary>
    public int AddMeat(int meat)
    {
        int spaceLeft = _MaxMeat - _currentMeat;
        int toAdd = Mathf.Min(spaceLeft, meat);

        _currentMeat += toAdd;

        UpdateMeatDisplay(_currentMeat);
        return meat - toAdd; // 넘친 양
    }

    /// <summary>
    /// 고기 감소
    /// </summary>
    public int MinusMeat(int amount)
    {
        int removed = Mathf.Min(_currentMeat, amount);
        _currentMeat -= removed;

        UpdateMeatDisplay(_currentMeat);
        return removed;
    }

    /// <summary>
    /// 고기 시각화 함수
    /// </summary>
    /// <param name="currentMeat"></param>
    public void UpdateMeatDisplay(int currentMeat)
    {
        // 1. 고기 개수가 부족하면 채워줌
        while (_meatList.Count < currentMeat)
        {
            GameObject meat = _meatPool.GetMeat(); // 오브젝트 풀에서 꺼냄
            meat.transform.SetParent(_meatSpawnLocation, false);
            // 생성 위치값, 회전값, 크기값
            meat.transform.localPosition = GetStackPosition(_meatList.Count);
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }

        // 2. 고기 개수가 초과되면 제거 (위에서부터 하나씩)
        while (_meatList.Count > currentMeat)
        {
            GameObject lastMeat = _meatList[_meatList.Count - 1];
            _meatList.RemoveAt(_meatList.Count - 1);
            _meatPool.ReturnToPool(lastMeat);
        }
    }
    #endregion

    #region 뼈 관련 함수들
    /// <summary>
    /// 뼈 더하는 함수
    /// </summary>
    /// <param name="trash"></param>
    /// <returns></returns>
    public int AddBone(int trash)
    {
        int spaceLeft = _maxBone - _currentMeat;
        int toAdd = Mathf.Min(spaceLeft, trash);
        _currentBone += toAdd;
        UpdateBoneDisplay(_currentBone);
        return trash - toAdd; // 넘친 양
    }

    /// <summary>
    /// 뼈 감소
    /// </summary>
    public int MinusBone(int amount)
    {
        int removed = Mathf.Min(_currentBone, amount);
        _currentBone -= removed;
        UpdateBoneDisplay(_currentBone);
        return removed;
    }
    public void UpdateBoneDisplay(int currentBone)
    {
        // 1. 뼈 개수가 부족하면 채워줌
        while (_boneList.Count < currentBone)
        {
            GameObject meat = _meatPool.GetBone(); // 오브젝트 풀에서 꺼냄
            meat.transform.SetParent(_meatSpawnLocation, false);
            // 생성 위치값, 회전값, 크기값
            meat.transform.localPosition = GetStackPosition(_boneList.Count);
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _bonePrefab.transform.localScale;

            _boneList.Add(meat);
        }

        // 2. 뼈 개수가 초과되면 제거 (위에서부터 하나씩)
        while (_boneList.Count > currentBone)
        {
            GameObject lastMeat = _boneList[_boneList.Count - 1];
            _boneList.RemoveAt(_boneList.Count - 1);
            _meatPool.ReturnToPool(lastMeat);
        }
    }
    #endregion

    #region 박스 관련 함수들
    /// <summary>
    /// 박스 추가. 넘칠 경우, 넘친 양 반환
    /// </summary>
    public int AddBox(int box)
    {
        int spaceLeft = _MaxBox - _currentBox;
        int toAdd = Mathf.Min(spaceLeft, box);
        _currentBox += toAdd;

        UpdateBoxDisplay(_currentBox);
        return box - toAdd; // 넘친 양
    }

    /// <summary>
    /// 박스 감소
    /// </summary>
    public int MinusBox(int amount)
    {
        int removed = Mathf.Min(_currentBox, amount);
        _currentBox -= removed;

        UpdateBoxDisplay(_currentBox);
        return removed;
    }

    /// <summary>
    /// 박스 시각화 함수
    /// </summary>
    public void UpdateBoxDisplay(int currentBox)
    {
        // 프리팹 null 체크
        if (_boxPrefab == null)
        {
            Debug.LogError("[PlayerController] _boxPrefab 프리팹이 할당되지 않았습니다!");
            return;
        }

        // 1. 박스 개수가 부족하면 채워줌
        while (_boxList.Count < currentBox)
        {
            GameObject box = _meatPool.GetBox(); // 오브젝트 풀에서 꺼냄 (박스용 메서드 필요)
            box.transform.SetParent(_meatSpawnLocation, false);
            box.transform.localPosition = GetStackPosition(_boxList.Count, true);
            box.transform.localRotation = Quaternion.identity;
            box.transform.localScale = _boxPrefab.transform.localScale;

            _boxList.Add(box);
        }

        // 2. 박스 개수가 초과되면 제거 (위에서부터 하나씩)
        while (_boxList.Count > currentBox)
        {
            GameObject lastBox = _boxList[_boxList.Count - 1];
            _boxList.RemoveAt(_boxList.Count - 1);
            _meatPool.ReturnToPool(lastBox); // 박스용 메서드 필요
        }
    }
    #endregion


    #region 움직일 때 애니메이션 부분
    Vector3 GetStackPosition(int index, bool isBox = false)
    {
        float height = isBox ? _boxStackHeight : _stackHeight;
        return new Vector3(0, index * height, 0);
    }

    public PlayerPickUpObject CheckPickUpObject()
    {
        if (_currentMeat > 0)
        {
            playerPickUpObject = PlayerPickUpObject.Meat;
        }
        else if (_currentBone > 0)
        {
            playerPickUpObject = PlayerPickUpObject.Bone;
        }
        else if(_currentBox > 0)
        {
            playerPickUpObject = PlayerPickUpObject.MeatBox;
        }
        else
        {
            playerPickUpObject = PlayerPickUpObject.None;
        }
        if (playerPickUpObject == PlayerPickUpObject.None)
        {
            _animationController.SetBool("IsCarry", false);
            board.SetActive(false);
        }
        else
        {
            _animationController.SetBool("IsCarry", true);
            board.SetActive(true);
        }
        return playerPickUpObject;
    }
    #endregion

    #region 조이스틱 이벤트 함수
    public void InvokeJoystickReleased()
    {
         _animationController.SetBool("IsWalk", false);
        OnJoystickReleased?.Invoke();
    }
    public void InvokeJoystickPerformed()
    {
        _animationController.SetBool("IsWalk", true);
        OnJoystickRelePerformed?.Invoke();
    }
    #endregion
    public void MaxAmountLevelUp()
    {
        _playerHoldMaxLevel += 1;
    }
    public void SpeedLevelUp()
    {
        _playerSpeedLevel += 1;
    }
    public void MakeMoneyLevelUp()
    {
        _playerMakeMoneyLevel += 1;
    }
    public void SetAbility()
    {
        _moveSpeed = _baseMoveSpeed + _playerSpeedLevel;
        _maxMeat = _baseMaxMeat + _playerHoldMaxLevel + 1;
    }
}
