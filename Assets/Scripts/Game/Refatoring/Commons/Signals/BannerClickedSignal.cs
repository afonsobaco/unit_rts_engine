using RTSEngine.Core;
namespace RTSEngine.Signal
{
    public class BannerClickedSignal
    {
        public bool ToRemove { get; set; }
        public object PartyId { get; set; }
    }
}
