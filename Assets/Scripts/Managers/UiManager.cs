using System.Buffers.Text;
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
public abstract class UiBase:MonoBehaviour
{
    public virtual void Initialize() { }
    public abstract void Show<T>(T test);
    public virtual void Hide() 
    {
        gameObject.SetActive(false);
    } 
}

public class UiManager : MonoBehaviour
{
    private Dictionary<UiType, UiBase> _uiDict;
    public static UiManager _instance;
    [SerializeField] Camera _camera;
    [SerializeField] GameObject _npcBuyUi;
    [SerializeField] GameObject _playerUpgradeUi;
    [SerializeField] GameObject _objectUpgradeUi;
    [SerializeField] GameObject _upGradeNav;
    [SerializeField] GameObject _meatOrderUi;
    [SerializeField] TextMeshProUGUI _meatOrderTxt;
    [SerializeField] GameObject _meatBoxOrderUi;
    [SerializeField] TextMeshProUGUI _meatBoxOrderTxt;
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
        _uiDict = new Dictionary<UiType, UiBase>
        {
            { UiType.NpcBuy, _npcBuyUi.GetComponent<UiBase>() },
            { UiType.PlayerUpgrade, _playerUpgradeUi.GetComponent<UiBase>() },
            { UiType.ObjectUpgrade, _objectUpgradeUi.GetComponent<UiBase>() },
            { UiType.ObjectUpgrdeNav, _upGradeNav.GetComponent<UiBase>() },
            { UiType.MeatOrderUi, _meatOrderUi.GetComponent<UiBase>() },
            { UiType.MeatBoxOrderUi, _meatBoxOrderUi.GetComponent<UiBase>() },
        };
    }

    public void RegisterUI(UiType type, UiBase ui)
    {
        if (!_uiDict.ContainsKey(type))
            _uiDict[type] = ui;
    }
    public T GetUI<T>(UiType type) where T : UiBase
    {
        if (_uiDict.TryGetValue(type, out var ui))
            return ui as T;
        return null;
    }
    public void OnUi(UiType type)
    {
        if (_uiDict.TryGetValue(type, out var ui))
            ui.Show();
    }

    public void OffUi(UiType type)
    {
        if (_uiDict.TryGetValue(type, out var ui))
            ui.Hide();
    }

    //public void SetActive(UiType type, bool isActive)
    //{
    //    _uiDict[type].SetActive(isActive);
    //}
    //public T GetUiComponent<T>(UiType type) where T : Component
    //{
    //    return _uiDict[type].GetComponent<T>();
    //}

    //    public void OnUpgradeNavUi(Vector3 objectPos)
    //    {
    //        _upGradeNavScrit.targetPos = objectPos;
    //        _upGradeNav.SetActive(true);
    //    }
    //    public void OffUpgradeNavUi()
    //    {
    //        _upGradeNav.SetActive(false);
    //    }
    //    public void SetInteractionObjectKey(string key)
    //    {
    //        _interactionObjectKey = key;
    //    }
    //    void OnObjectActiveUi(BaseObject baseObject)
    //    {
    //        SetActive(UiType.ObjectUpgrade, true);
    //        var temp =GetUiComponent<ObjectUpgradeUi>(UiType.ObjectUpgrade);
    //        temp.SetTarget((ILevelable)baseObject,baseObject._objectType);
    //    }

    //    public void ActiveMeatOrdreUi(int orderMeatCount, bool active)
    //    {
    //        SetActive(UiType.MeatOrderUi, active);
    //        _meatOrderTxt.text = orderMeatCount.ToString();
    //    }

    //    public void ActiveMeatBoxOrdreUi(int orderMeatCount, bool active)
    //    {
    //        SetActive(UiType.MeatBoxOrderUi, active);
    //        _meatBoxOrderTxt.text = orderMeatCount.ToString();
    //    }
    //}
}
