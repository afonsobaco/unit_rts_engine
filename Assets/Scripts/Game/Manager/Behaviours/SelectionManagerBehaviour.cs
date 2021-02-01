using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using RTSEngine.Core;

namespace RTSEngine.Manager
{
    public class SelectionManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private SelectableObjectRuntimeSetSO selectableList;
        [SerializeField] private RectTransform selectionBox;

        private ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> manager;

        [Inject]
        private void Construct(ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> manager)
        {
            this.manager = manager;
            selectableList.GetList().Clear();
            this.manager.SetMainList(this.selectableList.GetList());
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
            if (manager.IsSelecting())
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
            selectionBox.position = SelectionUtil.GetAreaCenter(manager.GetInitialScreenPosition(), manager.GetFinalScreenPosition());
            selectionBox.sizeDelta = SelectionUtil.GetAreaSize(manager.GetInitialScreenPosition(), manager.GetFinalScreenPosition());
        }


    }
}