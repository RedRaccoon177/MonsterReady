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
    int _currentLevel;
    int _currentMaxLevel;
    float _moveSpeed;
    int _holdMaxAmount;
    bool _isUnlockNpc;
    public NpcPickUpObject _pickUpObject; // npc�� ��� �մ°�
    public INpcDestination _destination { get; set; } // npc ������
    public List<Node> _path{ get; set; } // npc ������������ ��

    public Coroutine _questCor { get; set; }
    // npc ����
    INpcState _currentState;
    public NpcIdle _npcIdle;
    public NpcAiMoveToPutDown _npcMoveToPutDown;
    public NpcMoveToPickUp _npcMoveToPickUp;

    [Header("npc�� ���")]
    [SerializeField] int _maxMeat;              //���� ��� �ִ� ��� �ִ� ��
    [SerializeField] int _currentMeat;          //���� ��� �ִ� ��� ��

    List<GameObject> _meatList = new List<GameObject>();
    [Header("��� ������")]
    [SerializeField] GameObject _meatPrefab;

    [Header("������Ʈ Ǯ�� ����")]
    [SerializeField] ObjectPooling _meatPool; // ��⸦ �����ϴ� ������Ʈ Ǯ

    [Header("��� ���� ���� ����")]
    [SerializeField] float _stackHeight = 0.11f;

    [Header("��� ��ġ�ϴ� ��")]
    [SerializeField] Transform _meatSpawnLocation;
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
    private void Start()
    {
        _MaxMeat = 4;
        _CurrentMeat = 0;
        _npcIdle = new NpcIdle();
        _npcMoveToPutDown = new NpcAiMoveToPutDown();
        _npcMoveToPickUp = new NpcMoveToPickUp();
        _path = new List<Node>();
        ChangeState(_npcIdle);
    }
    private void Update()
    {
        _currentState.Update(this);
    }
    public void currentPickUpType()
    {
        if (_CurrentMeat > 0)
        {
            _pickUpObject = NpcPickUpObject.Meat;
        }
        //else if (_CurrentMeat > 0)
        //{
        //    _pickUpObject = NpcPickUpObject.Meat;
        //}
        //else if(_CurrentMeat > 0)
        //{
        //    _pickUpObject = NpcPickUpObject.Meat;
        //}
        //else if(_CurrentMeat > 0)
        //{
        //    _pickUpObject = NpcPickUpObject.Meat;
        //}
    }

    /// <summary>
    /// ��� ����. ��ĥ ���, ��ġ�� ���� ��ȯ
    /// </summary>
    public int AddMeat(int meat)
    {
        int spaceLeft = _MaxMeat - _currentMeat;
        int toAdd = Mathf.Min(spaceLeft, meat);

        _currentMeat += toAdd;

        UpdateMeatDisplay(_currentMeat);
        return meat - toAdd; // ��ģ ��
    }

    /// <summary>
    /// ��� ����
    /// </summary>
    public int MinusMeat(int amount)
    {
        int removed = Mathf.Min(_currentMeat, amount);
        _currentMeat -= removed;

        UpdateMeatDisplay(_currentMeat);
        return removed;
    }

    /// <summary>
    /// ��� �ð�ȭ �Լ�
    /// </summary>
    /// <param name="currentMeat"></param>
    public void UpdateMeatDisplay(int currentMeat)
    {
        // 1. ��� ������ �����ϸ� ä����
        while (_meatList.Count < currentMeat)
        {
            GameObject meat = _meatPool.GetMeat(); // ������Ʈ Ǯ���� ����
            meat.transform.SetParent(_meatSpawnLocation, false);
            // ���� ��ġ��, ȸ����, ũ�Ⱚ
            meat.transform.localPosition = GetStackPosition(_meatList.Count);
            meat.transform.localRotation = Quaternion.identity;
            meat.transform.localScale = _meatPrefab.transform.localScale;

            _meatList.Add(meat);
        }

        // 2. ��� ������ �ʰ��Ǹ� ���� (���������� �ϳ���)
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

}
