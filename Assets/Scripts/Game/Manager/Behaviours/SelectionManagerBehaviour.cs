using UnityEngine;
using Zenject;
using RTSEngine.Core.Impls;
using RTSEngine.Core.Enums;
using RTSEngine.Manager.Interfaces;
using RTSEngine.Manager.Enums;
using RTSEngine.Manager.Utils;

namespace RTSEngine.Manager.Impls
{
    public class SelectionManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private SelectableObjectRuntimeSetSO selectableList;
        [SerializeField] private SelectionSettingsSO settings;
        [SerializeField] private RectTransform selectionBox;
        private ISelectionManager<SelectableObject, SelectionTypeEnum, ObjectTypeEnum> manager;

        public ISelectionManager<SelectableObject, SelectionTypeEnum, ObjectTypeEnum> Manager { get => manager; private set => manager = value; }

        [Inject]
        private void Construct(ISelectionManager<SelectableObject, SelectionTypeEnum, ObjectTypeEnum> manager)
        {
            Manager = manager;
            Manager.SelectableList = selectableList;
            Manager.Settings = settings;
        }

        private void Update()
        {
            ActivateSelectionBox();
        }

        private void ActivateSelectionBox()
        {
            if (!selectionBox)
            {
                return;
            }
            if (Manager.IsSelecting)
            {
                if (!selectionBox.gameObject.activeInHierarchy)
                {
                    selectionBox.gameObject.SetActive(true);
                }
                DrawSelectionBox();
            }
            else
            {

                if (selectionBox.gameObject.activeInHierarchy)
                {
                    selectionBox.gameObject.SetActive(false);
                }
            }

        }

        private void DrawSelectionBox()
        {
            selectionBox.position = SelectionUtil.GetAreaCenter(Manager.InitialScreenPosition, Manager.FinalScreenPosition);
            selectionBox.sizeDelta = SelectionUtil.GetAreaSize(Manager.InitialScreenPosition, Manager.FinalScreenPosition);
        }


    }
}