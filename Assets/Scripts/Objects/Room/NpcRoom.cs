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
            Debug.Log(111);
            var temp = UiManager._instance.OnNpcBuyUi();
            temp.SetUi(_npcSpawner);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        UiManager._instance.OffNpcBuyUi();
    }
}
