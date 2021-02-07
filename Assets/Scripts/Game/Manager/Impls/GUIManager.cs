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
        private Transform _selectionGridPlaceholder;
        private Transform _portraitPlaceholder;
        private GameObject _selectedMiniaturePrefab;
        private GameObject _selectedPortraitPrefab;
        private GraphicRaycaster raycaster;
        private EventSystem eventSystem;
        private PointerEventData pointerEventData;

        public void OnSelectionChange(SelectionChangeSignal signal)
        {
            this.UpdateSelection(signal.Selection);
        }

        private void UpdateSelection(ISelectableObject[] selection)
        {
            List<ISelectableObject> orderedSelection = GetOrderedSelection(selection);
            UpdateMiniatureGrid(orderedSelection);
            UpdatePortrait(orderedSelection);
        }

        private void UpdatePortrait(List<ISelectableObject> selection)
        {
            //TODO get from TAB
            ISelectableObject selected = null;
            if (selection.Count > 0)
            {
                selected = selection.First();
            }
            GetPortrait(selected);
        }

        private void UpdateMiniatureGrid(List<ISelectableObject> selection)
        {
            ClearGrid();
            if (selection.Count > 1)
            {
                _selectionGridPlaceholder.gameObject.SetActive(true);
                for (var i = 0; i < selection.Count; i++)
                {
                    AddToGrid(selection.ElementAt(i));
                }
            }
            else
            {
                _selectionGridPlaceholder.gameObject.SetActive(false);
            }

        }

        private void GetPortrait(ISelectableObject selected)
        {
            if (selected != null)
            {
                _portraitPlaceholder.gameObject.SetActive(true);
                var selectedGUIPortrait = _portraitPlaceholder.gameObject.GetComponentInChildren<GUISelectedPortraitBehaviour>();
                if (!selectedGUIPortrait)
                {
                    selectedGUIPortrait = CreatePortrait();
                }
                AdjustSelected(selected, selectedGUIPortrait);
            }
            else
            {
                _portraitPlaceholder.gameObject.SetActive(false);
            }
        }

        private GUISelectedPortraitBehaviour CreatePortrait()
        {
            GameObject miniature = GameObject.Instantiate(_selectedPortraitPrefab);
            miniature.transform.SetParent(_portraitPlaceholder, false);
            var selectedGUIPortrait = miniature.GetComponent<GUISelectedPortraitBehaviour>();
            selectedGUIPortrait.gameObject.SetActive(true);
            return selectedGUIPortrait;
        }

        private GUISelectedMiniatureBehaviour CreateMiniature()
        {
            GameObject miniature = GameObject.Instantiate(_selectedMiniaturePrefab);
            miniature.transform.SetParent(_selectionGridPlaceholder, false);
            var selectedGUIMiniature = miniature.GetComponent<GUISelectedMiniatureBehaviour>();
            selectedGUIMiniature.gameObject.SetActive(true);
            return selectedGUIMiniature;
        }

        private void ClearGrid()
        {
            foreach (Transform child in _selectionGridPlaceholder)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        private void AddToGrid(ISelectableObject selected)
        {
            GUISelectedMiniatureBehaviour selectedGUIMiniature = CreateMiniature();
            AdjustSelected(selected, selectedGUIMiniature);
        }

        private void AdjustSelected(ISelectableObject selected, AbstractGUISelectedInfoBehaviour selectedGUIMiniature)
        {
            selectedGUIMiniature.Selected = selected;
            var selectableObjectInfo = selected.SelectableObjectInfo;
            selectedGUIMiniature.Picture.sprite = selectableObjectInfo.Picture;
            selectedGUIMiniature.LifeBar.gameObject.SetActive(selectableObjectInfo.Life.Enabled);
            selectedGUIMiniature.ManaBar.gameObject.SetActive(selectableObjectInfo.Mana.Enabled);
            UpdateMiniatureStatusBar(selectedGUIMiniature.ManaBar, selectableObjectInfo.Mana);
            UpdateMiniatureStatusBar(selectedGUIMiniature.LifeBar, selectableObjectInfo.Life);
        }

        private void UpdateMiniatureStatusBar(GUIStatusBarBehaviour statusBar, ObjectStatus objectStatus)
        {
            if (statusBar.enabled)
            {
                statusBar.StatusText.text = String.Format("{0}/{1}", objectStatus.Value, objectStatus.MaxValue);
                statusBar.StatusBar.fillAmount = (float)objectStatus.Value / (float)objectStatus.MaxValue;
            }
        }

        private List<ISelectableObject> GetOrderedSelection(ISelectableObject[] selection)
        {
            List<ISelectableObject> list = new List<ISelectableObject>();
            var grouped = selection.GroupBy(x => x, new EqualityComparer());
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

        public void SetSelectionGridPlaceholder(Transform selectionGridTransform)
        {
            this._selectionGridPlaceholder = selectionGridTransform;
        }

        public void SetPortraitPlaceholder(Transform profileInfoTransform)
        {
            this._portraitPlaceholder = profileInfoTransform;
        }

        public void SetRaycaster(GraphicRaycaster raycaster)
        {
            this.raycaster = raycaster;
        }

        public void SetSelectedMiniaturePrefab(GameObject selectedMiniaturePrefab)
        {
            this._selectedMiniaturePrefab = selectedMiniaturePrefab;
        }

        public void SetSelectedPortraitPrefab(GameObject selectedPortraitPrefab)
        {
            this._selectedPortraitPrefab = selectedPortraitPrefab;
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