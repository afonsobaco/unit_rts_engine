using RTSEngine.Core;

namespace RTSEngine.Signal
{
    public class SelectableObjectDeletedSignal : ISelectableSignal<ISelectable>
    {
        public ISelectable Selectable { get; set; }
    }
}
