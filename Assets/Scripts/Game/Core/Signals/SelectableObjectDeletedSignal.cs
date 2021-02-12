using RTSEngine.Core;

namespace RTSEngine.Core
{
    public class SelectableObjectDeletedSignal : ISelectableSignal<ISelectable>
    {
        private ISelectable selectable;

        public ISelectable Selectable { get => selectable; set => selectable = value; }
    }
}
