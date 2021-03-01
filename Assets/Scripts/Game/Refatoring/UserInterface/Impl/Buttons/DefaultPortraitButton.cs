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
            Debug.Log("Portrait");
        }

        public override void DoPress()
        {
            if (ObjectReference is ISelectable)
                Debug.Log((ObjectReference as ISelectable).Position);
        }
        public class Factory : PlaceholderFactory<DefaultPortraitButton>
        {
        }
    }
}
