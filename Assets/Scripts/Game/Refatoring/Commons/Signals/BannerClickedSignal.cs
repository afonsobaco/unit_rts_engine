using RTSEngine.Core;
namespace RTSEngine.Signal
{
    public class BannerClickedSignal
    {
        public bool ToRemove { get; set; }
        public object GroupId { get; set; }
    }
}
