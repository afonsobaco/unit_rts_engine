using RTSEngine.Core;

namespace RTSEngine.Signal
{
    public class IndividualSelectionSignal
    {
        public ISelectable Clicked { get; set; }
        public bool OnSelection { get; set; }
    }
}