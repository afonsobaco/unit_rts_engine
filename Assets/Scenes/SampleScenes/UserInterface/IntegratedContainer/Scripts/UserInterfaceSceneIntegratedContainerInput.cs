using UnityEngine;
using RTSEngine.Utils;
using Zenject;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneIntegratedContainerInput : UserInterfaceContainerInput
    {

        [Inject] private GameSignalBus _signalBus;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _signalBus.Fire(new AddContentSignal() { Content = new UserInterfaceContent { ContentId = "Log" } });
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                _signalBus.Fire(new AddContentSignal() { Content = new UserInterfaceContent { ContentId = "Banner" } });
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _signalBus.Fire(new AddContentSignal() { Content = new UserInterfaceContent { ContentId = "Info" } });

            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                _signalBus.Fire(new AddContentSignal() { Content = new UserInterfaceContent { ContentId = "Miniature" } });
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                _signalBus.Fire(new AddContentSignal() { Content = new UserInterfaceContent { ContentId = "Item" } });
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _signalBus.Fire(new AddContentSignal() { Content = new UserInterfaceContent { ContentId = "Action" } });

            }
        }
    }
}

