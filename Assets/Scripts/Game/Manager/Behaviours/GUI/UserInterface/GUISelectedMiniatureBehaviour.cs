using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RTSEngine.Manager
{
    public class GUISelectedMiniatureBehaviour : AbstractGUISelectedInfoBehaviour
    {


        [SerializeField] private Image[] selectionBorder;
        public Image[] SelectionBorder { get => selectionBorder; set => selectionBorder = value; }

        public override void DoAction(params object[] parameters)
        {
            if (parameters.Length > 0 && parameters[0] is KeyButtonType)
            {
                SignalBus.Fire(new SelectedMiniatureClickSignal() { Selectable = this.Selected, Type = (KeyButtonType)parameters[0] });
            }
        }

    }
}