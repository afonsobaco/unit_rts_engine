using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectedPortraitClickSignal : ISelectableSignal<ISelectableObject>
    {
        private ISelectableObject selectable;

        public ISelectableObject Selectable { get => selectable; set => selectable = value; }
    }
}