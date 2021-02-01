using RTSEngine.Core;
namespace RTSEngine.Manager
{
    public interface ISelectionSettings
    {
        int Limit { get; }
        ObjectTypeEnum[] CanGroup { get; }
        ObjectTypeEnum[] Primary { get; }
        ObjectTypeEnum[] Secondary { get; }
        SameTypeSelectionModeEnum Mode { get; }
    }

}