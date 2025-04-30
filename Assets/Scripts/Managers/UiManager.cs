using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager _instance;
    [SerializeField] GameObject _npcBuyUi;
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
        _npcBuyUi = Instantiate(_npcBuyUi, _canvas);
        OffNpcBuyUi();
    }

    public NpcBuyUi OnNpcBuyUi()
    {
        
        _npcBuyUi.SetActive(true);
        Debug.Log(22);
        return _npcBuyUi.GetComponent<NpcBuyUi>();
    }
    public void OffNpcBuyUi()
    {
        _npcBuyUi.SetActive(false);
    }
}
