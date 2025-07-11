using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UiManager._instance.SetActive(UiType.PlayerUpgrade,true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UiManager._instance.SetActive(UiType.PlayerUpgrade,false);
        }
    }
}
