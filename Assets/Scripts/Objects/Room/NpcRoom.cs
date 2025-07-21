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
            UiManager._instance.OnUi(UiType.NpcBuy);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            UiManager._instance.OffUi(UiType.NpcBuy);
        }
    }
}
