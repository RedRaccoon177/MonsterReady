using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectUpgradeUi : MonoBehaviour
{
    [SerializeField] BaseObject _baseObject;
    [SerializeField] ObjectType _objectType;
    [SerializeField] ILevelable levelable;
    [SerializeField] Button _upgradeButton;
    [SerializeField] Image[] levelImgArr;
    [SerializeField] Transform levelImgParant;
    [SerializeField] TextMeshProUGUI _currentLevel;
    [SerializeField] TextMeshProUGUI _upgradePrice;
    [SerializeField] float upgradePrice;

    private void Awake()
    {
        levelable =_baseObject.GetComponent<ILevelable>();
        levelImgArr = levelImgParant.GetComponentsInChildren<Image>();
        _upgradeButton.onClick.AddListener(() => 
        { 
            SetObjectUpgradePrice();
            SetObjectLevel();
            PrintObjectUpgradePriceText();
            //DataManager._Instance.SaveObjectData(_objectType,);
        });
    }
    private void OnEnable()
    {
        SetObjectUpgradePrice();
        SetObjectLevel();
        PrintObjectUpgradePriceText();
    }
    void SetObjectLevel()
    {
        for (int i=0; i< levelable.GetLevel(); i++)
        {
            levelImgArr[i].color = Color.yellow;
        }
    }
    void SetObjectUpgradePrice()
    {
        upgradePrice = levelable.GetLevel() * 5;
    }
    void PrintObjectUpgradePriceText()
    {
        _currentLevel.text = levelable.GetLevel().ToString();
        _upgradePrice.text = upgradePrice.ToString();
    }
}
