using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ObjectUpgradeUi : MonoBehaviour
{
    PlayerController _player;
    private ILevelable _levelable;
    private ObjectType _objectType;
    [SerializeField] Button _upgradeButton;
    [SerializeField] Button _closeButton;
    [SerializeField] Image[] _levelImgArr;
    [SerializeField] Transform levelImgParant;
    [SerializeField] TextMeshProUGUI _currentLevel;
    [SerializeField] TextMeshProUGUI _upgradePriceText;
    [SerializeField] int _upgradePrice;

    IEnumerator SetField()
    {
        _player = PlayerController._instance;
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        _upgradeButton.onClick.AddListener(() =>
        {
            _player.MinusGold(_upgradePrice);
            _levelable.LevelUp();
            SetObjectUpgradePrice();
            SetObjectLevel();
            PrintObjectUpgradePriceText();
            ActiveBtn();
            SaveLevelUpData(_objectType);
            DataManager._Instance.SaveObjectData(_objectType);
        });
        yield return null;
    }
    private void Awake()
    {
        _levelImgArr = levelImgParant.GetComponentsInChildren<Image>();
        StartCoroutine(SetField());
    }
    void SetObjectLevel()
    {
        for (int i=0; i< _levelImgArr.Length; i++)
        {
            if (i<_levelable.GetLevel())
            {
                _levelImgArr[i].color = Color.yellow;
            }
            else
            {
                _levelImgArr[i].color = Color.gray;
            }
        }
    }
    void SetObjectUpgradePrice()
    {
        _upgradePrice = _levelable.GetLevel() * 5;
    }
    void PrintObjectUpgradePriceText()
    {
        _currentLevel.text = _levelable.GetKey().ToString();
        _upgradePriceText.text = _upgradePrice.ToString();
    }
    public void SetTarget(ILevelable levelable, ObjectType objectType)
    {
        _levelable = levelable;
        _objectType = objectType;
        UpdateUI();
    }
    void UpdateUI()
    {
        ActiveBtn();
        SetObjectUpgradePrice();
        SetObjectLevel();
        PrintObjectUpgradePriceText();
    }
    void ActiveBtn()
    {
        // Speed ¹öÆ°
        if (_player._playerGold < _upgradePrice || _levelable.GetLevel()>= _levelImgArr.Length)
        {
            _upgradeButton.interactable = false;
        }
        else
        {
            _upgradeButton.interactable = true;
        }
    }
    void SaveLevelUpData(ObjectType objectType)
    {
        if (objectType == ObjectType.Table)
        {
            DataManager._Instance.SaveObjectData(ObjectType.Counter);
        }
        else if (objectType == ObjectType.Counter)
        {

            DataManager._Instance.SaveObjectData(ObjectType.Table);
        }
        else if (objectType == ObjectType.Grill)
        {
            DataManager._Instance.SaveObjectData(ObjectType.Grill);

        }
    }
}
