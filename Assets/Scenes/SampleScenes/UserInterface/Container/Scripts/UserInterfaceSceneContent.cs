using UnityEngine;
using UnityEngine.EventSystems;
using RTSEngine.Utils;
using Zenject;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneContent : UIContent, IPointerClickHandler
    {
        [Inject] private GameSignalBus _signalBus;

        public void OnPointerClick(PointerEventData eventData)
        {
            _signalBus.Fire(new UIRemoveContentSignal() { ContainerInfo = new UIContainerInfo() { ContainerId = this.Container.ContainerId }, Content = this });
        }
    }
}

