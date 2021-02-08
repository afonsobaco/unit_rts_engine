using UnityEngine;
using Zenject;
namespace RTSEngine.Manager
{
    public class GUISelectedPortraitBehaviour : AbstractGUISelectedInfoBehaviour
    {
        [Inject]
        private SignalBus _signalBus;

        public override void DoAction(params object[] parameters)
        {
            if (parameters.Length > 0 && parameters[0] is KeyButtonType)
            {
                SignalBus.Fire(new SelectedPortraitClickSignal() { Selectable = this.Selected, Type = (KeyButtonType)parameters[0] });
            }

        }

    }
}