using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.RTSUserInterface
{
    public class DefaultActionButton : DefaultClickableButton
    {
        public override void DoClick()
        {
            Debug.Log("Unimplemented: Action");
        }

        public override void DoPress()
        {
            Debug.Log("Unimplemented: Action Press");
        }
        public class Factory : PlaceholderFactory<DefaultActionButton>
        {
        }
    }
}
