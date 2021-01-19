using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class SelectionManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISelectionManager>().To<SelectionManager>().AsSingle();
            Container.Bind<ICameraManager>().To<CameraManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputManager>().AsSingle();
        }
    }
}