using Zenject;
using RTSEngine.Manager;

namespace RTSEngine.Installers
{
    public class GameManagerInstaller : MonoInstaller
    {
        // public override void InstallBindings()
        // {
        //     SignalBusInstaller.Install(Container);
        //     Container.BindInterfacesAndSelfTo<SelectionManager>().AsSingle();
        //     Container.BindInterfacesAndSelfTo<SelectionInputManager>().AsSingle();
        //     // Container.Bind<IGUIManager>().To<GUIManager>().AsSingle();
        //     Container.Bind<ICameraManager>().To<CameraManager>().AsSingle();

        //     DeclareSignals();
        // }


        // private void DeclareSignals()
        // {
        //     Container.DeclareSignal<SelectableObjectCreatedSignal>();
        //     Container.DeclareSignal<SelectableObjectDeletedSignal>();
        //     // Container.DeclareSignal<UpdateGUISignal>();
        //     Container.DeclareSignal<MiniatureClickSignal>();
        //     Container.DeclareSignal<ProfileInfoClickSignal>();

        //     Container.BindSignal<SelectableObjectCreatedSignal>().ToMethod<SelectionManager>(x => x.AddSelectableObject).FromResolve();
        //     Container.BindSignal<SelectableObjectDeletedSignal>().ToMethod<SelectionManager>(x => x.RemoveSelectableObject).FromResolve();
        //     // Container.BindSignal<UpdateGUISignal>().ToMethod<IGUIManager>(x => x.OnSelectionChange).FromResolve();
        //     Container.BindSignal<MiniatureClickSignal>().ToMethod<SelectionManager>(x => x.DoMiniatureClick).FromResolve();
        //     Container.BindSignal<ProfileInfoClickSignal>().ToMethod<ICameraManager>(x => x.DoProfileInfoClick).FromResolve();
        // }
    }
}