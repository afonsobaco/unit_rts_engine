using RTSEngine.Core;

namespace RTSEngine.Signal
{
    public class IndividualSelectionSignal
    {
        public bool BlockAreaSelection { get; set; }
        public ISelectable Clicked { get; set; }
    }
}