using Zenject;
using UnityEngine;
using RTSEngine.Utils;

namespace RTSEngine.Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.Bind<GameSignalBus>().AsSingle().MoveIntoAllSubContainers();

        }
    }
}