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
    GameObjectDataList gameData;
    GroundMoneyDataList groundMoneyDatas;

    string filePathPlayer; // �÷��̾� ������ ���� ���
    string filePathObject; // ȭ��,���̺�,ī���� ������ ���� ���
    string filePathGroundGold; // ���� ������ �ִ� �� ������ ���� ���

    private void Awake()
    {
        filePathPlayer = Path.Combine(Application.persistentDataPath, "playerData.json");
        filePathObject = Path.Combine(Application.persistentDataPath, "objectData.json");
        filePathGroundGold = Path.Combine(Application.persistentDataPath, "groundGold.json");
        Debug.Log("������ ���� ���!" + filePathPlayer);
        if (_Instance == null)
        {
            _Instance = this;
        }
    }
    private void Start()
    {
        // List<ObjectData>�� ������ �ִ� ����ȭ Ŭ����
        gameData = new GameObjectDataList();
        gameData.objectDatas = new List<ObjectData>();

        // List<int>�� ������ �ִ� ����ȭ Ŭ����
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
    /// ȭ��,ī����,���̺���� ������ �����ϴ� �Լ�
    /// ���ο� ������Ʈ ������ ����� ȣ���ؼ� ���� �������
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
        File.WriteAllText(filePathObject, playerJsonData); // ���Ͽ� ����
        Debug.Log("������ ���� �Ϸ�!" + playerJsonData);
    }

    /// <summary>
    /// json���� ������ ����Ʈ => ���ӸŴ����� �ִ� �迭�� �ҷ����� �Լ�
    /// ���� �����ϰ� �ε� �� ȣ���ؾ���
    /// </summary>
    public void LoadObjectData()
    {
        // ������ �����ϴ��� Ȯ��
        if (File.Exists(filePathObject))
        {
            string objectJsonData = File.ReadAllText(filePathObject); // ���Ͽ��� JSON �б�
            GameObjectDataList objectList = JsonUtility.FromJson<GameObjectDataList>(objectJsonData); // JSON�� ��ü�� ��ȯ

            // ���� �Ŵ����� �ִ� ��ȣ�ۿ� �迭���� ��ųʸ��� ����
            // ��� ����: Ű������ �����ϰ� ������Ʈ ��Ī��Ű�� ����
            Dictionary<string, InteractionObject> objectDictGM = new Dictionary<string, InteractionObject>();
            foreach (var obj in GameManager._instance._interactObjs)
            {
                objectDictGM.Add(obj.objectKeyName, obj);
            }

            
            for (int i=0; i< objectList.objectDatas.Count; i++)
            {
                // ��ųʸ� �ȿ� �ִ� Ű�� �� ���� ������ Ű�� ��ġ�ϴٸ�
                if (objectDictGM.TryGetValue(objectList.objectDatas[i].keyName, out var target))
                {
                    target.objectKeyName = objectList.objectDatas[i].keyName;
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


    /// <summary>
    /// �÷��̾��� ��� ������ json���� ����
    /// �÷��̾� ������ ��ȭ �� ȣ��
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
        File.WriteAllText(filePathPlayer, playerJsonData); // ���Ͽ� ����
        Debug.Log("������ ���� �Ϸ�!" + playerJsonData);
    }

    /// <summary>
    /// �÷��̾� ������ json -> PlayerController �ҷ�����
    /// �ε��� ȣ��!
    /// </summary>
    public void LoadPlayerAllData()
    {
        // ������ �����ϴ��� Ȯ��
        if (File.Exists(filePathPlayer))
        {
            string playerJsonData = File.ReadAllText(filePathPlayer); // ���Ͽ��� JSON �б�
            PlayerData player = JsonUtility.FromJson<PlayerData>(playerJsonData); // JSON�� ��ü�� ��ȯ
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
            Debug.LogWarning("����� ������ �����ϴ�!");
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
        File.WriteAllText(filePathGroundGold, groundGoldDataJson); // ���Ͽ� ����
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
