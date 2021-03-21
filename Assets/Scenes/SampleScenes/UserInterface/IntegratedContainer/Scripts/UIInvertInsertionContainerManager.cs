using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene

{
    public class UIInvertInsertionContainerManager : UIContainerManager
    {
        public override UIContent AddToContainer(UIContentInfo info)
        {
            UIContent uIContent = base.AddToContainer(info);
            uIContent.transform.SetAsFirstSibling();
            return uIContent;
        }
    }
}