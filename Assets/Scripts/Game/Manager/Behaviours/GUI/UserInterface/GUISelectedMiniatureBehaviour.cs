using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RTSEngine.Manager
{
    public class GUISelectedMiniatureBehaviour : AbstractGUISelectedInfoBehaviour
    {


        [SerializeField] private Image selectionBorder;
        public Image SelectionBorder { get => selectionBorder; set => selectionBorder = value; }

        public override void DoAction()
        {
            Debug.Log("Miniature!");
            SignalBus.Fire(new SelectedMiniatureClickSignal() { Selectable = this.Selected });
        }

    }
}