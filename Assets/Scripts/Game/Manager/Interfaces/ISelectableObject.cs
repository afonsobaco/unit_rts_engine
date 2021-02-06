using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Utils;

namespace RTSEngine.Manager
{
    public interface ISelectableObject : ISelectable
    {
        SelectionMark SelectionMark { get; set; }
        SelectionMark PreSelectionMark { get; set; }
        SelectableObjectInfoSO SelectableObjectInfo { get; set; }
        bool IsCompatible(ISelectable other);


    }



}
