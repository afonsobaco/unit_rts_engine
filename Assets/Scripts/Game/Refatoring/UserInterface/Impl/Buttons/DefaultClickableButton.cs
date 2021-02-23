using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{
    public abstract class DefaultClickableButton : MonoBehaviour, UIClickable
    {
        private GameSignalBus _signalBus;
        private bool _pressing;
        private bool _inside;
        protected GameSignalBus SignalBus { get => _signalBus; set => _signalBus = value; }

        [Inject]
        public void Construct(GameSignalBus signalBus)
        {
            this.SignalBus = signalBus;
        }

        private void Update()
        {
            if (_pressing && _inside)
            {
                DoPress();
            }
        }

        public abstract void DoClick();

        public abstract void DoPress();

        public virtual void OnPress(bool pressing)
        {
            this._pressing = pressing;
        }

        public virtual void OnHover(bool inside)
        {
            this._inside = inside;
        }

    }
}
