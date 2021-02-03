using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RTSEngine.Manager
{
    public class GUIManagerBehaviour : MonoBehaviour, IGUIManager
    {
        [SerializeField] private SelectionGridBehaviour selectionGrid;
        [SerializeField] private ProfileInfoBehaviour profileInfo;
        private SelectedMiniatureBehaviour[] miniatureList;
        private ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> manager;

        [Inject]
        public void Construct(ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> manager)
        {
            this.manager = manager;
            miniatureList = selectionGrid.transform.GetComponentsInChildren<SelectedMiniatureBehaviour>(true);
        }

        public void OnSelectionChange()
        {
            this.UpdateSelection();
        }

        private void UpdateSelection()
        {
            HashSet<ISelectableObjectBehaviour> selection = GetOrderedSelection();

            UpdateSelectionWithNew(selection);
        }

        private void UpdateSelectionWithNew(HashSet<ISelectableObjectBehaviour> selection)
        {
            for (var i = 0; i < miniatureList.Length; i++)
            {
                if (i < selection.Count)
                {
                    miniatureList[i].Selected = selection.ElementAt(i);
                }
                else
                {
                    miniatureList[i].Selected = null;
                }
                miniatureList[i].gameObject.SetActive(miniatureList[i].Selected != null);
            }
        }

        private HashSet<ISelectableObjectBehaviour> GetOrderedSelection()
        {
            HashSet<ISelectableObjectBehaviour> selectableObjectBehaviours = manager.GetCurrentSelection();
            foreach (var item in selectableObjectBehaviours)
            {
                //TODO Do Order 
            }
            return selectableObjectBehaviours;
        }
    }
}