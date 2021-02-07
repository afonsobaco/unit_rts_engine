using RTSEngine.Core;
using UnityEngine;
namespace RTSEngine.Manager
{
    public interface ISelectionSettings
    {
        Vector2 MinViewport { get; }
        Vector2 MaxViewport { get; }
        int Limit { get; }
        ObjectTypeEnum[] CanGroup { get; }
        ObjectTypeEnum[] Primary { get; }
        ObjectTypeEnum[] Secondary { get; }
        SameTypeSelectionModeEnum Mode { get; }
        ObjectTypeEnum[] Restricted { get; }
    }

}