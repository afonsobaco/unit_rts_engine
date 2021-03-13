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
                _signalBus.Fire(new AddContentSignal() { Content = new UserInterfaceContent { ContentId = "ContentLeft" } });
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                _signalBus.Fire(new AddContentSignal() { Content = new UserInterfaceContent { ContentId = "ContentTop" } });
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                _signalBus.Fire(new AddContentSignal() { Content = new UserInterfaceContent { ContentId = "ContentBottom" } });

            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _signalBus.Fire(new AddContentSignal() { Content = new UserInterfaceContent { ContentId = "ContentRight" } });

            }
        }
    }
}

