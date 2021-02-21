using RTSEngine.Core;

namespace RTSEngine.Signal
{
    public class ChangeSelectionSignal
    {
        public ISelectable[] Selection { get; set; }
    }
}