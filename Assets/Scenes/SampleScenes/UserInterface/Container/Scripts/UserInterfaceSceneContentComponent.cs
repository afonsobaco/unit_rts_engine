using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using RTSEngine.Utils;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneContentComponent : UserInterfaceContentComponent, IPointerClickHandler
    {

        private GameSignalBus _signalBus;

        [Inject]
        public void Constructor(GameSignalBus signalBus)
        {

            this._signalBus = signalBus;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _signalBus.Fire(new RemoveContentSignal() { Component = this });
        }
    }
}

