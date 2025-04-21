using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class DataManager : MonoBehaviour
{
    public static DataManager _Instance { get; private set; }
    [Header("플레이어 스크립트")] [SerializeField] PlayerController _playerController;
    PlayerData playerData;

    GameObjectDataList objectDataList;
    ActivatorList activatorList;
    GroundMoneyDataList groundMoneyDataList;

    string filePathPlayer; // 플레이어 데이터 저장 경로
    string filePathGroundGold; // 땅에 떨어져 있는 돈 데이터 저장 경로
    string filePathActivatorObject; // 

    string filePathTable;
    string filePathCounter;
    string filePathExpand;
    string filePathGrill;


    private void Awake()
    {
        #region 데이터 경로, 클래스 초기화
        filePathTable = Path.Combine(Application.persistentDataPath, "tableData.json");
        filePathCounter = Path.Combine(Application.persistentDataPath, "counterData.json");
        filePathGrill = Path.Combine(Application.persistentDataPath, "grillData.json");
        filePathExpand = Path.Combine(Application.persistentDataPath, "ecpandData.json");
        filePathGroundGold = Path.Combine(Application.persistentDataPath, "groundGold.json");
        filePathPlayer = Path.Combine(Application.persistentDataPath, "playerData.json");
        filePathActivatorObject = Path.Combine(Application.persistentDataPath, "unlockObject.json");
        Debug.Log(Application.persistentDataPath); // 경로 디버그 출력

        objectDataList = new GameObjectDataList();
        objectDataList.objectDatas = new List<ObjectData>();
        // 해금 오브젝트 정보 저장 주머니
        activatorList = new ActivatorList();
        activatorList.activators = new List<Activator>();
        // List<int>를 가지고 있는 직렬화 클래스
        groundMoneyDataList = new GroundMoneyDataList();
        groundMoneyDataList.groundMoneys = new List<GroundMoneyData>();
        if (_Instance == null)
        {
            _Instance = this;
        }
        #endregion
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveTableData(GameManager._instance._tables,ObjectType.Table);
            SaveTableData(GameManager._instance._counters,ObjectType.Counter);
            SaveTableData(GameManager._instance._tables,ObjectType.Grill);
            SavePlayerAllData();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadPlayerAllData();
            LoadActivatorData();
            LoadObjectData(ObjectType.Table);
            LoadObjectData(ObjectType.Counter);
            LoadObjectData(ObjectType.Grill);
        }
    }
    /// <summary>
    ///  BaseObject를 상속받은 오브젝트 배열을 받아 json에 저장 하는 함수 
    ///  - 오브젝트 활성화 될때 호출!
    /// </summary>
    /// <param name="baseObject"></param>
    /// <param name="type"></param>
    public void SaveTableData(BaseObject[] baseObject, ObjectType type)
    {
        objectDataList.objectDatas.Clear();
        for (int i = 0; i < baseObject.Length; i++)
        {
            ObjectData tableData = new ObjectData();
            tableData.key = baseObject[i]._keyName;
            tableData.isActive = baseObject[i]._isActive;
            if (type != ObjectType.Expand) // 확장에는 레벨이 없음
            {
                tableData.level = ((ILevelable)baseObject[i]).GetLevel();
            }
            objectDataList.objectDatas.Add(tableData);
        }
        string objectJsonData = JsonUtility.ToJson(objectDataList, true); // json으로 저장
        WriteAllText(type, objectJsonData);
    }

    /// <summary>
    /// 타입으로 json경로 받아오는 함수
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetJsonJsonRoute(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Table:
                return filePathTable;
            case ObjectType.Counter:

                return filePathCounter;
            case ObjectType.Grill:
                return filePathGrill;

            case ObjectType.Expand:
                return filePathExpand;

        }
        return null;
    }

    /// <summary> 
    /// 경로에 json파일 저장
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objectJsonData"></param>
    public void WriteAllText(ObjectType type, string objectJsonData)
    {
        string path = GetJsonJsonRoute(type);
        File.WriteAllText(path, objectJsonData);
    }

    /// <summary>
    /// 타입과 일치하는 배열을 게임 매니져에서 받아옴
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Dictionary<string, BaseObject> GetobjectDictGM(ObjectType type)
    {
        BaseObject[] tempArr = new BaseObject[0];
        switch (type)
        {
            case ObjectType.Table:
                tempArr = GameManager._instance._tables;
                break;

            case ObjectType.Counter:
                tempArr = GameManager._instance._counters;
                break;

            case ObjectType.Grill:
                tempArr = GameManager._instance._grills;
                break;

            case ObjectType.Expand:
                tempArr = GameManager._instance._expens;
                break;
        }

        Dictionary<string, BaseObject> tempDict = new Dictionary<string, BaseObject>();
        foreach (BaseObject obj in tempArr)
        {
            tempDict.Add(obj._keyName, obj);
        }
        return tempDict;
    }
    public void LoadObjectData(ObjectType type)
    {
        Dictionary<string, BaseObject> objectDictGM = GetobjectDictGM(type);
        var json = File.ReadAllText(GetJsonJsonRoute(type));
        GameObjectDataList objectList = JsonUtility.FromJson<GameObjectDataList>(json); // JSON을 객체로 변환
        for (int i = 0; i < objectList.objectDatas.Count; i++)
        {
            // 딕셔너리 안에 있는 키와 내 저장 데이터 키가 일치하다면
            if (objectDictGM.TryGetValue(objectList.objectDatas[i].key, out var target))
            {
                target._isActive = objectList.objectDatas[i].isActive;
                target.gameObject.SetActive(target._isActive);
                if (type != ObjectType.Expand)
                {
                    ((ILevelable)target).SetLevel(objectList.objectDatas[i].level);
                }
            }
            else
            {
                Debug.Log("일치하는 항목 없음");
            }
        }
    }
    public void SaveActivatorData(ObjectsActivator[] activator)
    {
        activatorList.activators.Clear();
        for (int i = 0; i < activator.Length; i++)
        {
            if (activator[i] != null)
            {
                Activator data = new Activator();
                data.step = activator[i]._step;
                data.isActive = activator[i]._isActive;
                activatorList.activators.Add(data);
            }
        }
        string activatorJsonData = JsonUtility.ToJson(activatorList, true);
        File.WriteAllText(filePathActivatorObject, activatorJsonData);
    }
    public void LoadActivatorData()
    {
        if (File.Exists(filePathActivatorObject))
        {
            string objectJsonData = File.ReadAllText(filePathActivatorObject); // 파일에서 JSON 읽기
            ActivatorList objectList = JsonUtility.FromJson<ActivatorList>(objectJsonData); // JSON을 객체로 변환

            Dictionary<int, ObjectsActivator> objectDictGM = new Dictionary<int, ObjectsActivator>();
            foreach (var obj in GameManager._instance._activator)
            {
                objectDictGM.Add(obj._step, obj);
            }
            for (int i = 0; i < objectList.activators.Count; i++)
            {
                // 딕셔너리 안에 있는 키와 내 저장 데이터 키가 일치하다면
                if (objectDictGM.TryGetValue(objectList.activators[i].step, out var target))
                {
                    target._isActive = objectList.activators[i].isActive;
                    target.gameObject.SetActive(target._isActive);
                }
                else
                {
                    Debug.Log("일치하는 항목 없음");
                }
            }
        }
    }

    /// <summary>
    /// 플레이어의 모든 데이터 json으로 저장
    /// 플레이어 데이터 변화 시 호출
    /// </summary>
    public void SavePlayerAllData()
    {
        playerData = new PlayerData(
            _playerController.gameObject.transform.position
            , _playerController._Gold
            , _playerController._Gem
            , _playerController._PassLevel
            , _playerController._SpeedLevel
            , _playerController._HoldMaxLevel
            , _playerController._MakeMoneyLevel
            );
        string playerJsonData = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(filePathPlayer, playerJsonData); // 파일에 저장
        Debug.Log("데이터 저장 완료!" + playerJsonData);
    }

    /// <summary>
    /// 플레이어 데이터 json -> PlayerController 불러오기
    /// 로딩때 호출!
    /// </summary>
    public void LoadPlayerAllData()
    {
        // 파일이 존재하는지 확인
        if (File.Exists(filePathPlayer))
        {
            string playerJsonData = File.ReadAllText(filePathPlayer); // 파일에서 JSON 읽기
            PlayerData player = JsonUtility.FromJson<PlayerData>(playerJsonData); // JSON을 객체로 변환
            _playerController.gameObject.transform.position = player.playerPos;
            _playerController._Gold = player.playerGold;
            _playerController._Gem = player.playergem;
            _playerController._PassLevel = player.playerPassLevel;
            _playerController._SpeedLevel = player.playerSpeedLevel;
            _playerController._HoldMaxLevel = player.playerHoldMaxLevel;
            _playerController._MakeMoneyLevel = player.playerMakeMoneyLevel;
        }
        else
        {
            Debug.LogWarning("저장된 파일이 없습니다!");
        }
    }
  }
