using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expen : BaseObject
{
    private void OnDisable()
    {
        if (_keyName =="점포확장1")
        {
            NodeManager._instance.SettingFirstExpand();
        } else if (_keyName == "점포확장2")
        {
            NodeManager._instance.SettingSecondExpand();
        }
    }

}
