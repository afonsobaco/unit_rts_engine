using UnityEngine;
using Zenject;
using RTSEngine.Core.Impls;
using RTSEngine.Core.Enums;
using RTSEngine.Manager.Interfaces;
using RTSEngine.Manager.Enums;

namespace RTSEngine.Manager.Impls
{
    public class SelectionManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private SelectableObjectRuntimeSetSO selectableList;
        [SerializeField] private SelectionSettingsSO settings;

        [Inject]
        public ISelectionManager<SelectableObject, SelectionTypeEnum, ObjectTypeEnum> Manager { get; private set; }

        private void Awake()
        {
            Manager.SelectableList = selectableList;
            Manager.Settings = settings;
        }
    }
}
