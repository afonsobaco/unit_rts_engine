using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Utils;
using Zenject;
using UnityEngine.EventSystems;

namespace RTSEngine.RTSUserInterface
{
    public abstract class DefaultClickable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] private bool _enableToolTip;
        [SerializeField] private float _toolTipTime;
        private GameSignalBus _signalBus;
        private bool _isMousePressing;
        private bool _isMouseHover;
        private bool _showToolTip;
        private object _objectReference;
        private float _lastTimeEnter;

        protected GameSignalBus SignalBus { get => _signalBus; set => _signalBus = value; }
        public bool IsMouseHover { get => _isMouseHover; }
        public bool IsMousePressing { get => _isMousePressing; }
        public object ObjectReference { get => _objectReference; set => _objectReference = value; }
        public bool ShowToolTip { get => _showToolTip; set => _showToolTip = value; }
        public bool EnableToolTip { get => _enableToolTip; set => _enableToolTip = value; }

        [Inject]
        public void Construct(GameSignalBus signalBus)
        {
            this.SignalBus = signalBus;
        }

        private void Update()
        {
            if (this._enableToolTip && this._isMouseHover && !this._showToolTip && Time.time - this._lastTimeEnter > this._toolTipTime)
            {
                this._showToolTip = true;
            }
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            this._lastTimeEnter = Time.time;
            this._isMouseHover = true;
            DoMouseEnter();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            this._isMouseHover = false;
            this._showToolTip = false;
            DoMouseExit();
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            DoPress();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            DoRelease();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            DoClick();
        }

        public virtual void UpdateApperance() { }
        public virtual void DoMouseEnter() { }
        public virtual void DoMouseExit() { }
        public virtual void DoPress() { }
        public virtual void DoRelease() { }
        public virtual void DoClick() { }

    }
}
