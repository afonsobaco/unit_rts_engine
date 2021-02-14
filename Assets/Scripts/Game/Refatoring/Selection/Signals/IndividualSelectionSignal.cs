using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class IndividualSelectionSignal
    {
        public bool BlockAreaSelection { get; set; }
        public ISelectable Clicked { get; set; }
    }
}