using UnityEngine;
using Zenject;
using RTSEngine.Core;

namespace RTSEngine.Manager
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