using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Utils;

namespace RTSEngine.Manager
{
    public interface ISelectableObjectBehaviour : ISelectable
    {
        int SelectionOrder { get; set; }
        ObjectTypeEnum Type { get; set; }
        SelectionMark SelectionMark { get; set; }
        SelectionMark PreSelectionMark { get; set; }
        Sprite Picture { get; set; }

        ObjectStatus Life { get; set; }
        ObjectStatus Mana { get; set; }

        string TypeStr { get; set; }

    }



}
