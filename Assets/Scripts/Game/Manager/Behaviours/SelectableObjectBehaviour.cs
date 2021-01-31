using RTSEngine.Core;
using RTSEngine.Utils;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public interface ISelectableObjectBehaviour : ISelectable
    {
        ObjectTypeEnum Type { get; set; }
        SelectionMark SelectionMark { get; set; }
        SelectionMark PreSelectionMark { get; set; }

    }

    public class SelectableObjectBehaviour : MonoBehaviour, ISelectableObjectBehaviour
    {

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            SignalBus = signalBus;
        }

        [SerializeField] private ObjectTypeEnum type;
        [SerializeField] private SelectionMark selectionMark;
        [SerializeField] private SelectionMark preSelectionMark;
        [SerializeField] private string typeStr;  //TODO should be an Enum?

        private bool isPreSelected = false;
        public bool IsPreSelected
        {
            get { return isPreSelected; }
            set
            {
                ChangePreSelectionMarkStatus(value);
                isPreSelected = value;
            }
        }
        private bool isSelected = false;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                ChangeSelectionMarkStatus(value);
                isSelected = value;
            }
        }

        private SignalBus _signalBus;
        public SignalBus SignalBus { get => _signalBus; set => _signalBus = value; }
        public Vector3 Position
        {
            get { return this.transform.position; }
            set
            {
                this.transform.position = value;
            }
        }

        public int Index { get; set; }
        public ObjectTypeEnum Type { get => type; set => type = value; }
        public SelectionMark SelectionMark { get => selectionMark; set => selectionMark = value; }
        public SelectionMark PreSelectionMark { get => preSelectionMark; set => preSelectionMark = value; }

        public void ChangeSelectionMarkStatus(bool value)
        {
            if (SelectionMark)
            {
                SelectionMark.gameObject.SetActive(value);
            }
        }

        public void ChangePreSelectionMarkStatus(bool value)
        {
            if (PreSelectionMark)
            {
                PreSelectionMark.gameObject.SetActive(value);
            }
        }

        void OnDisable()
        {
            SignalBus.Fire(new SelectableObjectDeletedSignal() { Selectable = this });
        }

        void OnEnable()
        {
            SignalBus.Fire(new SelectableObjectCreatedSignal() { Selectable = this });
        }

        public bool IsCompatible(ISelectable other)
        {
            if (other != null && other is SelectableObjectBehaviour)
            {
                var second = other as SelectableObjectBehaviour;
                return second.Type == this.Type && second.typeStr.Equals(this.typeStr);
            }
            return false;
        }
    }



}
