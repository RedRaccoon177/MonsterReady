using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expen : BaseObject
{
    private void OnDisable()
    {
        if (_keyName =="expand1")
        {
            NodeManager._instance.SettingFirstExpand();
        } else if (_keyName == "expand2")
        {
            NodeManager._instance.SettingSecondExpand();
        }
    }

}
