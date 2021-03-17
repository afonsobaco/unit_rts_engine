using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene

{
    public class UILimitedContainerManager : UIContainerManager
    {
        [SerializeField] protected int _limit = 10;
        public override UIContent AddToContainer(UIAddContentSignal signal)
        {
            if (GetUIContentChildren().Count < _limit)
            {
                return base.AddToContainer(signal);
            }
            return null;
        }
    }
}