using UnityEngine;
using Zenject;
namespace RTSEngine.Manager
{
    public class GUISelectedPortraitBehaviour : AbstractGUISelectedInfoBehaviour
    {
        [Inject]
        private SignalBus _signalBus;

        public override void DoAction()
        {
            Debug.Log("Portrait!");
            _signalBus.Fire(new SelectedPortraitClickSignal() { Selectable = this.Selected });
        }

    }
}