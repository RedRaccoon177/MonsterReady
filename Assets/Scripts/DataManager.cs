using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class DataManager : MonoBehaviour
{

    //!!!!!
    public static DataManager _Instance { get; private set; }
    [Header("�÷��̾� ��ũ��Ʈ")] [SerializeField] PlayerController _playerController;
    PlayerData playerData;
    GameObjectData gameData;

    string filePath;
    string filePathGrill;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        filePathGrill = Path.Combine(Application.persistentDataPath, "grillData.json");
        Debug.Log("������ ���� ���!" + filePath);
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

    // ���� �Ŵ������� ��� ������Ʈ�� ���� �迭 �޾Ƽ� JSON���� ����
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
        File.WriteAllText(filePathGrill, playerJsonData); // ���Ͽ� ����
        Debug.Log("������ ���� �Ϸ�!" + playerJsonData);
    }

    // JSON �����͸� �о� ���� ����,
    // json���� ������ ����Ʈ => ���ӸŴ����� �ִ� ��� ������Ʈ�� ���� �迭
    public void LoadObjectData()
    {
        // ������ �����ϴ��� Ȯ��
        if (File.Exists(filePathGrill))
        {
            string objectJsonData = File.ReadAllText(filePathGrill); // ���Ͽ��� JSON �б�
            GameObjectData objectList = JsonUtility.FromJson<GameObjectData>(objectJsonData); // JSON�� ��ü�� ��ȯ
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
            Debug.LogWarning("����� ������ �����ϴ�!");
        }

    }


    // �÷��̾��� ��� ������ json���� ����
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
        File.WriteAllText(filePath, playerJsonData); // ���Ͽ� ����
        Debug.Log("������ ���� �Ϸ�!" + playerJsonData);
    }

    public void LoadPlayerAllData()
    {
        // ������ �����ϴ��� Ȯ��
        if (File.Exists(filePath))
        {
            string playerJsonData = File.ReadAllText(filePath); // ���Ͽ��� JSON �б�
            PlayerData player = JsonUtility.FromJson<PlayerData>(playerJsonData); // JSON�� ��ü�� ��ȯ
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
            Debug.LogWarning("����� ������ �����ϴ�!");
        }
    }
}
