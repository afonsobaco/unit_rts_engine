using UnityEngine;
using Zenject;
using RTSEngine.Core.Impls;
using RTSEngine.Manager.Interfaces;
using RTSEngine.Manager.Impls;

namespace RTSEngine.Manager.Installers
{
    public class SelectionManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISelectionManager<SelectableObject>>().To<SelectionManager>().AsSingle();
            Container.Bind<ICameraManager>().To<CameraManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputManager>().AsSingle();
        }
    }
}