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
        }

        public class Factory : PlaceholderFactory<UIContainer, UIContainerFacade>
        {
        }
    }
}