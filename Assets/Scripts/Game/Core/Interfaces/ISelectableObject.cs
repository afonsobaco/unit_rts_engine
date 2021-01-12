using UnityEngine;

namespace RTSEngine.Core
{
    public interface ISelectableObject
    {
        bool IsSelected { get; set; }
        bool IsPreSelected { get; set; }
    }

}
