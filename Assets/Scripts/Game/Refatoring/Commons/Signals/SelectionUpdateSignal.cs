using RTSEngine.Core;

namespace RTSEngine.Signal
{
    public class SelectionUpdateSignal
    {
        public ISelectable[] Selection { get; set; }
        public bool IsUISelection { get; set; }
    }
}
