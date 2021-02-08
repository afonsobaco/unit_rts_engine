using System.Net;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

//TODO Clean code this
namespace RTSEngine.Manager
{
    //TODO tests
    public class GUIManager : IGUIManager, ILateTickable
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
        private bool _additive;
        private bool _tabbed;
        private SignalBus _signalBus;
        private ObjectTypeEnum[] _canShowStatus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            this._signalBus = signalBus;
        }

        public void OnSelectionChange(SelectionChangeSignal signal)
        {
            this._selection = signal.Selection;
            this._additive = signal.Additive;
            this.UpdateSelection();
        }

        private void UpdateSelection()
        {
            var selectionAsList = new List<ISelectableObject>(_selection);
            _highlighted = UpdateActualHighlighted(selectionAsList);
            UpdateMiniatureGrid(selectionAsList);
            UpdateSelectionBoard();
        }

        private ISelectableObject UpdateActualHighlighted(List<ISelectableObject> selectionAsList)
        {
            ISelectableObject result = null;
            if (ShouldMaintainHighlight(selectionAsList))
            {
                var data = selectionAsList.FindAll(x => x.IsCompatible(_highlighted));
                if (data.Contains(_highlighted))
                {
                    result = _highlighted;
                }
                else if (data.Count > 0)
                {
                    result = data.First();
                }
            }
            else
            {
                this._tabbed = false;
            }
            if (result == null && selectionAsList.Count > 0)
            {
                result = selectionAsList.First();
            }
            return result;
        }

        private bool ShouldMaintainHighlight(List<ISelectableObject> selectionAsList)
        {
            if (selectionAsList.Count == 0 || !_additive || _highlighted == null || !this._tabbed)
            {
                return false;
            }
            var oldSelection = GetGridList().ToList().Select(x => x.Selected);
            HashSet<ISelectableObject> data = GetDifferenceOnSelections(oldSelection);
            bool highlightedWasRemoved = data.Contains(_highlighted);
            bool hasBeenAddedOrRemoved = data.Count > 0;
            bool areEqual = selectionAsList.SequenceEqual(oldSelection);
            return hasBeenAddedOrRemoved || areEqual || !highlightedWasRemoved;
        }

        private HashSet<ISelectableObject> GetDifferenceOnSelections(IEnumerable<ISelectableObject> oldSelection)
        {
            HashSet<ISelectableObject> data = new HashSet<ISelectableObject>(oldSelection);
            data.SymmetricExceptWith(_selection);
            if (data.Count == 0)
            {
                data = new HashSet<ISelectableObject>(_selection);
                data.SymmetricExceptWith(oldSelection);
            }
            return data;
        }

        public void ChangeGroup(bool back)
        {
            this._tabbed = true;
            var selectionAsList = new List<ISelectableObject>(_selection);
            _highlighted = GetPreviousNextHighlightedGroup(selectionAsList, back);
            UpdateSelectionBoard();
            _signalBus.Fire(new PrimaryObjectSelectedSignal() { Selectable = _highlighted });
        }

        private void UpdateSelectionBoard()
        {
            var gridList = GetGridList();
            foreach (var item in gridList)
            {
                UpdateItem(item);
            }
            UpdatePortrait();

        }

        private void UpdateItem(AbstractGUISelectedInfoBehaviour item)
        {
            SelectableObjectBehaviour selected = item.Selected as SelectableObjectBehaviour;
            if (selected == null || selected.IsDestroyed)
            {
                GameObject.Destroy(item.gameObject);
            }
            else
            {
                if (item is GUISelectedMiniatureBehaviour)
                {
                    ((GUISelectedMiniatureBehaviour)item).SelectionBorder.enabled = selected.IsCompatible(_highlighted);
                }
                item.Picture.sprite = selected.SelectableObjectInfo.Picture;
                if (!this._canShowStatus.ToList().Contains(item.Selected.SelectableObjectInfo.Type))
                {
                    selected.ManaStatus.Enabled = false;
                    selected.LifeStatus.Enabled = false;
                }
                UpdateMiniatureStatusBar(item.ManaBar, selected.ManaStatus);
                UpdateMiniatureStatusBar(item.LifeBar, selected.LifeStatus);
            }
        }

        private ISelectableObject GetPreviousNextHighlightedGroup(List<ISelectableObject> selectionAsList, bool back)
        {
            if (selectionAsList.Count > 0)
            {

                if (back)
                {
                    return selectionAsList.ElementAt(GetPreviousGroup(selectionAsList));
                }
                else
                {
                    return selectionAsList.ElementAt(GetNextGroup(selectionAsList));
                }
            }
            else
            {
                return null;
            }
        }

        private int GetPreviousGroup(List<ISelectableObject> selectionAsList)
        {
            if (_highlighted != null)
            {
                var index = GetIndexOfFirst(selectionAsList, _highlighted);
                if (index != 0)
                {
                    return selectionAsList.IndexOf(selectionAsList.ElementAt(index - 1));
                }
            }
            return selectionAsList.Count - 1;
        }

        private int GetNextGroup(List<ISelectableObject> selectionAsList)
        {
            if (_highlighted != null)
            {
                var index = GetIndexOfLast(selectionAsList, _highlighted);
                if (index < selectionAsList.Count - 1)
                {
                    return index + 1;
                }
            }
            return 0;
        }

        private int GetIndexOfFirst(List<ISelectableObject> selectionAsList, ISelectableObject selected)
        {
            var compatibles = selectionAsList.FindAll(x => { return x.IsCompatible(selected); });
            if (compatibles.Count > 0)
                return selectionAsList.IndexOf(compatibles.First());
            return 0;
        }

        private int GetIndexOfLast(List<ISelectableObject> selectionAsList, ISelectableObject selected)
        {
            var compatibles = selectionAsList.FindAll(x => { return x.IsCompatible(selected); });
            if (compatibles.Count > 0)
                return selectionAsList.IndexOf(compatibles.Last());
            return 0;
        }

        private void UpdateMiniatureGrid(List<ISelectableObject> selection)
        {
            ClearGrid();
            if (selection.Count > 0)
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
                selectedGUIPortrait.Selected = _highlighted;
                UpdateItem(selectedGUIPortrait);
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
            selectedGUIMiniature.Selected = selected;
        }

        private void UpdateMiniatureStatusBar(GUIStatusBarBehaviour statusBar, ObjectStatus objectStatus)
        {
            statusBar.gameObject.SetActive(objectStatus.Enabled);
            if (statusBar.enabled)
            {
                statusBar.StatusText.text = String.Format("{0}/{1}", objectStatus.CurrentValue, objectStatus.MaxValue);
                statusBar.StatusBar.fillAmount = (float)objectStatus.CurrentValue / (float)objectStatus.MaxValue;
            }
        }

        public List<RaycastResult> GetGUIElementsClicked(Vector3 mousePosition)
        {
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(new PointerEventData(null) { position = mousePosition }, results);
            return results;
        }

        public void DoClickOnElement(List<RaycastResult> results, KeyButtonType type)
        {
            var found = results.Find(x =>
            {
                var a = x.gameObject.transform.parent.GetComponent<IGUIClickableElement>();
                return (a != null);
            });
            if (found.gameObject != null)
            {
                var clickable = found.gameObject.transform.parent.GetComponent<IGUIClickableElement>();
                clickable.DoAction(type);
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

        public void LateTick()
        {
            UpdateSelectionBoard();
        }

        private GUISelectedMiniatureBehaviour[] GetGridList()
        {
            return _selectionGridPlaceholder.GetComponentsInChildren<GUISelectedMiniatureBehaviour>();
        }

        public void SetCanShowStatus(ObjectTypeEnum[] canShowStatus)
        {
            this._canShowStatus = canShowStatus;
        }
    }


}