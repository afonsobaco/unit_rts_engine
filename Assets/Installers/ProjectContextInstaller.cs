using Zenject;
using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
        }
    }
}