using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System.Linq;
using System;

namespace RTSEngine.Manager
{
    public class SelectionManager : ISelectionManager, IInitializable
    {
        private ISelectionSettings _settings;

        private Dictionary<int, HashSet<ISelectableObject>> _groupSet = new Dictionary<int, HashSet<ISelectableObject>>();
        private HashSet<ISelectableObject> _mainList = new HashSet<ISelectableObject>();
        private HashSet<ISelectableObject> _currentSelection = new HashSet<ISelectableObject>();
        private HashSet<ISelectableObject> _preSelection = new HashSet<ISelectableObject>();
        private ISelectableObject _lastClicked;
        private ISelectableObject _clicked;

        private int _groupNumberPressed = 0;
        private bool _isAditiveSelection;
        private bool _isDoubleClick;
        private bool _isSameTypeSelection;
        private bool _isSelecting;
        private bool _isPreSelection;
        private bool _canSelect = true;
        private Vector2 _finalScreenPosition;
        private Vector2 _initialScreenPosition;
        private SelectionTypeEnum _selectionType;
        private List<ISelectionModifier> _mods;
        private SignalBus _signalBus;
        private ISelectableObject _selectedMiniature;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            this._signalBus = signalBus;
        }

        public void Initialize()
        {
            _mods = new List<ISelectionModifier>()
            {
                new SameTypeSelectionModifier(this),
                new OrderOfSelectionModifier(this),
                new AdditiveSelectionModifier(this),
                new GroupRestrictorSelectionModifier(this),
                new LimitSelectionModifier(this),
                new GroupSelectionModifier(this)
            };
        }

        public ISelectionSettings GetSettings()
        {
            return _settings;
        }

        public void SetSettings(ISelectionSettings value)
        {
            _settings = value;
        }

        public void SetMainList(HashSet<ISelectableObject> list)
        {
            this._mainList = list;
        }

        public void SetSelectionModifiers(List<ISelectionModifier> list)
        {
            this._mods = list;
        }

        public void SetKeysPressed(bool additiveKeyPressed, bool sameTypeKeyPressed)
        {
            this._isAditiveSelection = additiveKeyPressed;
            this._isSameTypeSelection = sameTypeKeyPressed;
        }

        public void SetGroupNumperPressed(int groupNumber)
        {
            this._groupNumberPressed = groupNumber;
        }

        public bool CanSelect()
        {
            return this._canSelect;
        }

        public void SetDoubleClick(bool doubleClick)
        {
            this._isDoubleClick = doubleClick;
        }

        public void SetCurrentSelection(HashSet<ISelectableObject> selection)
        {
            this._currentSelection = selection;
        }

        public void SetClicked(ISelectableObject selected)
        {
            this._clicked = selected;
        }

        public void SetLastClicked(ISelectableObject selected)
        {
            this._lastClicked = selected;
        }

        public void SetIsPreSelection(bool value)
        {
            this._isPreSelection = value;
        }

        public void SetPreSelection(HashSet<ISelectableObject> preSelection)
        {
            this._preSelection = preSelection;
        }

        public void SetSelectionType(SelectionTypeEnum type)
        {
            this._selectionType = type;
        }

        public bool IsSelecting()
        {
            return this._isSelecting;
        }

        public bool IsAdditive()
        {
            return this._isAditiveSelection;
        }
        public bool IsSameType()
        {
            return this._isSameTypeSelection;
        }

        public Vector2 GetInitialScreenPosition()
        {
            Vector2 initialSelectionPos = this._isSameTypeSelection && !this._isPreSelection ? (Vector2)Camera.main.ViewportToScreenPoint(this.GetSettings().MinViewport) : this._initialScreenPosition;
            return initialSelectionPos;
        }

        public Vector2 GetFinalScreenPosition()
        {
            Vector2 finalSelectionPos = this._isSameTypeSelection && !this._isPreSelection ? (Vector2)Camera.main.ViewportToScreenPoint(this.GetSettings().MaxViewport) : this._finalScreenPosition;
            return finalSelectionPos;

        }

        public HashSet<ISelectableObject> GetCurrentSelection()
        {
            return this._currentSelection;
        }

        public int GetGroupSetNumberPressed()
        {
            return this._groupNumberPressed;
        }

        public IEnumerable<ISelectableObject> GetPreSelection()
        {
            return this._preSelection;
        }

        public void CreateGroupSet(int number)
        {
            this._groupSet[number] = this._currentSelection;
            //TODO call signal
        }

