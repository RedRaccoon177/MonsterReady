using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class DataManager : MonoBehaviour
{

    //!!!!!
    public static DataManager _Instance { get; private set; }
    [Header("플레이어 스크립트")] [SerializeField] PlayerController _playerController;
    PlayerData playerData;
    GameObjectData gameData;

    string filePath;
    string filePathGrill;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        filePathGrill = Path.Combine(Application.persistentDataPath, "grillData.json");
        Debug.Log("데이터 저장 경로!" + filePath);
        if (_Instance == null)
        {
            _Instance = this;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SavePlayerAllData();
            SaveObjectData(GameManager._Instance._interactObjs);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadPlayerAllData();
            LoadObjectData();
        }
    }

    // 게임 매니져에서 모든 오브젝트를 담은 배열 받아서 JSON으로 저장
    public void SaveObjectData(InteractionObject[] interactObj)
    {
        gameData = new GameObjectData();
        gameData.objectDatas = new List<ObjectData>();
        for (int i=0; i< interactObj.Length; i++)
        {
            ObjectData data = new ObjectData();
            data.name = interactObj[i].objectName;
            data.key = interactObj[i].objectKey;
            data.isActive = interactObj[i].objectIsActive;
            data.level = interactObj[i].objectLevel;
            gameData.objectDatas.Add(data);
        }
        string playerJsonData = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(filePathGrill, playerJsonData); // 파일에 저장
        Debug.Log("데이터 저장 완료!" + playerJsonData);
    }

    // JSON 데이터를 읽어 오고 전달,
    // json으로 저장한 리스트 => 게임매니져에 있는 모든 오브젝트를 담은 배열
    public void LoadObjectData()
    {
        // 파일이 존재하는지 확인
        if (File.Exists(filePathGrill))
        {
            string objectJsonData = File.ReadAllText(filePathGrill); // 파일에서 JSON 읽기
            GameObjectData objectList = JsonUtility.FromJson<GameObjectData>(objectJsonData); // JSON을 객체로 변환
            for (int i=0; i< objectList.objectDatas.Count; i++)
            {
                if (
                    objectList.objectDatas[i].name == 
                    GameManager._Instance._interactObjs[i].objectName &&
                    objectList.objectDatas[i].key ==
                    GameManager._Instance._interactObjs[i].objectKey
                    )
                {
                    var tmep = GameManager._Instance._interactObjs[i];
                    GameManager._Instance._interactObjs[i].objectKey = objectList.objectDatas[i].key;
                    GameManager._Instance._interactObjs[i].objectName = objectList.objectDatas[i].name;
                    GameManager._Instance._interactObjs[i].objectLevel = objectList.objectDatas[i].level;
                    GameManager._Instance._interactObjs[i].objectIsActive = objectList.objectDatas[i].isActive;
                    Debug.Log("objectKey : " + tmep.objectKey);
                    Debug.Log("objectName : " + tmep.objectName);
                    Debug.Log("objectLevel : " + tmep.objectLevel); 
                }
            }
        }
        else
        {
            Debug.LogWarning("저장된 파일이 없습니다!");
        }

    }


    // 플레이어의 모든 데이터 json으로 저장
    public void SavePlayerAllData()
    {
        playerData = new PlayerData(
            _playerController.gameObject.transform.position
            , _playerController._playerGold
            , _playerController._playergem
            , _playerController._playerPassLevel
            , _playerController._playerSpeedLevel
            , _playerController._playerHoldMaxLevel
            , _playerController._playerMakeMoneyLevel
            );
        string playerJsonData = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(filePath, playerJsonData); // 파일에 저장
        Debug.Log("데이터 저장 완료!" + playerJsonData);
    }

    public void LoadPlayerAllData()
    {
        // 파일이 존재하는지 확인
        if (File.Exists(filePath))
        {
            string playerJsonData = File.ReadAllText(filePath); // 파일에서 JSON 읽기
            PlayerData player = JsonUtility.FromJson<PlayerData>(playerJsonData); // JSON을 객체로 변환
            _playerController.gameObject.transform.position = player.playerPos;
            _playerController._playerGold = player.playerGold;
            _playerController._playergem = player.playergem;
            _playerController._playerPassLevel = player.playerPassLevel;
            _playerController._playerSpeedLevel = player.playerSpeedLevel;
            _playerController._playerHoldMaxLevel = player.playerHoldMaxLevel;
            _playerController._playerMakeMoneyLevel = player.playerMakeMoneyLevel;
        }
        else
        {
            Debug.LogWarning("저장된 파일이 없습니다!");
        }
    }
}
