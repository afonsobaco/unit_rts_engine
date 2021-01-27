using UnityEngine;
namespace RTSEngine.Core
{
    public interface ISelectable
    {
        int Index { get; set; } //forDebugOnly
        bool IsSelected { get; set; }
        bool IsPreSelected { get; set; }
        Vector3 Position { get; set; }
        void ChangePreSelectionMarkStatus(bool value);
        void ChangeSelectionMarkStatus(bool value);
        bool IsCompatible(ISelectable other);


    }

}