        public void AddSelectableObject(SelectableObjectCreatedSignal signal)
        {
            signal.Selectable.Index = this._mainList.Count;
            this._mainList.Add(signal.Selectable);
        }

        public void RemoveSelectableObject(SelectableObjectDeletedSignal signal)
        {
            this._mainList.Remove(signal.Selectable);
        }

        public void Dispose()
        {
            this._mainList.Clear();
        }

        public virtual ISelectableObject GetObjectClicked()
        {
            return SelectionUtil.GetObjectClicked(this._initialScreenPosition, this._finalScreenPosition);
        }

        public virtual Dictionary<int, HashSet<ISelectableObject>> GetAllGroupSets()
        {
            return this._groupSet;
        }

        public HashSet<ISelectableObject> OrderSelection(HashSet<ISelectableObject> newSelection)
        {
            var orderedListOfSelection = new HashSet<ISelectableObject>(this._preSelection);
            orderedListOfSelection.RemoveWhere(x => !newSelection.Contains(x));
            orderedListOfSelection.UnionWith(newSelection);
            return orderedListOfSelection;
        }

        public virtual HashSet<ISelectableObject> GetDragSelection()
        {
            var selectionOnScreen = SelectionUtil.GetAllObjectsInsideSelectionArea(this._mainList, this._initialScreenPosition, this._finalScreenPosition);
            return OrderSelection(selectionOnScreen);
        }

        public virtual List<ISelectionModifier> GetModifiersToBeApplied(SelectionTypeEnum type)
        {
            return this._mods.FindAll(x => x.Type.Equals(type) || x.Type.Equals(SelectionTypeEnum.ANY));
        }

        public virtual HashSet<ISelectableObject> GetGroupSet(int number)
        {
            HashSet<ISelectableObject> list;
            GetAllGroupSets().TryGetValue(number, out list);
            if (list == null)
            {
                list = new HashSet<ISelectableObject>();
            }
            return list;
        }

        //TODO clear selectiontype after selection
        public virtual SelectionTypeEnum GetSelectionType()
        {
            this._selectionType = SelectionTypeEnum.DRAG;
            if (this._groupNumberPressed > 0)
            {
                this._selectionType = SelectionTypeEnum.KEY;
            }
            else
            {
                this._clicked = GetObjectClicked();
                if (this._clicked != null)
                {
                    this._selectionType = SelectionTypeEnum.CLICK;
                }
            }
            return this._selectionType;
        }

        public virtual HashSet<ISelectableObject> GetSelectionBySelectionType()
        {
            HashSet<ISelectableObject> list = new HashSet<ISelectableObject>();
            switch (this._selectionType)
            {
                case SelectionTypeEnum.CLICK:
                    list.Add(this._clicked);
                    break;
                case SelectionTypeEnum.KEY:
                    list.UnionWith(GetGroupSet(this._groupNumberPressed));
                    break;
                case SelectionTypeEnum.DRAG:
                    list.UnionWith(GetDragSelection());
                    break;
                default:
                    break;
            }
            return list;
        }

        private void AdjustSameTypeIfDoubleClick(HashSet<ISelectableObject> newSelection)
        {
            if (this._clicked != null && this._clicked == _lastClicked && !this._isSameTypeSelection)
            {
                if (newSelection.Count == 0) newSelection.Add(this._clicked);
                if (this._currentSelection.Contains(this._clicked)) this._currentSelection.Remove(this._clicked);
                else this._currentSelection.Add(this._clicked);
                this._isSameTypeSelection = true;
            }
        }

        public SelectionArguments GetSelectionArgs(HashSet<ISelectableObject> newSelection)
        {
            if (this._isDoubleClick)
            {
                AdjustSameTypeIfDoubleClick(newSelection);
            }
            return new SelectionArguments(this._currentSelection, newSelection, this._mainList);
        }

        public SelectionArguments ApplyModifiers(SelectionArguments args)
        {
            var collection = GetModifiersToBeApplied(this._selectionType);
            foreach (var item in collection)
            {
                if (this._isPreSelection)
                {
                    if (item is IPreSelectionModifier && ((IPreSelectionModifier)item).ActiveOnPreSelection)
                    {
                        args = item.Apply(args);
                    }
                }
                else
                {
                    args = item.Apply(args);
                }
            }
            return args;
        }

        private static void UpdatePreSelectionStatus(HashSet<ISelectableObject> list, bool status)
        {
            foreach (var item in list)
            {
                item.IsPreSelected = status;
            }
        }

