using RTSEngine.Manager;

namespace RTSEngine.Core
{
    public class SelectableObjectDeletedSignal : ISelectableDeletedSignal<ISelectable>
    {
        private ISelectable selectable;

        public ISelectable Selectable { get => selectable; set => selectable = value; }
    }
}
