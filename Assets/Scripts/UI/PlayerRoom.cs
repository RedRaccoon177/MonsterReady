using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoom : UiBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UiManager._instance.OnUi(UiType.PlayerUpgrade);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UiManager._instance.OffUi(UiType.PlayerUpgrade);
        }
    }
}
