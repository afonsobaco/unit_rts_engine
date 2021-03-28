using RTSEngine.RTSUserInterface;
using RTSEngine.Core;

namespace RTSEngine.Integration.Scene
{
    public class UISceneIntegratedContentInfo : UIContentInfo
    {
        private ISelectable _selectable;
        public ISelectable Selectable { get => _selectable; set => _selectable = value; }
    }
}