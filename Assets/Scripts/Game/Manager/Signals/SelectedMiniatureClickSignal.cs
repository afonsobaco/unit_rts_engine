using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectedMiniatureClickSignal : ISelectableSignal<ISelectableObject>
    {
        private ISelectableObject selectable;

        public ISelectableObject Selectable { get => selectable; set => selectable = value; }
    }
}