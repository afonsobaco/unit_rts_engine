using System;
using UnityEngine;
namespace RTSEngine.Core
{
    public interface ISelectable : IComparable
    {
        int Index { get; set; } //forDebugOnly
        bool IsSelected { get; set; }
        [ObsoleteAttribute]
        bool IsPreSelected { get; set; }
        Vector3 Position { get; set; }
        bool IsHighlighted { get; set; }
    }

}
