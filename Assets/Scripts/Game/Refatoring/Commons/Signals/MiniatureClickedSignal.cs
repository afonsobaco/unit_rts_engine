using RTSEngine.Core;
namespace RTSEngine.Signal
{
    public class MiniatureClickedSignal
    {
        public bool AsSubGroup { get; set; }
        public bool ToRemove { get; set; }
        public ISelectable Selected { get; set; }
    }
}
