using Zenject;
using RTSEngine.Core;
namespace RTSEngine.Manager.Installers
{
    public class GameManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.BindInterfacesAndSelfTo<SelectionManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<GUIManager>().AsSingle();
            Container.Bind<ICameraManager>().To<CameraManager>().AsSingle();
            DeclareSignals();
        }

        private void DeclareSignals()
        {
            Container.DeclareSignal<UpdateGUISignal>();
            Container.DeclareSignal<SelectableObjectCreatedSignal>();
            Container.DeclareSignal<SelectableObjectDeletedSignal>();

            Container.BindSignal<UpdateGUISignal>().ToMethod<GUIManager>(x => x.OnSelectionChange).FromResolve();
            Container.BindSignal<SelectableObjectCreatedSignal>().ToMethod<SelectionManager>(x => x.AddSelectableObject).FromResolve();
            Container.BindSignal<SelectableObjectDeletedSignal>().ToMethod<SelectionManager>(x => x.RemoveSelectableObject).FromResolve();
        }
    }
}