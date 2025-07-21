using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : BaseObject, ILevelable, INpcDestination
{
    #region 키값 및 레벨
    [SerializeField] public int _level;
    [SerializeField] bool isDestination = false;
    [SerializeField] public Vector2 _nodeGridNum;
    [SerializeField] public Vector3 _objectPos;
    #endregion

    #region 변수들
    [Header("오브젝트 풀링 연결")]
    [SerializeField] ObjectPooling _meatPool; // 고기를 관리하는 오브젝트 풀

    [Header("고기 프리펩")]
    [SerializeField] GameObject _meatPrefab;

    [Header("카운터 npc")]
    [SerializeField] GameObject _npc;

    [Header("고기 배치하는 곳")]
    [SerializeField] Transform _meatSpawnLocation;

    [Header("현재 고기 갯수")]
    [SerializeField] public int _currentMeatCount = 0;

    [Header("고기 쌓일 높이 간격")]
    [SerializeField] float _stackHeight = 0.11f;
    [SerializeField] float _counterY = 1.65f;

    // 생성된 고기 오브젝트들을 담는 리스트
    List<GameObject> _meatList = new List<GameObject>();

    //플레이어 정보
    PlayerController _player;

    [Header("카운터 옆에 달린 현금")]
    [SerializeField] GoldObject _goldObject;

    [Header("카운터 상호작용 지역")]
    public ObjectInteration _objectInteration;
    #endregion

    #region 박스 관련 변수들
    [Header("박스 관련 변수")]
    [SerializeField] ObjectPooling _boxPool;          // 박스를 관리하는 오브젝트 풀
    [SerializeField] GameObject _boxPrefab;          // 박스 프리팹
    [SerializeField] Transform _boxSpawnLocation;    // 박스 쌓는 위치
    [SerializeField] float _boxStackHeight = 0.15f; // 박스 쌓는 간격
    [SerializeField] public int _currentBoxCount = 0; // 현재 박스 개수

    List<GameObject> _boxList = new List<GameObject>(); // 박스 오브젝트 리스트
    #endregion

    [Header("고기 -> 박스")]
    [SerializeField] MeatChangeToBoxCounter _meatTOBox;

    private void Awake()
    {
        _objectPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        _level = 1;
    }
    IEnumerator Start()
    {
        
        yield return null;
        _player = PlayerController._instance;
        SettingNode();
        SettingGMBaseDict();
        ActiveNpc();
        if (GetKeyName() == "counter1")
        {
            GameManager._instance._isCounterOneActive = true;
        }
        if (GetKeyName() == "counter3")
        {
            GameManager._instance._isCounterSecondActive = true;
            StartCoroutine(ChangeMeatToBox());

        }
    }

    #region 고기 증가 및 감소
    /// <summary>
    /// 카운터 고기 증가 함수
    /// </summary>
    /// <param name="_meatCount"></param>
    void AddMeat(int _meatCount)
    {
        _currentMeatCount = Mathf.Max(0, _currentMeatCount + _meatCount);
        UpdateMeatDisplay(_currentMeatCount);
    }

    /// <summary>
    /// 카운터 고기 감소
    /// </summary>
    /// <param name="_minusMeat"></param>
    public int MinusMeat(int _minusMeat)
    {
        int _someoneGetMeat;

        if (_currentMeatCount < _minusMeat)
        {
            _someoneGetMeat = _currentMeatCount;
            _currentMeatCount = 0;
        }
        else
        {
            _someoneGetMeat = _minusMeat;
            _currentMeatCount -= _minusMeat;
        }

        if (_goldObject != null)
        {
            _goldObject.AddGold(_someoneGetMeat);
        }

        UpdateMeatDisplay(_currentMeatCount);
        return _someoneGetMeat;
    }
    #endregion

    #region 고기 시각화 하는 코드 모음
    //고기 시각화 하는 코드
    public void UpdateMeatDisplay(int currentMeat)
    {
        // 1. 고기 개수가 부족하면 채워줌
        while (_meatList.Count < currentMeat)
        {
            GameObject meat = _meatPool.GetMeat(); // 오브젝트 풀에서 꺼냄

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
        return new Vector3
        (
            _meatSpawnLocation.position.x,
            _meatSpawnLocation.position.y + index * _stackHeight + _counterY,
            _meatSpawnLocation.position.z
        );
    }
    #endregion

    #region 박스 증가 및 감소
    /// <summary>
    /// 카운터 박스 증가 함수
    /// </summary>
    /// <param name="_boxCount"></param>
    void AddBox(int _boxCount)
    {
        _currentBoxCount = Mathf.Max(0, _currentBoxCount + _boxCount);
        UpdateBoxDisplay(_currentBoxCount);
    }

    /// <summary>
    /// 카운터 박스 감소 함수
    /// </summary>
    /// <param name="_minusBox"></param>
    public int MinusBox(int _minusBox)
    {
        int _someoneGetBox;

        if (_currentBoxCount < _minusBox)
        {
            _someoneGetBox = _currentBoxCount;
            _currentBoxCount = 0;
        }
        else
        {
            _someoneGetBox = _minusBox;
            _currentBoxCount -= _minusBox;
        }

        if (_goldObject != null)
        {
            _goldObject.AddGold(_someoneGetBox);
        }

        UpdateBoxDisplay(_currentBoxCount);
        return _someoneGetBox;
    }
    #endregion

    #region 박스 시각화 하는 코드 모음
    // 박스 시각화 코드
    public void UpdateBoxDisplay(int currentBox)
    {
        // 1. 박스 개수가 부족하면 채워줌
        while (_boxList.Count < currentBox)
        {
            GameObject box = _boxPool.GetBox(); // 오브젝트 풀에서 꺼냄

            // 생성 위치값, 회전값, 크기값
            box.transform.localPosition = GetBoxStackPosition(_boxList.Count);
            box.transform.localRotation = Quaternion.identity;
            box.transform.localScale = _boxPrefab.transform.localScale;

            _boxList.Add(box);
        }

        // 2. 박스 개수가 초과되면 제거 (위에서부터 하나씩)
        while (_boxList.Count > currentBox)
        {
            GameObject lastBox = _boxList[_boxList.Count - 1];
            _boxList.RemoveAt(_boxList.Count - 1);
            _boxPool.ReturnToPool(lastBox);
        }
    }

    Vector3 GetBoxStackPosition(int index)
    {
        return new Vector3
        (
            _boxSpawnLocation.position.x,
            _boxSpawnLocation.position.y + index * _boxStackHeight + _counterY,
            _boxSpawnLocation.position.z
        );
    }
    #endregion

    #region Trigger 부분
    // 플레이어가 범위에 들어왔을 때 고기 자동 제공
    private void OnTriggerEnter(Collider other)
    {
        // 태그가 Player가 아닐 경우 무시
        if (!other.CompareTag("Player") && !other.CompareTag("Npc")) return;
        if (_player == null)
        {
            return;
        }

        if (this.GetKeyName() == "counter1")
        {
            //플레이어의 정보를 바탕으로 더해야 할 무언가
            if (other.CompareTag("Player"))
            {
                UiManager._instance.OnUi(UiType.ObjectUpgrdeNav);
                if (0 != _player._CurrentMeat)
                {
                    AddMeat(_player._CurrentMeat);
                    _player.MinusMeat(_currentMeatCount);
                    _player.CheckPickUpObject();
                }
            }
            else if (other.CompareTag("Npc"))
            {
                var npc = other.gameObject.GetComponent<NpcAi>();
                if (0 != npc._CurrentMeat)
                {
                    AddMeat(npc._CurrentMeat);
                    npc.MinusMeat(_currentMeatCount);
                    npc.CurrentPickUpType();
                }
            }
        }
        else if (this.GetKeyName() == "counter2")
        {
            // 박스를 가져오면 고기처럼 박스가 쌓이고 손님이 오면 가져가기
            if (other.CompareTag("Player"))
            {
                UiManager._instance.OnUi(UiType.ObjectUpgrdeNav);
                if (0 != _player._CurrentBox)
                {
                    AddBox(_player._CurrentBox);
                    _player.MinusBox(_currentBoxCount);
                    _player.CheckPickUpObject();
                }
            }
            else if (other.CompareTag("Npc"))
            {
                var npc = other.gameObject.GetComponent<NpcAi>();
                if (0 != npc._CurrentBox)
                {
                    AddBox(npc._CurrentBox);
                    npc.MinusBox(_currentBoxCount);
                    npc.CurrentPickUpType();
                }
            }
        }
        else if (this.GetKeyName() == "counter3")
        {
            if (_level >= 2)
            {
                return;
            }
            // 고기를 가져오면 박스로 전환되게 하기
            if (other.CompareTag("Player"))
            {
                UiManager._instance.OnUi(UiType.ObjectUpgrdeNav);

                if (0 != _meatTOBox._currentMeatCount)
                {
                    AddBox(_meatTOBox._currentMeatCount);
                    _meatTOBox.MinusMeat(_meatTOBox._currentMeatCount);
                }
            }
            //else if (other.CompareTag("Npc"))
            //{
            //    var npc = other.gameObject.GetComponent<NpcAi>();
            //    if (0 != npc._CurrentMeat)
            //    {
            //        AddBox(npc._CurrentMeat);
            //        npc.MinusBox(_currentMeatCount);
            //        npc.CurrentPickUpType();
            //    }
            //}
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UiManager._instance.OffUi(UiType.ObjectUpgrdeNav);
        }
    }
    #endregion

    public bool HasStack()
    {
        if (GetKeyName() == "counter1")
        {
            return _currentMeatCount >0;
        }
        else if (GetKeyName() == "counter3")
        {
            return _currentBoxCount > 0;
        }
        return false;
    }

    public int GetStackCount()
    {
        if (GetKeyName() == "counter1")
        {
            return _currentMeatCount;
        }
        else if (GetKeyName() == "counter3")
        {
            return _currentBoxCount;
        }
        return 0;
    }
    public void SettingNode()
    {
        Node _tempNode = NodeManager._instance._nodeList[(int)_nodeGridNum.x, (int)_nodeGridNum.y];
        GameManager._instance._npcObjectNodeDict.TryAdd(GetKeyName(), _tempNode);
    }
    public void SettingGMBaseDict()
    {
        GameManager._instance._baseObjectDict.TryAdd(GetKeyName(), this);
    }

    public void OnDestination()
    {
        isDestination = true;
    }

    public void OffDestination()
    {
        isDestination = false;  
    }

    public bool IsDestination()
    {
        return isDestination;
    }
    public string GetKey()
    {
        return GetKeyName();
    }

    public int SetLevel(int level)
    {
        _level = level;
        return level;
    }

    public void LevelUp()
    {
        _level++;
        ActiveNpc();
    }
    void ActiveNpc()
    {
        if (_level == 2)
        {
            _npc.SetActive(true);
            _objectInteration.OnNpc();
        }
    }
    IEnumerator ChangeMeatToBox()
    {
        yield return new WaitUntil(() => _level >= 2);
        while (true)
        {
            if (_meatTOBox._currentMeatCount > 0)
            {
                _meatTOBox.MinusMeat(1);
                AddBox(1);
            }
            yield return new WaitForSeconds(2);
        }
    }
    public int GetLevel()
    {
        return _level;
    }
}
