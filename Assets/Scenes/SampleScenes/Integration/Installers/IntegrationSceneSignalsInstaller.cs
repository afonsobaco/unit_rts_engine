using Zenject;
using RTSEngine.Signal;

namespace RTSEngine.Integration.Scene
{
    public class IntegrationSceneSignalsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<MoveCameraSignal>();
            Container.DeclareSignal<PartyUpdateSignal>();
            Container.DeclareSignal<UIUpdatePartySignal>();
            Container.DeclareSignal<UIPartySelectionSignal>();
        }
    }
}