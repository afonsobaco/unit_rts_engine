using RTSEngine.Core;
using RTSEngine.Utils;
using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace RTSEngine.Manager
{

    public class SelectableObjectBehaviour : MonoBehaviour, ISelectableObject //, IDestructable
    {
        [Space]
        [Header("Prefab Info")]
        [SerializeField] private SelectionMark _selectionMark;
        [SerializeField] private SelectionMark _preSelectionMark;
        [SerializeField] private GUISelectableObjectInfoSO _selectableObjectInfo;
        [SerializeField] private bool _isStatic;
        [SerializeField] private bool _isImortal;

        private ObjectStatus lifeStatus = new ObjectStatus();
        private ObjectStatus manaStatus = new ObjectStatus();
        private bool isSelected = false;
        private bool isPreSelected = false;
        private SignalBus _signalBus;
        private bool _isDestroyed;

        public int Index { get; set; }
        public SelectionMark SelectionMark { get => _selectionMark; set => _selectionMark = value; }
        public SelectionMark PreSelectionMark { get => _preSelectionMark; set => _preSelectionMark = value; }


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

        public GUISelectableObjectInfoSO SelectableObjectInfo { get => _selectableObjectInfo; set => _selectableObjectInfo = value; }
        public ObjectStatus LifeStatus { get => lifeStatus; set => lifeStatus = value; }
        public ObjectStatus ManaStatus { get => manaStatus; set => manaStatus = value; }
        public bool IsDestroyed { get => _isDestroyed; set => _isDestroyed = value; }
        public bool IsHighlighted { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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

        private void Start()
        {
            UpdateVariables();
        }

        private void Update()
        {
            if (!this._isStatic && this.lifeStatus.CurrentValue <= 0)
            {
                DoDestroy();
            }
        }

        private void OnValidate()
        {
            UpdateVariables();
        }

        private void UpdateVariables()
        {
            lifeStatus.MaxValue = _selectableObjectInfo.Life;
            manaStatus.MaxValue = _selectableObjectInfo.Mana;
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
            //TODO should verify other things eg.: [Human, Wizzard, Level2, etc...]
            if (other != null && other is SelectableObjectBehaviour)
            {
                var second = other as SelectableObjectBehaviour;
                return second.SelectableObjectInfo.Type == this.SelectableObjectInfo.Type && second.SelectableObjectInfo.ObjectName.Equals(this.SelectableObjectInfo.ObjectName);
            }
            return false;
        }

        public void DoDestroy()
        {
            if (_isImortal)
            {
                Debug.Log(string.Format("Imortal {0} at {1}. Do \"Dying\" Imortal Animation", this.Index, this.Position));
            }
            else
            {
                Debug.Log(string.Format("Destroyed {0} at {1}. Do \"Dying\" Animation. Wait to be Removed.", this.Index, this.Position));
                this.IsDestroyed = true;
                Destroy(this.gameObject);
            }
        }

        public int CompareTo(object obj)
        {
            throw new System.NotImplementedException();
        }
    }

}
