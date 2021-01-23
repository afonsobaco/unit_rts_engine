using RTSEngine.Manager;

namespace RTSEngine.Core
{

    //TODO criar interface
    public class SelectableObjectCreatedSignal : ISelectableCreatedSignal<SelectableObject>
    {
        private SelectableObject selectable;

        public SelectableObject Selectable { get => selectable; set => selectable = value; }
    }
}
