using System;
using UnityEngine;
using RTSEngine.Core;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class DefaultPortraitButton : DefaultClickableButton
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
