using UnityEngine;
using Zenject;
using System;

namespace RTSEngine.RTSUserInterface
{
    public class DefaultActionButton : DefaultClickable
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
