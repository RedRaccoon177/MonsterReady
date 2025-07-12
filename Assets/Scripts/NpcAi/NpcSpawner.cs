using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    [Header("npcµ•¿Ã≈Õ")] [SerializeField] public NpcScriptableObject[] _npcdData;
    [SerializeField] GameObject _meatPrefab;
    [SerializeField] GameObject _bonePrefab;
    [SerializeField] ObjectPooling _meatPool;
    Transform _meatSpawnLocation;
    public List<NpcAi> _npcScriptList;
    static public Dictionary<string, NpcAi> _npcAiDic;


    private void Awake()
    {
        _npcAiDic = new Dictionary<string, NpcAi>();
        CreateNpcAi();
    }
    public void CreateNpcAi()
    {
        for (int i=0; i< _npcdData.Length; i++)
        {
            var temp = Instantiate(_npcdData[i].npcPrefab,gameObject.transform).GetComponent<NpcAi>();
            temp._keyName = _npcdData[i].keyName;
            temp._maxLevel = _npcdData[i].maxLevel;
            temp._startLevel = _npcdData[i].startLevel;
            temp._price = _npcdData[i].price;
            temp._meatPrefab = _meatPrefab;
            temp._bonePrefab = _bonePrefab;
            temp._meatPool = _meatPool;
            temp._currentLevel = _npcdData[i].startLevel;
            temp._npcIcon = _npcdData[i].iconImage;
            _npcAiDic.Add(temp._keyName, temp);
            _npcScriptList.Add(temp);
        }
    }
}
