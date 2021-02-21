using RTSEngine.Core;
namespace RTSEngine.Signal
{
    public class MiniatureClickedSignal
    {
        public bool AsGroup { get; set; }
        public bool ToRemove { get; set; }
        public ISelectable Selected { get; set; }
    }
}
