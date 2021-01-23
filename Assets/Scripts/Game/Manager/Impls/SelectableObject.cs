using RTSEngine.Core;
using RTSEngine.Utils;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class SelectableObject : MonoBehaviour, ISelectable
    {
        private bool _selected = false;
        private ObjectTypeEnum type;
        private SelectionMark selectionMark;
        private SelectionMark preSelectionMark;

        //TODO should be an Enum?
        private string typeStr;

        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            SignalBus = signalBus;
        }

        public bool IsSelected
        {
            get { return Selected; }
            set
            {
                if (SelectionMark)
                    SelectionMark.transform.gameObject.SetActive(value);
                Selected = value;
            }
        }
        private bool _preSelected = false;

        public bool IsPreSelected
        {
            get { return PreSelected; }
            set
            {
                if (PreSelectionMark)
                    PreSelectionMark.transform.gameObject.SetActive(value);
                PreSelected = value;
            }
        }

        public bool Selected { get => _selected; set => _selected = value; }
        public ObjectTypeEnum Type { get => type; set => type = value; }
        public SelectionMark SelectionMark { get => selectionMark; set => selectionMark = value; }
        public SelectionMark PreSelectionMark { get => preSelectionMark; set => preSelectionMark = value; }
        public string TypeStr { get => typeStr; set => typeStr = value; }
        public SignalBus SignalBus { get => _signalBus; set => _signalBus = value; }
        public bool PreSelected { get => _preSelected; set => _preSelected = value; }

        void OnEnable()
        {
            SignalBus.Fire(new SelectableObjectCreatedSignal() { Selectable = this });
        }

        void OnDisable()
        {
            SignalBus.Fire(new SelectableObjectDeletedSignal() { Selectable = this });
        }

    }

}
