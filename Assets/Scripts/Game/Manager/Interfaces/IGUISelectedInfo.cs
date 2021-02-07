using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public interface IGUISelectedInfo
    {
        ISelectableObject Selected { get; set; }
    }
}