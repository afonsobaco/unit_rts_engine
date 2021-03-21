using UnityEngine;
using RTSEngine.Utils;
using Zenject;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneContainerInput : MonoBehaviour
    {

        [Inject] private SignalBus _signalBus;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _signalBus.Fire(new UIAddContentSignal() { ContainerInfo = new UIContainerInfo { ContainerId = "ContainerLeft" }, Info = new UIContentInfo() { } });
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                _signalBus.Fire(new UIAddContentSignal() { ContainerInfo = new UIContainerInfo { ContainerId = "ContainerTop" }, Info = new UIContentInfo() { } });
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                _signalBus.Fire(new UIAddContentSignal() { ContainerInfo = new UIContainerInfo { ContainerId = "ContainerBottom" }, Info = new UIContentInfo() { } });

            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _signalBus.Fire(new UIAddContentSignal() { ContainerInfo = new UIContainerInfo { ContainerId = "ContainerRight" }, Info = new UIContentInfo() { } });

            }
        }
    }
}

