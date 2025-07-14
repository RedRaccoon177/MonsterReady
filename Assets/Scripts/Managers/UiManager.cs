using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum UiType
{
    NpcBuy,
    PlayerUpgrade,
    ObjectUpgrade,
    ObjectUpgrdeNav,
    MeatOrderUi,
    MeatBoxOrderUi

}

public class UiManager : MonoBehaviour
{
    private Dictionary<UiType, GameObject> _uiDict;
    public static UiManager _instance;
    [SerializeField] Camera _camera;
    [SerializeField] GameObject _npcBuyUi;
    [SerializeField] GameObject _playerUpgradeUi;
    [SerializeField] GameObject _objectUpgradeUi;
    [SerializeField] GameObject _upGradeNav;
    [SerializeField] GameObject _meatOrderUi;
    [SerializeField] TextMeshProUGUI _meatOrderTxt;
    [SerializeField] GameObject _meatBoxOrderUi;
    [SerializeField] ObjectNavUi _upGradeNavScrit;
    [SerializeField] public string _interactionObjectKey { get; private set; }
    [SerializeField] Transform _canvas;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
    private void Start()
    {
        _upGradeNav.GetComponent<Button>().onClick.AddListener(() => 
        OnObjectActiveUi(GameManager._instance._baseObjectDict[_interactionObjectKey])
        );
        _uiDict = new Dictionary<UiType, GameObject>
        {
            { UiType.NpcBuy, _npcBuyUi },                                              
            { UiType.PlayerUpgrade, _playerUpgradeUi },
            { UiType.ObjectUpgrade, _objectUpgradeUi },
            { UiType.ObjectUpgrdeNav, _upGradeNav },
            { UiType.MeatOrderUi, _meatOrderUi },
            { UiType.MeatBoxOrderUi, _meatBoxOrderUi },
        };
    }
    public void SetActive(UiType type, bool isActive)
    {
        _uiDict[type].SetActive(isActive);
    }
    public T GetUiComponent<T>(UiType type) where T : Component
    {
        return _uiDict[type].GetComponent<T>();
    }
    public void OnUpgradeNavUi(Vector3 objectPos)
    {
        _upGradeNavScrit.targetPos = objectPos;
        _upGradeNav.SetActive(true);
    }
    public void OffUpgradeNavUi()
    {
        _upGradeNav.SetActive(false);
    }
    public void SetInteractionObjectKey(string key)
    {
        _interactionObjectKey = key;
    }
    void OnObjectActiveUi(BaseObject baseObject)
    {
        SetActive(UiType.ObjectUpgrade, true);
        var temp =GetUiComponent<ObjectUpgradeUi>(UiType.ObjectUpgrade);
        temp.SetTarget((ILevelable)baseObject,baseObject._objectType);
    }

    public void ActiveMeatOrdreUi(int orderMeatCount, bool active)
    {
        SetActive(UiType.MeatOrderUi, active);
        _meatOrderTxt.text = orderMeatCount.ToString();
    }
}
