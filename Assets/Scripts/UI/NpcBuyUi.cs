using System.Collections.Generic;
using UnityEngine;

public class NpcBuyUi : MonoBehaviour
{
    [SerializeField] GameObject _contentPrefab;
    [SerializeField] Transform _contentParent;
    [SerializeField] List<NpcBuyUiContent> _contentList = new List<NpcBuyUiContent>();
    private NpcSpawner _npcSpawner;

    private void Awake()
    {
        Debug.Log("NpcBuyUi Awake ȣ��");
    }

    public void SetUi(NpcSpawner npcSpawner)
    {
        Debug.Log(00);
        _npcSpawner = npcSpawner;

        if (_contentList.Count == 0)
        {
            CreateContent(); // ó�� 1ȸ�� ����
        }

        UpdateContent(); // �Ź� �� �����ͷ� ����
    }

    private void CreateContent()
    {
        Debug.Log(11);
        for (int i = 0; i < _npcSpawner._npcdData.Length; i++)
        {
            var content = Instantiate(_contentPrefab, _contentParent).GetComponent<NpcBuyUiContent>();
            content.CreateSquare(_npcSpawner._npcdData[i]);
            _contentList.Add(content);
        }
    }

    private void UpdateContent()
    {
        Debug.Log(22);
        for (int i = 0; i < _contentList.Count; i++)
        {
            if (_contentList[i]._npcName == _npcSpawner._npcScriptList[i]._keyName)
            {
                _contentList[i].Setting(_npcSpawner._npcScriptList[i]);
            }
        }
    }

    private void OnEnable()
    {
        if (_npcSpawner != null)
        {
            UpdateContent();
        }
    }
}
