using RTSEngine.Utils;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class SelectionManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private SelectableObjectRuntimeSetSO selectableList;
        [SerializeField] private RectTransform selectionBox;
        [SerializeField] private List<ScriptableObject> mods;
        private ISelectionManager<SelectableObject, SelectionTypeEnum> manager;

        public SelectableObjectRuntimeSetSO SelectableList { get => selectableList; set => selectableList = value; }
        public RectTransform SelectionBox { get => selectionBox; set => selectionBox = value; }
        public List<ScriptableObject> Mods { get => mods; set => mods = value; }
        public ISelectionManager<SelectableObject, SelectionTypeEnum> Manager { get => manager; set => manager = value; }

        [Inject]
        private void Construct(ISelectionManager<SelectableObject, SelectionTypeEnum> manager)
        {
            Manager = manager;
            SelectableList.GetList().Clear();
            Manager.SelectableList = SelectableList;
            Manager.ScriptableObjectMods = Mods;
        }

        private void Update()
        {
            ActivateSelectionBox();
        }

        private void ActivateSelectionBox()
        {
            if (!SelectionBox)
            {
                return;
            }
            if (Manager.IsSelecting)
            {
                if (!SelectionBox.gameObject.activeInHierarchy)
                {
                    SelectionBox.gameObject.SetActive(true);
                }
                DrawSelectionBox();
            }
            else
            {

                if (SelectionBox.gameObject.activeInHierarchy)
                {
                    SelectionBox.gameObject.SetActive(false);
                }
            }

        }

        private void DrawSelectionBox()
        {
            SelectionBox.position = SelectionUtil.GetAreaCenter(Manager.InitialScreenPosition, Manager.FinalScreenPosition);
            SelectionBox.sizeDelta = SelectionUtil.GetAreaSize(Manager.InitialScreenPosition, Manager.FinalScreenPosition);
        }


    }
}