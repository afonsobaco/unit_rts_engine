using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{

    public class GameDefaultObject : DefaultSelectable
    {
        [SerializeField] private SelectionMark selectionMark;
        public int selectionOrder;
        public string objectType;

        private SignalBus _signalBus;

        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                base.IsSelected = value;
                if (selectionMark)
                    selectionMark.gameObject.SetActive(value);
            }
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void Update()
        {
        }

        private void OnEnable()
        {
            _signalBus.Fire(new SelectableObjectCreatedSignal { Selectable = this });
        }

        private void OnDisable()
        {
            _signalBus.Fire(new SelectableObjectDeletedSignal { Selectable = this });
        }

        private void OnMouseUpAsButton()
        {
            _signalBus.Fire(new IndividualSelectionSignal() { Clicked = this });
        }

        public override int CompareTo(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return -1;
            }

            var other = obj as GameDefaultObject;

            int v = other.selectionOrder - this.selectionOrder;

            return v;
        }

    }

}