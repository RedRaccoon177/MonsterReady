using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUpgradeUI : MonoBehaviour
{

    PlayerController _player;
    [SerializeField] Transform _speedImgParant;
    [SerializeField] Transform _amountImgParant;
    [SerializeField] Transform _makeMoneyImgParant;
    Image[] _speed;
    Image[] _amunt;
    Image[] _makeMoney;
    [SerializeField] Button _speedBtn;
    [SerializeField] Button _amountBtn;
    [SerializeField] Button _makeMoneyBtn;
    int _speedPrice;
    int _amountPrice;
    int _makeMoneyPrice;
    [SerializeField] TextMeshProUGUI _speedPriceTxt;
    [SerializeField] TextMeshProUGUI _amountPriceText;
    [SerializeField] TextMeshProUGUI _makeMoneyPriceText;


    private void Awake()
    {
        _player = PlayerController._instance;
        _speed = _speedImgParant.GetComponentsInChildren<Image>();
        _amunt = _amountImgParant.GetComponentsInChildren<Image>();
        _makeMoney = _makeMoneyImgParant.GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        _speedBtn.onClick.AddListener(() =>
        {
            _player.MinusGold(_speedPrice);
            _player.SpeedLevelUp();
            SetPlayerSpeed();
            DataManager._Instance.SavePlayerAllData();
            _player.SetAbility();
            SetUpgradePrive();
            CheckMoneyActiveBtn();
        });

        _amountBtn.onClick.AddListener(() =>
        {
            _player.MinusGold(_amountPrice);
            _player.MaxAmountLevelUp();
            SetPlayerAmount();
            DataManager._Instance.SavePlayerAllData();
            _player.SetAbility();
            SetUpgradePrive();
            CheckMoneyActiveBtn();
        });
        _makeMoneyBtn.onClick.AddListener(() =>
        {
            _player.MinusGold(_makeMoneyPrice);
            _player.MakeMoneyLevelUp();
            SetPlayerMakeMoney();
            DataManager._Instance.SavePlayerAllData();
            _player.SetAbility();
            SetUpgradePrive();
            CheckMoneyActiveBtn();
        });
    }

    private void OnEnable()
    {
        SetUpgradePrive();
        SetPlayerSpeed();
        SetPlayerMakeMoney();
        SetPlayerAmount();
        CheckMoneyActiveBtn();
    }
    void SetUpgradePrive()
    {
        _speedPrice = _player._SpeedLevel * 10;
        _amountPrice = _player._HoldMaxLevel * 10;
        _makeMoneyPrice = _player._MakeMoneyLevel * 10;
        _speedPriceTxt.text = _speedPrice.ToString();
        _amountPriceText.text= _amountPrice.ToString();
        _makeMoneyPriceText.text= _makeMoneyPrice.ToString();
    }
    void CheckMoneyActiveBtn()
    {
        // Speed 버튼
        if (_player._playerGold < _speedPrice || _player._SpeedLevel >= _speed.Length )
        {
            _speedBtn.interactable = false;
        }
        else
        {
            _speedBtn.interactable = true;
        }

        // Amount 버튼
        if (_player._playerGold < _amountPrice || _player._HoldMaxLevel >= _amunt.Length)
        {
            _amountBtn.interactable = false;
        }
        else
        {
            _amountBtn.interactable = true;
        }

        // MakeMoney 버튼
        if (_player._playerGold < _makeMoneyPrice || _player._MakeMoneyLevel >= _makeMoney.Length)
        {
            _makeMoneyBtn.interactable = false;
        }
        else
        {
            _makeMoneyBtn.interactable = true;
        }
    }

    void SetPlayerSpeed()
    {
        for (int i = 0; i < _player._SpeedLevel; i++)
        {
            _speed[i].color = Color.yellow;
        }
    }
    void SetPlayerMakeMoney()
    {

        for (int i = 0; i < _player._MakeMoneyLevel; i++)
        {
            _makeMoney[i].color = Color.yellow;
        }
    }
    void SetPlayerAmount()
    {
        for (int i = 0; i < _player._HoldMaxLevel; i++)
        {
            _amunt[i].color = Color.yellow;
        }
    }
}
