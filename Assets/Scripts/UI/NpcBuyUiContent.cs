using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class NpcBuyUiContent : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _level;        
    [SerializeField] Button buyButton;        
    [SerializeField] Button upgradeButton;        
    [SerializeField] GameObject deActiveUi;
    [SerializeField] GameObject[] _speedAbilityArray;
    [SerializeField] GameObject[] _amountAbilityArray;
    public string _npcName { get;private set; }
    int _speedLevel;
    int _amountLevel;

    private void Awake()
    {
        Debug.Log("content생성");
        _icon = _icon.GetComponent<Image>();
        buyButton = buyButton.GetComponent<Button>();
        buyButton.onClick.AddListener(OnBuyButtonClicked);
        upgradeButton.onClick.AddListener(UpgradeButtonClicked);
    }

    // 받은 능력치 배열 만큼 회색 네모칸 생성
    public void CreateMaxSquare(GameObject[] array , int max)
    {
        int count = 0;
        while (count < max)
        {
            array[count].SetActive(true);
            count++;
        }
    }

    // 모든 능력치 회색 네모칸 생성
    public void CreateSquare(NpcScriptableObject npcAi)
    {
        DivideLevel(npcAi.maxLevel);
        CreateMaxSquare(_speedAbilityArray,_speedLevel);
        CreateMaxSquare(_amountAbilityArray,_amountLevel);
        _npcName = npcAi.keyName;
    }
    // 정수를 받아서 홀수,짝수를 판별 후 , 스피드와 용량 레벨 세팅
    public void DivideLevel(int totalLevel)
    {
        var temp1 = totalLevel / 2;
        var temp2= totalLevel % 2;
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
    public void Setting(NpcAi npcAi)
    {
        _icon.sprite = npcAi._npcIcon;
        _npcName = npcAi._keyName;
        _name.text = npcAi._keyName;
        _level.text = npcAi._currentLevel.ToString();
        if (npcAi._isUnlockNpc == false)
        {
            deActiveUi.SetActive(true);
        }
        else
        {
            deActiveUi.SetActive(false);
        }
        CheckUpgradePossavble(npcAi);
        OnActiveAllGreenSqaure(npcAi);
    }
    private void OnBuyButtonClicked()
    {
        // 돈이 된다면?
        deActiveUi.SetActive(false);
        NpcSpawner._npcAiDic[_npcName].SettingActive(true);
        DataManager._Instance.SaveNpcData();
    }
    public void UpgradeButtonClicked()
    {
        NpcAi npcAi = NpcSpawner._npcAiDic[_npcName];
        npcAi.LevelUp();
        _level.text = npcAi._currentLevel.ToString();
        OnActiveAllGreenSqaure(npcAi);
        CheckUpgradePossavble(npcAi);
        DataManager._Instance.SaveNpcData();
    }
    public void CheckUpgradePossavble(NpcAi npcAi)
    {
        Debug.Log("npcAi._currentLevel" + npcAi._currentLevel);
        Debug.Log("npcAi._maxLevel" + npcAi._maxLevel);
        if (npcAi._currentLevel == npcAi._maxLevel)
        {
            upgradeButton.gameObject.SetActive(false);
        }
    }
    public void OnActiveAllGreenSqaure(NpcAi npcAi)
    {
        npcAi.DevideLevel();
        OnActiveGreenSqaure(_speedAbilityArray,npcAi._speedLevel);
        OnActiveGreenSqaure(_amountAbilityArray, npcAi._amountLevel);

    }

    // 배열과 최대치를 받아서 초록 네모칸(현재 능력치) 활성화
    public void OnActiveGreenSqaure(GameObject[] array, int maxLevel)
    {
        int count = 0;
        while (count < maxLevel)
        {
            array[count].transform.GetChild(0).gameObject.SetActive(true);
            count++;
        }
    }
}
