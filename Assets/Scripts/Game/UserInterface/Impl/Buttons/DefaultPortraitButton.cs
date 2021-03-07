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
        }

        public override void DoPress()
        {
        }
        public class Factory : PlaceholderFactory<DefaultPortraitButton>
        {
        }
    }
}
