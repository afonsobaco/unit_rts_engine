using RTSEngine.Core;
using RTSEngine.Utils;
using UnityEngine;
using Zenject;
using System;

namespace RTSEngine.Manager
{

    public class SelectableObjectBehaviour : MonoBehaviour, ISelectableObject
    {

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            SignalBus = signalBus;
        }

        [Space]
        [Header("Prefab Info")]
        [SerializeField] private ObjectTypeEnum type;
        [SerializeField] private SelectionMark selectionMark;
        [SerializeField] private SelectionMark preSelectionMark;

        [Space]
        [Header("Selectable Info")]
        [SerializeField] private Sprite picture;
        [Space]
        [SerializeField] ObjectStatus life;
        [SerializeField] ObjectStatus mana;
        [Space]

        [SerializeField] private string typeStr;  //TODO should be an Enum?
        [SerializeField] private int selectionOrder;





        private bool isSelected = false;
        private bool isPreSelected = false;
        private SignalBus _signalBus;


        public int Index { get; set; }
        public ObjectTypeEnum Type { get => type; set => type = value; }
        public SelectionMark SelectionMark { get => selectionMark; set => selectionMark = value; }
        public SelectionMark PreSelectionMark { get => preSelectionMark; set => preSelectionMark = value; }
        public SignalBus SignalBus { get => _signalBus; set => _signalBus = value; }
        public Sprite Picture { get => picture; set => picture = value; }
        public string TypeStr { get => typeStr; set => typeStr = value; }
        public int SelectionOrder { get => selectionOrder; set => selectionOrder = value; }

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

        public ObjectStatus Life { get => life; set => life = value; }
        public ObjectStatus Mana { get => mana; set => mana = value; }

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
                return second.Type == this.Type && second.TypeStr.Equals(this.TypeStr);
            }
            return false;
        }
    }

    [Serializable]
    public class ObjectStatus
    {
        [Space]
        [SerializeField] private int value = 500;
        [SerializeField] private int maxValue = 500;
        [SerializeField] private bool enabled = true;

        public int Value
        {
            get => value; set
            {
                if (value > maxValue)
                {
                    this.value = maxValue;
                }
                else
                {
                    this.value = value;
                }
            }
        }
        public int MaxValue
        {
            get
            {
                if (maxValue <= 0) return 1; else return maxValue;
            }
            set => maxValue = value;
        }
        public bool Enabled { get => enabled; set => enabled = value; }
    }



}
