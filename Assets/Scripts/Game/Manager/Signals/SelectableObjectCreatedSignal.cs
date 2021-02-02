using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectableObjectCreatedSignal : ISelectableSignal<ISelectableObjectBehaviour>
    {
        private ISelectableObjectBehaviour selectable;

        public ISelectableObjectBehaviour Selectable { get => selectable; set => selectable = value; }
    }
}
