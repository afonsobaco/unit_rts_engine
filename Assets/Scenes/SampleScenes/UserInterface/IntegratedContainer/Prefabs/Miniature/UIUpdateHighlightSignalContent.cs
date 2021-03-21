using System.Collections;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.RTSUserInterface.Scene
{

    public class UIUpdateHighlightSignalContent : UIGlobalContainerSignalContent
    {
        public ISelectable Highlighted { get; set; }
    }

}