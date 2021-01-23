using RTSEngine.Manager;

namespace RTSEngine.Core
{
    public class SelectableObjectDeletedSignal : ISelectableDeletedSignal<SelectableObject>
    {
        private SelectableObject selectable;

        public SelectableObject Selectable { get => selectable; set => selectable = value; }
    }
}
