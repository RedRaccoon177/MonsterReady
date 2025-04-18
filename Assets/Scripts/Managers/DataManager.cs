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
    GameObjectDataList gameData;
    GroundMoneyDataList groundMoneyDatas;

    string filePathPlayer; // 플레이어 데이터 저장 경로
    string filePathObject; // 화로,테이블,카운터 데이터 저장 경로
    string filePathGroundGold; // 땅에 떨어져 있는 돈 데이터 저장 경로

    private void Awake()
    {
        filePathPlayer = Path.Combine(Application.persistentDataPath, "playerData.json");
        filePathObject = Path.Combine(Application.persistentDataPath, "objectData.json");
        filePathGroundGold = Path.Combine(Application.persistentDataPath, "groundGold.json");
        Debug.Log("데이터 저장 경로!" + filePathPlayer);
        if (_Instance == null)
        {
            _Instance = this;
        }
    }
    private void Start()
    {
        // List<ObjectData>를 가지고 있는 직렬화 클래스
        gameData = new GameObjectDataList();
        gameData.objectDatas = new List<ObjectData>();

        // List<int>를 가지고 있는 직렬화 클래스
        groundMoneyDatas = new GroundMoneyDataList();
        groundMoneyDatas.groundMoneyList = new List<int>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SavePlayerAllData();
            SaveObjectData(GameManager._instance._interactObjs);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadPlayerAllData();
            LoadObjectData();
            GameManager._instance.CreateInteractionObject();
        }
    }

    /// <summary>
    /// 화로,카운터,테이블들의 정보들 저장하는 함수
    /// 새로운 오브젝트 데이터 변경시 호출해서 저장 해줘야함
    /// </summary>
    /// <param name="interactObj"></param>
    public void SaveObjectData(InteractionObject[] interactObj)
    {
        for (int i=0; i< interactObj.Length; i++)
        {
            ObjectData data = new ObjectData();
            data.keyName = interactObj[i].objectKeyName;
            data.isActive = interactObj[i].objectIsActive;
            data.level = interactObj[i].objectLevel;
            gameData.objectDatas.Add(data);
        }
        string playerJsonData = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(filePathObject, playerJsonData); // 파일에 저장
        Debug.Log("데이터 저장 완료!" + playerJsonData);
    }

    /// <summary>
    /// json으로 저장한 리스트 => 게임매니져에 있는 배열에 불러오는 함수
    /// 게임 시작하고 로딩 때 호출해야함
    /// </summary>
    public void LoadObjectData()
    {
        // 파일이 존재하는지 확인
        if (File.Exists(filePathObject))
        {
            string objectJsonData = File.ReadAllText(filePathObject); // 파일에서 JSON 읽기
            GameObjectDataList objectList = JsonUtility.FromJson<GameObjectDataList>(objectJsonData); // JSON을 객체로 변환

            // 게임 매니져에 있는 상호작용 배열들을 딕셔너리에 복사
            // 사용 이유: 키값으로 안전하게 오브젝트 매칭시키기 위해
            Dictionary<string, InteractionObject> objectDictGM = new Dictionary<string, InteractionObject>();
            foreach (var obj in GameManager._instance._interactObjs)
            {
                objectDictGM.Add(obj.objectKeyName, obj);
            }

            
            for (int i=0; i< objectList.objectDatas.Count; i++)
            {
                // 딕셔너리 안에 있는 키와 내 저장 데이터 키가 일치하다면
                if (objectDictGM.TryGetValue(objectList.objectDatas[i].keyName, out var target))
                {
                    target.objectKeyName = objectList.objectDatas[i].keyName;
                    target.objectLevel = objectList.objectDatas[i].level;
                    target.objectIsActive = objectList.objectDatas[i].isActive;
                }
                else
                {
                    Debug.Log("일치한 오브젝트 못찾음");
                }
            } 
        }
        else
        {
            Debug.LogWarning("저장된 파일이 없습니다!");
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

    public void SaveGroundMoney()
    {
        var GameManagerArr = GameManager._instance._GroundMoney;
        for (int i=0; i< GameManagerArr.Length; i++)
        {
            groundMoneyDatas.groundMoneyList.Add(GameManagerArr[i]);
        }
        string groundGoldDataJson = JsonUtility.ToJson(groundMoneyDatas, true);
        File.WriteAllText(filePathGroundGold, groundGoldDataJson); // 파일에 저장
    }

    public void LoadGroundMoney()
    {
        if (File.Exists(filePathGroundGold))
        {
            string groundGoldDataJson = File.ReadAllText(filePathGroundGold);
            GroundMoneyDataList aa = JsonUtility.FromJson<GroundMoneyDataList>(groundGoldDataJson);
        }
        else
        {

        }
    }
}
