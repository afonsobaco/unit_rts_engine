using UnityEngine;
using Zenject;
namespace RTSEngine.Manager
{
    public class ProfileInfoBehaviour : SelectedInfoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        public override void DoAction()
        {
            _signalBus.Fire(new ProfileInfoClickSignal() { Selectable = this.Selected });
        }
    }
}