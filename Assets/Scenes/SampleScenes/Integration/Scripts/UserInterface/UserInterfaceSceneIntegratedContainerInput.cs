using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using Zenject;
using RTSEngine.RTSUserInterface;
using RTSEngine.Signal;

namespace RTSEngine.Integration.Scene
{
    public class UserInterfaceSceneIntegratedContainerInput : MonoBehaviour
    {

        [Inject] private SignalBus _signalBus;

        private void Start() {
            _signalBus.DeclareSignal<MoveCameraSignal>();
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                UIMiniatureContainerInfo previousInfo = new UIMiniatureContainerInfo() { OldSelection = true, NextHighlight = !Input.GetKey(KeyCode.LeftShift), ContainerId = "MiniatureContainer" };
                _signalBus.Fire(new UIUpdateContainerSignal() { ContainerInfo = previousInfo });
            }


            if (Input.GetKey(KeyCode.Space))
            {
                _signalBus.Fire(new MoveCameraSignal() { });
            }
        }

    }
}

