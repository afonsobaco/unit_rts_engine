using RTSEngine.Core;
using RTSEngine.Utils;
using RTSEngine.Signal;
using UnityEngine;
using Zenject;
namespace RTSEngine.Refactoring.Scene.Selection
{
    public class SelectionSceneObject : DefaultSelectable
    {
        [SerializeField] private SelectionMark selectionMark;
        [SerializeField] private Renderer mainRenderer;

        private GameSignalBus _signalBus;

        public Renderer MainRenderer { get => mainRenderer; set => mainRenderer = value; }

        [Inject]
        public void Construct(GameSignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Update()
        {
            if (this.IsSelected)
            {
                if (!selectionMark.gameObject.activeInHierarchy)
                    selectionMark.gameObject.SetActive(true);
            }
            else if (selectionMark.gameObject.activeInHierarchy)
            {
                selectionMark.gameObject.SetActive(false);
            }
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
            _signalBus.Fire(new IndividualSelectionSignal() { Clicked = this, OnSelection = false });
        }

    }

}