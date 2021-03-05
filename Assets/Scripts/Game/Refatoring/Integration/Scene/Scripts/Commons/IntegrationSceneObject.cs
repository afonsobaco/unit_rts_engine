using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class IntegrationSceneObject : DefaultSelectable
    {
        [SerializeField] private Sprite picture;
        [SerializeField] private string objectName;
        [SerializeField] private SelectionMark selectionMark;

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

        public Sprite Picture { get => picture; set => picture = value; }
        public string ObjectName { get => objectName; set => objectName = value; }

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

    }

}