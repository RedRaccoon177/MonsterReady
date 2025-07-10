using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager _instance;
    [SerializeField] GameObject _npcBuyUi;
    [SerializeField] GameObject _playerUpgradeUi;
    [SerializeField] GameObject _counterUpgradeUi;
    [SerializeField] GameObject _grillUpgradeUi;
    [SerializeField] GameObject _tableUpgradeUi;
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
        OffNpcBuyUi();
    }

    public NpcBuyUi OnNpcBuyUi()
    {
        _npcBuyUi.SetActive(true);
        return _npcBuyUi.GetComponent<NpcBuyUi>();
    }
    public void OffNpcBuyUi()
    {
        _npcBuyUi.SetActive(false);
    }
    public void OnPlayerUpgradeUi()
    {
        _playerUpgradeUi.SetActive(true);
    }
    public void OffPlayerUpgradeUi()
    {
        _playerUpgradeUi.SetActive(false);
    }
    public void OnCounterUpgradeUi()
    {
        _counterUpgradeUi.SetActive(true);
    }
    public void OffCounterUpgradeUi()
    {
        _counterUpgradeUi.SetActive(false);
    }

    public void OnGrillUpgradeUi()
    {
        _grillUpgradeUi.SetActive(true);
    }
    public void OffGrillUpgradeUi()
    {
        _grillUpgradeUi.SetActive(false);
    }

    public void OnTableUpgradeUi()
    {
        _tableUpgradeUi.SetActive(true);
    }
    public void OffTableUpgradeUi()
    {
        _tableUpgradeUi.SetActive(false);
    }
}
