using System;
using UnityEngine;
using RTSEngine.Core;
using Zenject;

namespace RTSEngine.RTSUserInterface
{
    public class DefaultPortraitButton : DefaultClickable
    {
        public override void DoClick()
        {
            Debug.Log("Unimplemented: Portrait");
        }

        public override void DoPress()
        {
            Debug.Log("Unimplemented: Portrait Press");
        }

        public class Factory : PlaceholderFactory<DefaultPortraitButton>
        {
        }
    }
}
