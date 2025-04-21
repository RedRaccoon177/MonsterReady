using System;
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

    GameObjectDataList objectDataList;
    ActivatorList activatorList;
    GroundMoneyDataList groundMoneyDataList;

    string filePathPlayer; // �÷��̾� ������ ���� ���
    string filePathGroundGold; // ���� ������ �ִ� �� ������ ���� ���
    string filePathActivatorObject; // 

    string filePathTable;
    string filePathCounter;
    string filePathExpand;
    string filePathGrill;


    private void Awake()
    {
        #region ������ ���, Ŭ���� �ʱ�ȭ
        filePathTable = Path.Combine(Application.persistentDataPath, "tableData.json");
        filePathCounter = Path.Combine(Application.persistentDataPath, "counterData.json");
        filePathGrill = Path.Combine(Application.persistentDataPath, "grillData.json");
        filePathExpand = Path.Combine(Application.persistentDataPath, "ecpandData.json");
        filePathGroundGold = Path.Combine(Application.persistentDataPath, "groundGold.json");
        filePathPlayer = Path.Combine(Application.persistentDataPath, "playerData.json");
        filePathActivatorObject = Path.Combine(Application.persistentDataPath, "unlockObject.json");
        Debug.Log(Application.persistentDataPath); // ��� ����� ���

        objectDataList = new GameObjectDataList();
        objectDataList.objectDatas = new List<ObjectData>();
        // �ر� ������Ʈ ���� ���� �ָӴ�
        activatorList = new ActivatorList();
        activatorList.activators = new List<Activator>();
        // List<int>�� ������ �ִ� ����ȭ Ŭ����
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
    ///  BaseObject�� ��ӹ��� ������Ʈ �迭�� �޾� json�� ���� �ϴ� �Լ� 
    ///  - ������Ʈ Ȱ��ȭ �ɶ� ȣ��!
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
            if (type != ObjectType.Expand) // Ȯ�忡�� ������ ����
            {
                tableData.level = ((ILevelable)baseObject[i]).GetLevel();
            }
            objectDataList.objectDatas.Add(tableData);
        }
        string objectJsonData = JsonUtility.ToJson(objectDataList, true); // json���� ����
        WriteAllText(type, objectJsonData);
    }

    /// <summary>
    /// Ÿ������ json��� �޾ƿ��� �Լ�
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
    /// ��ο� json���� ����
    /// </summary>
    /// <param name="type"></param>
    /// <param name="objectJsonData"></param>
    public void WriteAllText(ObjectType type, string objectJsonData)
    {
        string path = GetJsonJsonRoute(type);
        File.WriteAllText(path, objectJsonData);
    }

    /// <summary>
    /// Ÿ�԰� ��ġ�ϴ� �迭�� ���� �Ŵ������� �޾ƿ�
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
        GameObjectDataList objectList = JsonUtility.FromJson<GameObjectDataList>(json); // JSON�� ��ü�� ��ȯ
        for (int i = 0; i < objectList.objectDatas.Count; i++)
        {
            // ��ųʸ� �ȿ� �ִ� Ű�� �� ���� ������ Ű�� ��ġ�ϴٸ�
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
                Debug.Log("��ġ�ϴ� �׸� ����");
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
            string objectJsonData = File.ReadAllText(filePathActivatorObject); // ���Ͽ��� JSON �б�
            ActivatorList objectList = JsonUtility.FromJson<ActivatorList>(objectJsonData); // JSON�� ��ü�� ��ȯ

            Dictionary<int, ObjectsActivator> objectDictGM = new Dictionary<int, ObjectsActivator>();
            foreach (var obj in GameManager._instance._activator)
            {
                objectDictGM.Add(obj._step, obj);
            }
            for (int i = 0; i < objectList.activators.Count; i++)
            {
                // ��ųʸ� �ȿ� �ִ� Ű�� �� ���� ������ Ű�� ��ġ�ϴٸ�
                if (objectDictGM.TryGetValue(objectList.activators[i].step, out var target))
                {
                    target._isActive = objectList.activators[i].isActive;
                    target.gameObject.SetActive(target._isActive);
                }
                else
                {
                    Debug.Log("��ġ�ϴ� �׸� ����");
                }
            }
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
  }
