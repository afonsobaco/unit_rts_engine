using Zenject;
using RTSEngine.Core;
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

            Container.BindSignal<SelectableObjectCreatedSignal>().ToMethod<SelectionManager>(x => x.AddSelectableObject).FromResolve();
            Container.BindSignal<SelectableObjectDeletedSignal>().ToMethod<SelectionManager>(x => x.RemoveSelectableObject).FromResolve();
        }
    }
}