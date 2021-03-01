using UnityEngine;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class DefaultItemButton : DefaultClickableButton
    {
        public override void DoClick()
        {
            Debug.Log("Item");
        }

        public override void DoPress()
        {
            Debug.Log("Item Press");
        }
        public class Factory : PlaceholderFactory<DefaultItemButton>
        {
        }
    }
}
