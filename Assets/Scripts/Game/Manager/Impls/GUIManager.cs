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
        private ISelectableObject _highlighted;
        private ISelectableObject[] _selection;

        public void OnSelectionChange(SelectionChangeSignal signal)
        {
            this._selection = signal.Selection;
            this.UpdateSelection();
        }

        private void UpdateSelection()
        {
            List<ISelectableObject> orderedSelection = GetOrderedSelection();
            _highlighted = null;
            SetMainFromHighlightedGroup(orderedSelection, false);
            UpdateMiniatureGrid(orderedSelection);
            UpdateHighlightedGroupMiniatures();
        }

        public void ChangeGroup(bool back)
        {
            List<ISelectableObject> orderedSelection = GetOrderedSelection();
            SetMainFromHighlightedGroup(orderedSelection, back);
            UpdateHighlightedGroupMiniatures();
        }

        private void UpdateHighlightedGroupMiniatures()
        {
            var gridList = _selectionGridPlaceholder.GetComponentsInChildren<GUISelectedMiniatureBehaviour>();
            foreach (var item in gridList)
            {
                item.SelectionBorder.enabled = item.Selected.IsCompatible(_highlighted);
            }
            UpdatePortrait();
        }

        private void SetMainFromHighlightedGroup(List<ISelectableObject> orderedSelection, bool back)
        {
            if (orderedSelection.Count > 0)
            {
                if (back)
                {
                    _highlighted = orderedSelection.ElementAt(GetPreviousGroup(orderedSelection));
                }
                else
                {
                    _highlighted = orderedSelection.ElementAt(GetNextGroup(orderedSelection));
                }
            }
            else
            {
                _highlighted = null;
            }
        }

        private int GetPreviousGroup(List<ISelectableObject> orderedSelection)
        {
            if (_highlighted != null)
            {
                var index = orderedSelection.IndexOf(_highlighted);
                if (index != 0)
                {
                    return orderedSelection.IndexOf(orderedSelection.ElementAt(index - 1));
                }
            }
            return orderedSelection.Count - 1;
        }

        private int GetNextGroup(List<ISelectableObject> orderedSelection)
        {
            if (_highlighted != null)
            {
                var index = orderedSelection.LastIndexOf(_highlighted);
                if (index < orderedSelection.Count - 2)
                {
                    return index + 1;
                }
            }
            return 0;
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

        private void UpdatePortrait()
        {
            if (_highlighted != null)
            {
                _portraitPlaceholder.gameObject.SetActive(true);
                var selectedGUIPortrait = _portraitPlaceholder.gameObject.GetComponentInChildren<GUISelectedPortraitBehaviour>();
                if (!selectedGUIPortrait)
                {
                    selectedGUIPortrait = CreatePortrait();
                }
                AdjustSelected(_highlighted, selectedGUIPortrait);
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

        private void AdjustSelected(ISelectableObject selected, AbstractGUISelectedInfoBehaviour selectedGUI)
        {
            selectedGUI.Selected = selected;
            var selectableObjectInfo = selected.SelectableObjectInfo;
            selectedGUI.Picture.sprite = selectableObjectInfo.Picture;
            selectedGUI.LifeBar.gameObject.SetActive(selectableObjectInfo.Life.Enabled);
            selectedGUI.ManaBar.gameObject.SetActive(selectableObjectInfo.Mana.Enabled);
            UpdateMiniatureStatusBar(selectedGUI.ManaBar, selectableObjectInfo.Mana);
            UpdateMiniatureStatusBar(selectedGUI.LifeBar, selectableObjectInfo.Life);
        }

        private void UpdateMiniatureStatusBar(GUIStatusBarBehaviour statusBar, ObjectStatus objectStatus)
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
            var grouped = _selection.GroupBy(x => x, new EqualityComparer());
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