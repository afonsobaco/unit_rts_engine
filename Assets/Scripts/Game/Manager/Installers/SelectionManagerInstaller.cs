using UnityEngine;
using Zenject;
using RTSEngine.Core.Impls;
using RTSEngine.Core.Enums;
using RTSEngine.Manager.Enums;
using RTSEngine.Manager.Interfaces;
using RTSEngine.Manager.Impls;
namespace RTSEngine.Manager.Installers
{
    public class SelectionManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISelectionManager<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>>().To<SelectionManager>().AsSingle();
            Container.Bind<ICameraManager>().To<CameraManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputManager>().AsSingle();
        }
    }
}