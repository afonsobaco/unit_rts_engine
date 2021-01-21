using UnityEngine;
using Zenject;
using RTSEngine.Core.Impls;
using RTSEngine.Manager.Interfaces;

namespace RTSEngine.Manager.Impls
{
    public class SelectionManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private SelectableObjectRuntimeSetSO selectableList;

        [Inject]
        public ISelectionManager<SelectableObject> Manager { get; private set; }

        private void Awake()
        {
            Manager.SelectableList = selectableList;
        }
    }
}
