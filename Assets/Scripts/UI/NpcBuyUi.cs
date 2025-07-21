using System.Collections.Generic;
using UnityEngine;

public class NpcBuyUi : UiBase
{
    [SerializeField] GameObject _contentPrefab;
    [SerializeField] Transform _contentParent;
    [SerializeField] List<NpcBuyUiContent> _contentList = new List<NpcBuyUiContent>();
    public NpcSpawner _npcSpawner;

    private void Awake()
    {
        Debug.Log("NpcBuyUi Awake 호출");
    }

    private void CreateContent()
    {
        for (int i = 0; i < _npcSpawner._npcdData.Length; i++)
        {
            var content = Instantiate(_contentPrefab, _contentParent).GetComponent<NpcBuyUiContent>();
            content.CreateSquare(_npcSpawner._npcdData[i]);
            _contentList.Add(content);
        }
    }

    private void UpdateContent()
    {
        for (int i = 0; i < _contentList.Count; i++)
        {
            if (_contentList[i]._npcName == _npcSpawner._npcScriptList[i]._keyName)
            {
                _contentList[i].Setting(_npcSpawner._npcScriptList[i]);
            }
        }
    }

    public override void Show()
    {
        if (_npcSpawner != null)
        {
            if (_contentList.Count == 0)
            {
                CreateContent(); // 처음 1회만 생성
            }

            UpdateContent();
        }
    }
}
