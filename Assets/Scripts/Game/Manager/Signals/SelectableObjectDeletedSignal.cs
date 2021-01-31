using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectableObjectDeletedSignal : ISelectableDeletedSignal<ISelectableObjectBehaviour>
    {
        private ISelectableObjectBehaviour selectable;

        public ISelectableObjectBehaviour Selectable { get => selectable; set => selectable = value; }
    }
}
