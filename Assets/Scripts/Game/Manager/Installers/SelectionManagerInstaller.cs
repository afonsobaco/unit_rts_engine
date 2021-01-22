using UnityEngine;
using Zenject;
using RTSEngine.Core.Impls;
using RTSEngine.Core.Enums;
using RTSEngine.Core.Signals;
using RTSEngine.Manager.Enums;
using RTSEngine.Manager.Interfaces;
using RTSEngine.Manager.Impls;
namespace RTSEngine.Manager.Installers
{
    public class SelectionManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.BindInterfacesAndSelfTo<SelectionManager>().AsSingle();
            Container.Bind<ICameraManager>().To<CameraManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputManager>().AsSingle();

            Container.DeclareSignal<SelectableObjectCreatedSignal>();
            Container.DeclareSignal<SelectableObjectDeletedSignal>();

            Container.BindSignal<SelectableObjectCreatedSignal>().ToMethod<ISelectionManager<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>>(x => x.AddSelectableObject).FromResolve();
            Container.BindSignal<SelectableObjectDeletedSignal>().ToMethod<ISelectionManager<SelectableObject, SelectionTypeEnum, ObjectTypeEnum>>(x => x.RemoveSelectableObject).FromResolve();
        }
    }
}