using RTSEngine.Core;

namespace RTSEngine.Signal
{
    public class SelectableObjectCreatedSignal : ISelectableSignal<ISelectable>
    {
        public ISelectable Selectable { get; set; }
    }
}
