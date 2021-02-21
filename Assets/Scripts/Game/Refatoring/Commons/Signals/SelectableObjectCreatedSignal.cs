using RTSEngine.Core;

namespace RTSEngine.Signal
{
    public class SelectableObjectCreatedSignal : ISelectableSignal<ISelectable>
    {
        private ISelectable selectable;

        public ISelectable Selectable { get => selectable; set => selectable = value; }
    }
}
