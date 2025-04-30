using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum NpcPickUpObject
{
    Meat,          
    Trash,
    MeatSat,
    None
}

public interface INpcState
{
    public void Enter(NpcAi npcAi);
    public void Exiter(NpcAi npcAi);
    public void Update(NpcAi npcAi);
}
public class NpcAi : MonoBehaviour
{
    public string _keyName;
    public int _currentLevel { get; set; } // 현재 레벨
    public int _maxLevel { get; set; }  // 최대 레벨
    public int _amountLevel { get; set; }  // 최대 레벨
    public int _speedLevel { get; set; }  // 최대 레벨
    public int _startLevel { get; set; }  //시작 레벨
    public int _price { get; set; }  //가격
    float _moveSpeed; // 스피드
    public Sprite _npcIcon; // 스피드
    int _holdMaxAmount;
    public bool _isUnlockNpc; // 보유중인지
    public NpcPickUpObject _pickUpObject; // npc가 들고 잇는거
    public INpcDestination _destination { get; set; } // npc 목적지 오브젝트
    public Node _targetNode{ get; set; } // npc 목적지 노드
    public List<Node> _path{ get; set; } // npc 목적지까지의 길

    public Coroutine _questCor { get; set; }
    // npc 상태
    public INpcState _currentState;
    public NpcIdle _npcIdle;
    public NpcMove _npcMove;

    [Header("npc의 고기")]
    [SerializeField] int _maxMeat;              //현재 들수 있는 고기 최대 수
    [SerializeField] int _currentMeat;          //현재 들고 있는 고기 수

    List<GameObject> _meatList = new List<GameObject>();
    [Header("고기 프리펩")]
    [SerializeField] public GameObject _meatPrefab;

    [Header("오브젝트 풀링 연결")]
    [SerializeField] public ObjectPooling _meatPool; // 고기를 관리하는 오브젝트 풀

    [Header("고기 쌓일 높이 간격")]
    [SerializeField] float _stackHeight = 0.11f;

    [Header("고기 배치하는 곳")]
    [SerializeField] public Transform _meatSpawnLocation;
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

    public void SatAbility(int currentLevel)
    {
    }
    public void ChangeState(INpcState nextState)
    {
        _currentState?.Exiter(this);
        _currentState = nextState;
        _currentState?.Enter(this);
    }
    private void Awake()
    {
        Debug.Log(1);
        _isUnlockNpc = false;
        SettingActive(_isUnlockNpc);
        _MaxMeat = 4;
        _CurrentMeat = 0;
        _npcIdle = new NpcIdle();
        _npcMove = new NpcMove();
        _path = new List<Node>();
    }
    private void OnEnable()
    {
        ChangeState(_npcIdle);
    }
    private void Update()
    {
        _currentState.Update(this);
    }
    private void OnDisable()
    {
        if (_questCor != null)
        {
            StopCoroutine(_questCor);
        }
    }
    public void currentPickUpType()
    {
        if (_CurrentMeat > 0)
        {
            _pickUpObject = NpcPickUpObject.Meat;
        }
        else
        {
            _pickUpObject = NpcPickUpObject.None;

        }
    }

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
    Vector3 GetStackPosition(int index)
    {
        return new Vector3(0, index * _stackHeight, 0);
    }
    public void SettingActive(bool isActive)
    {
        gameObject.SetActive(isActive);
        _isUnlockNpc = isActive;
    }

    public void test()
    {

    }
    public void SettingAbility(int speedLevel, int amountLevel)
    {
        _moveSpeed = 1 + 0.5f * (speedLevel - 1);
        _maxMeat = 1 + 1 * (amountLevel - 1);
    }
    public void LevelUp()
    {
        _currentLevel += 1;
        DevideLevel();
        SettingAbility(_speedLevel, _amountLevel);
    }
    public void DevideLevel()
    {
        var temp1 = _currentLevel / 2;
        var temp2 = _currentLevel % 2;
        if (temp2 == 0)
        {
            _speedLevel = temp1;
            _amountLevel = temp1;
        }
        else
        {
            _speedLevel = temp1 + 1;
            _amountLevel = temp1;
        }

    }
}
