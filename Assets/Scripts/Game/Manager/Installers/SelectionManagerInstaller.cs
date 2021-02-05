using Zenject;
namespace RTSEngine.Manager.Installers
{
    public class SelectionManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.BindInterfacesAndSelfTo<SelectionManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputManager>().AsSingle();
            Container.Bind<ICameraManager>().To<CameraManager>().AsSingle();
            Container.Bind<IGUIManager>().To<GUIManager>().AsSingle();
            DeclareSignals();
        }


        private void DeclareSignals()
        {
            Container.DeclareSignal<SelectableObjectCreatedSignal>();
            Container.DeclareSignal<SelectableObjectDeletedSignal>();
            Container.DeclareSignal<UpdateGUISignal>();

            Container.BindSignal<SelectableObjectCreatedSignal>().ToMethod<SelectionManager>(x => x.AddSelectableObject).FromResolve();
            Container.BindSignal<SelectableObjectDeletedSignal>().ToMethod<SelectionManager>(x => x.RemoveSelectableObject).FromResolve();
            Container.BindSignal<UpdateGUISignal>().ToMethod<GUIManager>(x => x.OnSelectionChange).FromResolve();
        }
    }
}