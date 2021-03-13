using UnityEngine;
using RTSEngine.Utils;
using Zenject;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneContainerInput : UserInterfaceContainerInput
    {

        [Inject] private GameSignalBus _signalBus;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _signalBus.Fire(new UIAddContentSignal() { Info = new UIContentInfo { ContainerId = "ContainerLeft" } });
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                _signalBus.Fire(new UIAddContentSignal() { Info = new UIContentInfo { ContainerId = "ContainerTop" } });
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                _signalBus.Fire(new UIAddContentSignal() { Info = new UIContentInfo { ContainerId = "ContainerBottom" } });

            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _signalBus.Fire(new UIAddContentSignal() { Info = new UIContentInfo { ContainerId = "ContainerRight" } });

            }
        }
    }
}

