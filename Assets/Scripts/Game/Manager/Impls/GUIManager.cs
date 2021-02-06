using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

namespace RTSEngine.Manager
{
    public class GUIManager : IGUIManager
    {
        private Transform _selectionGridTransform;
        private Transform _profileInfoTransform;
        private SelectedInfoBehaviour[] miniatureList;
        private ISelectionManager<ISelectableObject, SelectionTypeEnum> selectionManager;
        private GraphicRaycaster raycaster;
        private EventSystem eventSystem;
        private PointerEventData pointerEventData;

        //TODO remover selection manager (create a GUI installer)
        public GUIManager(ISelectionManager<ISelectableObject, SelectionTypeEnum> selectionManager)
        {
            this.selectionManager = selectionManager;
        }

        public void OnSelectionChange()
        {
            this.UpdateSelection();
        }

        private void UpdateSelection()
        {
            List<ISelectableObject> selection = GetOrderedSelection();

            UpdateSelectionWithNew(selection);
        }

        private void UpdateSelectionWithNew(List<ISelectableObject> selection)
        {
            UpdateMiniatureList(selection);
            UpdateProfileInfo();

        }

        private void UpdateProfileInfo()
        {
            //TODO get From "tab" selection
            SelectedInfoBehaviour profileInfo = null;
            if (miniatureList.Length > 0)
            {
                var primary = miniatureList.First().Selected;
                profileInfo = this._profileInfoTransform.GetComponent<SelectedInfoBehaviour>();
                profileInfo.Selected = primary;
            }
            UpdateMiniature(profileInfo);
        }

        private void UpdateMiniatureList(List<ISelectableObject> selection)
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

        private void UpdateMiniature(SelectedInfoBehaviour miniature)
        {
            if (miniature.Selected != null)
            {
                miniature.gameObject.SetActive(true);
                ISelectableObjectInfo selectableObjectInfo = miniature.Selected.SelectableObjectInfo;
                miniature.Picture.sprite = selectableObjectInfo.Picture;
                miniature.LifeBar.gameObject.SetActive(selectableObjectInfo.Life.Enabled);
                miniature.ManaBar.gameObject.SetActive(selectableObjectInfo.Mana.Enabled);
                UpdateMiniatureStatusBar(miniature.ManaBar, selectableObjectInfo.Mana);
                UpdateMiniatureStatusBar(miniature.LifeBar, selectableObjectInfo.Life);
            }
            else
            {
                miniature.gameObject.SetActive(false);
            }
        }

        private void UpdateMiniatureStatusBar(StatusBarBehaviour statusBar, ObjectStatus objectStatus)
        {
            if (statusBar.enabled)
            {
                statusBar.StatusText.text = String.Format("{0}/{1}", objectStatus.Value, objectStatus.MaxValue);
                statusBar.StatusBar.fillAmount = (float)objectStatus.Value / (float)objectStatus.MaxValue;
            }
        }

        private List<ISelectableObject> GetOrderedSelection()
        {
            List<ISelectableObject> list = new List<ISelectableObject>();
            var grouped = selectionManager.GetCurrentSelection().GroupBy(x => x, new EqualityComparer());
            var sorted = grouped.ToList();
            sorted.Sort(new ObjectComparer());
            foreach (var item in sorted)
            {
                list.AddRange(item);
            }
            return new List<ISelectableObject>(list);
        }

        public bool ClickedOnGUI(Vector3 mousePosition)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(new PointerEventData(null) { position = mousePosition }, results);
            bool guiClicked = results.Count > 0;
            if (guiClicked)
            {
                DoClickOnElement(results);
            }
            return guiClicked;
        }

        private void DoClickOnElement(List<RaycastResult> results)
        {
            var found = results.Find(x =>
            {
                var a = x.gameObject.transform.parent.GetComponentInChildren<IGUIClickableElement>();
                return (a != null);
            });
            if (found.gameObject != null)
            {
                var clickable = found.gameObject.transform.parent.GetComponentInChildren<IGUIClickableElement>();
                clickable.DoAction();
            }
        }

        public void SetSelectionGrid(Transform selectionGridTransform)
        {
            this._selectionGridTransform = selectionGridTransform;
            this.miniatureList = this._selectionGridTransform.GetComponentsInChildren<SelectedInfoBehaviour>(true);
        }

        public void SetProfileInfo(Transform profileInfoTransform)
        {
            this._profileInfoTransform = profileInfoTransform;
        }

        public void SetRaycaster(GraphicRaycaster raycaster)
        {
            this.raycaster = raycaster;
        }

        private class ObjectComparer : IComparer<IGrouping<ISelectableObject, ISelectableObject>>
        {
            public int Compare(IGrouping<ISelectableObject, ISelectableObject> x, IGrouping<ISelectableObject, ISelectableObject> y)
            {
                int v = y.Key.SelectableObjectInfo.SelectionOrder - x.Key.SelectableObjectInfo.SelectionOrder;
                if (v == 0)
                {
                    if (y.Key.SelectableObjectInfo.Life.MaxValue > x.Key.SelectableObjectInfo.Life.MaxValue)
                    {
                        return 1;
                    }
                    else if (y.Key.SelectableObjectInfo.Life.MaxValue < x.Key.SelectableObjectInfo.Life.MaxValue)
                    {
                        return -1;
                    }
                }
                return v;
            }
        }

        private class EqualityComparer : IEqualityComparer<ISelectableObject>
        {
            public bool Equals(ISelectableObject x, ISelectableObject y)
            {
                return x.SelectableObjectInfo.Type == y.SelectableObjectInfo.Type && x.SelectableObjectInfo.TypeStr == y.SelectableObjectInfo.TypeStr;
            }

            public int GetHashCode(ISelectableObject obj)
            {
                int hCode = obj.SelectableObjectInfo.Type.GetHashCode() + obj.SelectableObjectInfo.TypeStr.GetHashCode();
                return hCode;
            }
        }
    }


}