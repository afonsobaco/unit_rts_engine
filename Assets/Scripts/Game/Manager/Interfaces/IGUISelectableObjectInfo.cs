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
        ObjectStatus Life { get; set; }
        ObjectStatus Mana { get; set; }
        string TypeStr { get; set; }
    }
}
