using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public interface ISelectedInfo
    {
        ISelectableObject Selected { get; set; }
    }
}