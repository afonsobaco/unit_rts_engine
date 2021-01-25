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
        [SerializeField] private List<ScriptableObject> modifiersList;

        private ISelectionManager<ISelectable, SelectionTypeEnum> manager;

        [Inject]
        private void Construct(ISelectionManager<ISelectable, SelectionTypeEnum> manager)
        {
            this.manager = manager;
            selectableList.GetList().Clear();
            this.manager.SelectableList = this.selectableList;
            this.manager.Mods = GetScriptableObjectsAsMods();
        }

        private List<IBaseSelectionMod> GetScriptableObjectsAsMods()
        {
            if (modifiersList != null)
            {
                return modifiersList.FindAll(x => x is IBaseSelectionMod).Select(x => x as IBaseSelectionMod).ToList();
            }
            else
            {
                return new List<IBaseSelectionMod>();
            }
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
            if (manager.IsSelecting)
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
            selectionBox.position = SelectionUtil.GetAreaCenter(manager.InitialScreenPosition, manager.FinalScreenPosition);
            selectionBox.sizeDelta = SelectionUtil.GetAreaSize(manager.InitialScreenPosition, manager.FinalScreenPosition);
        }


    }
}