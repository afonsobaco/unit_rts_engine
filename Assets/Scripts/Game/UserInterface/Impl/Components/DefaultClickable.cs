using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Utils;
using Zenject;
using UnityEngine.EventSystems;
using System;

namespace RTSEngine.RTSUserInterface
{
    public abstract class DefaultClickable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        private GameSignalBus _signalBus;
        private bool _isMousePressing;
        private bool _isMouseHover;
        private object _objectReference;
        protected GameSignalBus SignalBus { get => _signalBus; set => _signalBus = value; }
        public bool IsMouseHover { get => _isMouseHover; }
        public bool IsMousePressing { get => _isMousePressing; }
        public object ObjectReference { get => _objectReference; set => _objectReference = value; }

        [Inject]
        public void Construct(GameSignalBus signalBus)
        {
            this.SignalBus = signalBus;
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            this._isMouseHover = true;
            DoMouseEnter();
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            this._isMouseHover = false;
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
