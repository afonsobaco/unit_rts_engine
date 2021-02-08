using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Manager
{
    public interface IGUISelectableObjectInfo
    {
        int SelectionOrder { get; set; }
        Sprite Picture { get; set; }
        ObjectTypeEnum Type { get; set; }
        int Life { get; set; }
        int Mana { get; set; }
        string ObjectName { get; set; }
    }
}
