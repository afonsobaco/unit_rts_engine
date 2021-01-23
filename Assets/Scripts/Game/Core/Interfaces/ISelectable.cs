namespace RTSEngine.Core
{
    public interface ISelectable
    {
        bool IsSelected { get; set; }
        bool IsPreSelected { get; set; }
    }

}
