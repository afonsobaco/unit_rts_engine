using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectedMiniatureClickSignal : ISelectableSignal<ISelectableObject>, IClick
    {
        private ISelectableObject selectable;
        private KeyButtonType type;

        public ISelectableObject Selectable { get => selectable; set => selectable = value; }
        public KeyButtonType Type { get => type; set => type = value; }
    }
}