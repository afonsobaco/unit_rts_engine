using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public abstract class BaseSelectionModifier : MonoBehaviour, ISelectionModifier
    {
        [SerializeField] private SelectionType type;
        public SelectionType Type { get => type; set => type = value; }
        public abstract ISelectable[] Apply(ISelectable[] oldSelection, ISelectable[] newSelection, ISelectable[] actualSelection);

        private void Start()
        {
            StartVariables();
        }

        private void OnValidate()
        {
            StartVariables();
        }

        public virtual void StartVariables() { }
    }
}
