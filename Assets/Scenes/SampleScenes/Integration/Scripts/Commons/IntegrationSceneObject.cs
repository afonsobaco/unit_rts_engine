using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Integration.Scene
{
    public class IntegrationSceneObject : DefaultSelectable
    {
        [SerializeField] private Sprite picture;
        [SerializeField] private string objectName;
        [SerializeField] private SelectionMark selectionMark;
        [SerializeField] private float speed = 15f;

        public virtual bool IsHighlighted { get; set; }
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

        private void Update()
        {
            transform.Translate(GetRandomMovement() * speed * Time.deltaTime);
        }

        private static Vector3 GetRandomMovement()
        {
            return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        }
    }

}