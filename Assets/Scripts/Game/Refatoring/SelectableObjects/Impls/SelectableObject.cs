using UnityEngine;
using RTSEngine.Core;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class SelectableObject : ZenAutoInjecter, ISelectable
    {
        [SerializeField] private string _objectName;
        [SerializeField] private Status[] _statuses;

        public int Index { get; set; }
        public bool IsSelected { get; set; }
        public bool IsPreSelected { get; set; }
        public Vector3 Position { get; set; }
        public string ObjectName { get => _objectName; set => _objectName = value; }
        public Status[] Statuses { get => _statuses; set => _statuses = value; }

        private SignalBus _signalBus;

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
    }
}