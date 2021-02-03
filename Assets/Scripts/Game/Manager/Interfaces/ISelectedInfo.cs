using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public interface ISelectedInfo
    {
        ISelectableObjectBehaviour Selected { get; set; }
    }
}