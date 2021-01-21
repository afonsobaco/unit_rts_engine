using UnityEngine;

namespace RTSEngine.Core.Interfaces
{
    public interface ISelectableObject
    {
        bool IsSelected { get; set; }
        bool IsPreSelected { get; set; }
    }

}