        private static void UpdateSelectionStatus(HashSet<ISelectableObject> list, bool status)
        {
            foreach (var item in list)
            {
                item.IsSelected = status;
            }
        }

        public HashSet<ISelectableObject> GetFinalSelection(SelectionArguments args)
        {
            if (this._isPreSelection)
            {
                UpdatePreSelectionStatus(args.ToBeAdded, true);
            }
            else
            {
                UpdateSelectionStatus(args.OldSelection, false);
                UpdateSelectionStatus(args.ToBeAdded, true);
            }
            return new HashSet<ISelectableObject>(args.ToBeAdded);
        }

        public HashSet<ISelectableObject> GetSelection(HashSet<ISelectableObject> newSelection)
        {
            var args = GetSelectionArgs(newSelection);
            args = ApplyModifiers(args);
            return GetFinalSelection(args);
        }

        public HashSet<ISelectableObject> GetUpdatedPreSelection(HashSet<ISelectableObject> newSelection)
        {
            var list = new HashSet<ISelectableObject>();
            if (this._preSelection != null && this._preSelection.Count > 0)
            {
                UpdatePreSelectionStatus(this._preSelection, false);
            }
            if (newSelection != null)
            {
                UpdatePreSelectionStatus(newSelection, true);
                list = newSelection;
            }

            return list;
        }

        public virtual HashSet<ISelectableObject> GetUpdatedCurrentSelection(HashSet<ISelectableObject> newSelection)
        {
            var list = new HashSet<ISelectableObject>();
            if (this._currentSelection != null && this._currentSelection.Count > 0)
            {
                UpdateSelectionStatus(this._currentSelection, false);
            }
            if (newSelection != null)
            {
                UpdateSelectionStatus(newSelection, true);
                list = newSelection;
            }
            return list;
        }

        private void RestartVariables()
        {
            if (this._groupNumberPressed <= 0) this._isSelecting = false;
            this._signalBus.Fire(new CanMoveSignal() { Value = this._isSelecting });
            if (this._clicked != null) _lastClicked = this._clicked;
            if (this._isDoubleClick) _lastClicked = null;
            this._isDoubleClick = false;
            this._groupNumberPressed = 0;
            this._preSelection = new HashSet<ISelectableObject>();
        }

        public virtual HashSet<ISelectableObject> PerformSelection(Vector2 finalPos)
        {
            this._finalScreenPosition = finalPos;
            this._selectionType = GetSelectionType();
            var list = GetSelection(GetSelectionBySelectionType());
            return list;
        }

        public void DoPreSelection(Vector2 finalPos)
        {
            this._isPreSelection = true;
            HashSet<ISelectableObject> list = PerformSelection(finalPos);
            this._preSelection = GetUpdatedPreSelection(list);
        }

        public void DoSelection(Vector2 finalPos)
        {
            this._isPreSelection = false;
            UpdatePreSelectionStatus(this._preSelection, false);
            FinalizeSelection(PerformSelection(finalPos));
        }

        private void FireSignals()
        {
            _signalBus.Fire(new SelectionChangeSignal() { Selection = this._currentSelection.ToArray(), Additive = IsAdditive() });
            _signalBus.Fire(new PrimaryObjectSelectedSignal() { Selectable = this._currentSelection.Count > 0 ? this._currentSelection.First() : null });
        }

        public void StartOfSelection(Vector2 initialPos)
        {
            this._initialScreenPosition = initialPos;
            this._isSelecting = true;
            this._signalBus.Fire(new CanMoveSignal() { Value = this._isSelecting });
        }

        public void SetCanSelect(GUIClickedSignal signal)
        {
            this._canSelect = !signal.Value;
        }


        public void DoSelectedMiniatureClick(SelectedMiniatureClickSignal signal)
        {
            if (signal.Type == KeyButtonType.UP && this._selectedMiniature != null && this._selectedMiniature.Equals(signal.Selectable))
            {
                this._isPreSelection = false;
                this._selectionType = SelectionTypeEnum.CLICK;
                FinalizeSelection(GetSelection(new HashSet<ISelectableObject>() { this._selectedMiniature }));
            }
            if (signal.Type == KeyButtonType.DOWN)
            {
                this._selectedMiniature = signal.Selectable;
            }
        }

        private void FinalizeSelection(HashSet<ISelectableObject> selected)
        {
            this._currentSelection = GetUpdatedCurrentSelection(selected);
            FireSignals();
            RestartVariables();
        }
    }

}