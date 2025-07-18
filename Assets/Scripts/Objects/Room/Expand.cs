using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expand : BaseObject
{
    private void OnDisable()
    {
        if (GetKeyName() == "expand1")
        {
            NodeManager._instance.SettingFirstExpand();
        } else if (GetKeyName() == "expand2")
        {
            NodeManager._instance.SettingSecondExpand();
        }
    }

}
