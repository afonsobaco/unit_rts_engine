using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene

{
    public class UILimitedContainerManager : UIContainerManager
    {
        [SerializeField] protected int _limit = 10;
        public override UIContent AddToContainer(UIContentInfo info)
        {
            if (GetUIContentChildren().Count < _limit)
            {
                return base.AddToContainer(info);
            }
            return null;
        }
    }
}