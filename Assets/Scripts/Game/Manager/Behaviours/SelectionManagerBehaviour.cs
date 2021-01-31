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
        [SerializeField] private HashSet<ScriptableObject> modifiersList;

        private ISelectionManager<ISelectableObjectBehaviour, IBaseSelectionMod, SelectionTypeEnum> manager;

        [Inject]
        private void Construct(ISelectionManager<ISelectableObjectBehaviour, IBaseSelectionMod, SelectionTypeEnum> manager)
        {
            this.manager = manager;
            selectableList.GetList().Clear();
            this.manager.SetMainList(this.selectableList.GetList());
            this.manager.SetSelctionModifiers(GetScriptableObjectsAsMods());
        }

        private HashSet<IBaseSelectionMod> GetScriptableObjectsAsMods()
        {
            if (modifiersList != null)
            {
                IEnumerable<IBaseSelectionMod> list = modifiersList.ToList().FindAll(x => x is IBaseSelectionMod).Select(x => x as IBaseSelectionMod);
                return new HashSet<IBaseSelectionMod>(list);
            }
            else
            {
                return new HashSet<IBaseSelectionMod>();
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