using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class DataManager : MonoBehaviour
{

    public static DataManager _Instance { get; private set; }
    [Header("�÷��̾� ��ũ��Ʈ")] [SerializeField] PlayerController _playerController;
    PlayerData playerData;
    GameObjectData gameData;

    string filePath;
    string filePathGrill;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "playerData.json");
        filePathGrill = Path.Combine(Application.persistentDataPath, "objectData.json");
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
            data.keyName = interactObj[i].objectKeyName;
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
            Dictionary<string, InteractionObject> objectDict = new Dictionary<string, InteractionObject>();
            foreach (var obj in GameManager._Instance._interactObjs)
            {
                objectDict.Add(obj.objectKeyName, obj);
            }

            for (int i=0; i< objectList.objectDatas.Count; i++)
            {
                if (objectDict.TryGetValue(objectList.objectDatas[i].keyName, out var target))
                {
                    //target.objectKeyName = objectList.objectDatas[i].keyName;
                    target.objectLevel = objectList.objectDatas[i].level;
                    target.objectIsActive = objectList.objectDatas[i].isActive;
                }
                else
                {
                    Debug.Log("��ġ�� ������Ʈ ��ã��");
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
