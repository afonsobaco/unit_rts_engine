using RTSEngine.Core;
using RTSEngine.Utils;
using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace RTSEngine.Manager
{

    public class SelectableObjectBehaviour : MonoBehaviour, ISelectableObject
    {


        [Space]
        [Header("Prefab Info")]
        [SerializeField] private SelectionMark selectionMark;
        [SerializeField] private SelectionMark preSelectionMark;

        [SerializeField] private SelectableObjectInfoSO selectableObjectInfo;


        // [Space]
        // [Header("Selectable Info")]
        // [SerializeField] private ObjectTypeEnum type;
        // [SerializeField] private Sprite picture;
        // [Space]
        // [SerializeField] ObjectStatus life;
        // [SerializeField] ObjectStatus mana;
        // [Space]

        // [SerializeField] private string typeStr;  //TODO should be on An ScriptableObject
        // [SerializeField] private int selectionOrder;


        private bool isSelected = false;
        private bool isPreSelected = false;
        private SignalBus _signalBus;

        public int Index { get; set; }
        public SelectionMark SelectionMark { get => selectionMark; set => selectionMark = value; }
        public SelectionMark PreSelectionMark { get => preSelectionMark; set => preSelectionMark = value; }

        // public ObjectTypeEnum Type { get => type; set => type = value; }
        // public Sprite Picture { get => picture; set => picture = value; }
        // public string TypeStr { get => typeStr; set => typeStr = value; }
        // public int SelectionOrder { get => selectionOrder; set => selectionOrder = value; }


        // public ObjectStatus Life { get => life; set => life = value; }
        // public ObjectStatus Mana { get => mana; set => mana = value; }

        public bool IsPreSelected
        {
            get { return isPreSelected; }
            set
            {
                ChangePreSelectionMarkStatus(value);
                isPreSelected = value;
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                ChangeSelectionMarkStatus(value);
                isSelected = value;
            }
        }

        public Vector3 Position
        {
            get { return this.transform.position; }
            set
            {
                this.transform.position = value;
            }
        }

        public SelectableObjectInfoSO SelectableObjectInfo { get => selectableObjectInfo; set => selectableObjectInfo = value; }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

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
            _signalBus.Fire(new SelectableObjectDeletedSignal() { Selectable = this });
        }

        void OnEnable()
        {
            _signalBus.Fire(new SelectableObjectCreatedSignal() { Selectable = this });
        }

        private void OnMouseEnter()
        {
            ChangePreSelectionMarkStatus(true);
        }

        private void OnMouseExit()
        {
            ChangePreSelectionMarkStatus(false);
        }

        public bool IsCompatible(ISelectable other)
        {
            if (other != null && other is SelectableObjectBehaviour)
            {
                var second = other as SelectableObjectBehaviour;
                return second.SelectableObjectInfo.Type == this.SelectableObjectInfo.Type && second.SelectableObjectInfo.TypeStr.Equals(this.SelectableObjectInfo.TypeStr);
            }
            return false;
        }

    }



}
