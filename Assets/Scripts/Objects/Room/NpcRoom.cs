using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcRoom : MonoBehaviour
{
    [SerializeField] NpcSpawner _npcSpawner;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UiManager._instance.SetActive(UiType.NpcBuy,true);
            var temp = UiManager._instance.GetUiComponent<NpcBuyUi>(UiType.NpcBuy);
            temp.SetUi(_npcSpawner);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UiManager._instance.SetActive(UiType.NpcBuy, false);
        }
    }
}
