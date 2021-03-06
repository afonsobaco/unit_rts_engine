using RTSEngine.Core;

namespace RTSEngine.Signal
{
    public class SelectableObjectUpdatedSignal : ISelectableSignal<ISelectable>
    {
        public ISelectable Selectable { get; set; }
    }
}
