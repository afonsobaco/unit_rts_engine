using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectableObjectCreatedSignal : ISelectableCreatedSignal<ISelectableObjectBehaviour>
    {
        private ISelectableObjectBehaviour selectable;

        public ISelectableObjectBehaviour Selectable { get => selectable; set => selectable = value; }
    }
}
