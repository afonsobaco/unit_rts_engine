using System.Net.Mime;
using RTSEngine.Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RTSEngine.Manager
{
    public abstract class AbstractGUISelectedInfoBehaviour : ZenAutoInjecter, IGUISelectedInfo, IGUIClickableElement
    {
        [SerializeField] private Image picture;
        [SerializeField] private GUIStatusBarBehaviour lifeBar;
        [SerializeField] private GUIStatusBarBehaviour manaBar;
        private ISelectableObject selected;
        private SignalBus _signalBus;

        public ISelectableObject Selected { get => selected; set => selected = value; }
        public Image Picture { get => picture; set => picture = value; }
        public GUIStatusBarBehaviour LifeBar { get => lifeBar; set => lifeBar = value; }
        public GUIStatusBarBehaviour ManaBar { get => manaBar; set => manaBar = value; }
        public SignalBus SignalBus { get => _signalBus; set => _signalBus = value; }

        [Inject]
        public void InitSignalBus(SignalBus signalBus)
        {
            this.SignalBus = signalBus;
        }
        public abstract void DoAction();

    }
}