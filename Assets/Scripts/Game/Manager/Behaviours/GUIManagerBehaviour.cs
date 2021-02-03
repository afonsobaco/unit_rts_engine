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
            List<ISelectableObjectBehaviour> selection = GetOrderedSelection();

            UpdateSelectionWithNew(selection);
        }

        private void UpdateSelectionWithNew(List<ISelectableObjectBehaviour> selection)
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
                UpdateMiniature(miniatureList[i]);
            }
        }

        private void UpdateMiniature(SelectedMiniatureBehaviour miniature)
        {
            if (miniature.Selected != null)
            {
                miniature.gameObject.SetActive(true);
                miniature.Picture.sprite = miniature.Selected.Picture;
                miniature.LifeBar.gameObject.SetActive(miniature.Selected.Life.Enabled);
                miniature.ManaBar.gameObject.SetActive(miniature.Selected.Mana.Enabled);
                miniature.LifeBar.StatusBar.fillAmount = (float)miniature.Selected.Life.Value / (float)miniature.Selected.Life.MaxValue;
                miniature.ManaBar.StatusBar.fillAmount = (float)miniature.Selected.Mana.Value / (float)miniature.Selected.Mana.MaxValue;
            }
            else
            {
                miniature.gameObject.SetActive(false);
            }
        }

        private List<ISelectableObjectBehaviour> GetOrderedSelection()
        {
            List<ISelectableObjectBehaviour> list = new List<ISelectableObjectBehaviour>();
            var grouped = manager.GetCurrentSelection().GroupBy(x => x.SelectionOrder);
            var sorted = grouped.ToList();
            sorted.Sort(new ObjectComparer());
            foreach (var item in sorted)
            {
                list.AddRange(item);
            }
            return new List<ISelectableObjectBehaviour>(list);
        }

        private class ObjectComparer : IComparer<IGrouping<int, ISelectableObjectBehaviour>>
        {
            public int Compare(IGrouping<int, ISelectableObjectBehaviour> x, IGrouping<int, ISelectableObjectBehaviour> y)
            {
                int v = y.Key - x.Key;
                if (v == 0)
                {
                    if (y.First().Life.MaxValue > x.First().Life.MaxValue)
                    {
                        return 1;
                    }
                    else if (y.First().Life.MaxValue < x.First().Life.MaxValue)
                    {
                        return -1;
                    }
                }
                return v;
            }
        }
    }


}