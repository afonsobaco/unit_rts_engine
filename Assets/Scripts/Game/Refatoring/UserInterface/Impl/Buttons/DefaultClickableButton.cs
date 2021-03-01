using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{
    public abstract class DefaultClickableButton : MonoBehaviour, UIClickable
    {
        private GameSignalBus _signalBus;
        private bool _isMousePressing;
        private bool _isHover;

        private object _objectReference;
        public object ObjectReference { get => _objectReference; set => _objectReference = value; }
        protected GameSignalBus SignalBus { get => _signalBus; set => _signalBus = value; }

        [Inject]
        public void Construct(GameSignalBus signalBus)
        {
            this.SignalBus = signalBus;
        }

        private void Update()
        {
            if (_isMousePressing && _isHover)
            {
                DoPress();
            }
        }

        public abstract void DoClick();

        public abstract void DoPress();
        public virtual void UpdateApperance() { }

        public virtual void OnPress(bool pressing)
        {
            this._isMousePressing = pressing;
        }

        public virtual void OnHover(bool hover)
        {
            this._isHover = hover;
        }


    }
}
