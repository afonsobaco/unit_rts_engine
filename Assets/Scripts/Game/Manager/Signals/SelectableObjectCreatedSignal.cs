using RTSEngine.Manager;

namespace RTSEngine.Core
{

    //TODO criar interface
    public class SelectableObjectCreatedSignal : ISelectableCreatedSignal<ISelectable>
    {
        private ISelectable selectable;

        public ISelectable Selectable { get => selectable; set => selectable = value; }
    }
}
