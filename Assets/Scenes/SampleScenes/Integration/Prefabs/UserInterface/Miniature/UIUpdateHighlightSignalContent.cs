using RTSEngine.Core;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{

    public class UIUpdateHighlightSignalContent : UIGlobalContainerSignalContent
    {
        public ISelectable Highlighted { get; set; }
    }

}