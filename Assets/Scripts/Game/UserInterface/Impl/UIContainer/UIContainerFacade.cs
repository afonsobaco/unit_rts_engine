using Zenject;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{
    public class UIContainerFacade : Kernel
    {
        private UIContainer container;

        public UIContainerFacade(UIContainer container)
        {
            this.container = container;
            Debug.Log("Container for: " + container.ContainerId);
        }

        public class Factory : PlaceholderFactory<UIContainer, UIContainerFacade>
        {
        }
    }
}