using System;
using UnityEngine;
using Zenject;

namespace RTSEngine.RTSUserInterface
{
    public class DefaultItemButton : DefaultClickable
    {
        public override void DoClick()
        {
            Debug.Log("Unimplemented: Item");
        }

        public override void DoPress()
        {
            Debug.Log("Unimplemented: Item Press");
        }

        public class Factory : PlaceholderFactory<DefaultItemButton>
        {
        }
    }
}
