using RTSEngine.Core;
using RTSEngine.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RTSEngine.Manager
{
    public class SelectionManager : ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum>
    {

        private Dictionary<int, HashSet<ISelectableObjectBehaviour>> groupSet = new Dictionary<int, HashSet<ISelectableObjectBehaviour>>();
        private HashSet<ISelectableObjectBehaviour> mainList = new HashSet<ISelectableObjectBehaviour>();
        private HashSet<ISelectableObjectBehaviour> currentSelection = new HashSet<ISelectableObjectBehaviour>();
        private HashSet<ISelectableObjectBehaviour> preSelection = new HashSet<ISelectableObjectBehaviour>();
        private ISelectableObjectBehaviour lastClicked;
        private ISelectableObjectBehaviour clicked;

        private int groupNumberPressed = 0;
        private bool isAditiveSelection;
        private bool isDoubleClick;
        private bool isSameTypeSelection;
        private bool isSelecting;
        private bool isPreSelection;
        private Vector3 finalScreenPosition;
        private Vector3 initialScreenPosition;
        private Vector3 minScreenPos;
        private Vector3 maxScreenPos;
        private SelectionTypeEnum selectionType;
        private List<ISelectionModifier> mods;

        public SelectionManager()
        {
            mods = new List<ISelectionModifier>()
            {
                new SameTypeSelectionModifier(this),
                new OrderOfSelectionModifier(),
                new AdditiveSelectionModifier(this),
                new LimitSelectionModifier()
            };
        }

        public void SetMainList(HashSet<ISelectableObjectBehaviour> list)
        {
            this.mainList = list;
        }

        public void SetSelectionModifiers(List<ISelectionModifier> list)
        {
            this.mods = list;
        }

        public void SetScreenBoundries(Vector2 minScreenPoint, Vector2 maxScreenPoint)
        {
            this.minScreenPos = minScreenPoint;
            this.maxScreenPos = maxScreenPoint;
        }

        public void SetKeysPressed(bool additiveKeyPressed, bool sameTypeKeyPressed)
        {
            this.isAditiveSelection = additiveKeyPressed;
            this.isSameTypeSelection = sameTypeKeyPressed;
        }

        public void SetGroupNumperPressed(int groupNumber)
        {
            this.groupNumberPressed = groupNumber;
        }

        public void SetDoubleClick(bool doubleClick)
        {
            this.isDoubleClick = doubleClick;
        }

        public void SetCurrentSelection(HashSet<ISelectableObjectBehaviour> selection)
        {
            this.currentSelection = selection;
        }

        public void SetClicked(ISelectableObjectBehaviour selected)
        {
            this.clicked = selected;
        }

        public void SetLastClicked(ISelectableObjectBehaviour selected)
        {
            this.lastClicked = selected;
        }

        public void SetIsPreSelection(bool value)
        {
            this.isPreSelection = value;
        }

        public void SetPreSelection(HashSet<ISelectableObjectBehaviour> preSelection)
        {
            this.preSelection = preSelection;
        }

        public void SetSelectionType(SelectionTypeEnum type)
        {
            this.selectionType = type;
        }

        public bool IsSelecting()
        {
            return this.isSelecting;
        }

        public bool IsAdditive()
        {
            return this.isAditiveSelection;
        }
        public bool IsSameType()
        {
            return this.isSameTypeSelection;
        }

        public Vector2 GetInitialScreenPosition()
        {
            return this.initialScreenPosition;
        }

        public Vector2 GetFinalScreenPosition()
        {
            return this.finalScreenPosition;

        }

        public HashSet<ISelectableObjectBehaviour> GetCurrentSelection()
        {
            return this.currentSelection;
        }

        public int GetGroupSetNumberPressed()
        {
            return this.groupNumberPressed;
        }

        public IEnumerable<ISelectableObjectBehaviour> GetPreSelection()
        {
            return this.preSelection;
        }

        public void CreateGroupSet(int number)
        {
            this.groupSet[number] = this.currentSelection;
        }

        public void AddSelectableObject(SelectableObjectCreatedSignal signal)
        {
            signal.Selectable.Index = this.mainList.Count;
            this.mainList.Add(signal.Selectable);
        }

        public void RemoveSelectableObject(SelectableObjectDeletedSignal signal)
        {
            this.mainList.Remove(signal.Selectable);
        }

        public void Dispose()
        {
            this.mainList.Clear();
        }

        public virtual ISelectableObjectBehaviour GetObjectClicked()
        {
            return SelectionUtil.GetObjectClicked(this.initialScreenPosition, this.finalScreenPosition);
        }

        public virtual Dictionary<int, HashSet<ISelectableObjectBehaviour>> GetAllGroupSets()
        {
            return this.groupSet;
        }

        public HashSet<ISelectableObjectBehaviour> OrderSelection(HashSet<ISelectableObjectBehaviour> newSelection)
        {
            var orderedListOfSelection = new HashSet<ISelectableObjectBehaviour>(this.preSelection);
            orderedListOfSelection.RemoveWhere(x => !newSelection.Contains(x));
            orderedListOfSelection.UnionWith(newSelection);
            return orderedListOfSelection;
        }

        public virtual HashSet<ISelectableObjectBehaviour> GetDragSelection()
        {
            var selectionOnScreen = SelectionUtil.GetAllObjectsInsideSelectionArea(this.mainList, this.initialScreenPosition, this.finalScreenPosition);
            return OrderSelection(selectionOnScreen);
        }

        public virtual List<ISelectionModifier> GetModifiersToBeApplied(SelectionTypeEnum type)
        {
            return this.mods.FindAll(x => x.Type.Equals(type) || x.Type.Equals(SelectionTypeEnum.ANY));
        }

        public virtual HashSet<ISelectableObjectBehaviour> GetGroupSet(int number)
        {
            HashSet<ISelectableObjectBehaviour> list;
            GetAllGroupSets().TryGetValue(number, out list);
            if (list == null)
            {
                list = new HashSet<ISelectableObjectBehaviour>();
            }
            return list;
        }

        //TODO clear selectiontype after selection
        public virtual SelectionTypeEnum GetSelectionType()
        {
            this.selectionType = SelectionTypeEnum.DRAG;
            if (this.groupNumberPressed > 0)
            {
                this.selectionType = SelectionTypeEnum.KEY;
            }
            else
            {
                this.clicked = GetObjectClicked();
                if (this.clicked != null)
                {
                    this.selectionType = SelectionTypeEnum.CLICK;
                }
            }
            return this.selectionType;
        }

        public virtual HashSet<ISelectableObjectBehaviour> GetSelectionBySelectionType()
        {
            HashSet<ISelectableObjectBehaviour> list = new HashSet<ISelectableObjectBehaviour>();
            switch (this.selectionType)
            {
                case SelectionTypeEnum.CLICK:
                    list.Add(this.clicked);
                    break;
                case SelectionTypeEnum.KEY:
                    list.UnionWith(GetGroupSet(this.groupNumberPressed));
                    break;
                case SelectionTypeEnum.DRAG:
                    list.UnionWith(GetDragSelection());
                    break;
                default:
                    break;
            }
            return list;
        }

        private void AdjustSameTypeIfDoubleClick(HashSet<ISelectableObjectBehaviour> newSelection)
        {
            if (this.clicked != null && this.clicked == lastClicked && !this.isSameTypeSelection)
            {
                if (newSelection.Count == 0) newSelection.Add(this.clicked);
                if (this.currentSelection.Contains(this.clicked)) this.currentSelection.Remove(this.clicked);
                else this.currentSelection.Add(this.clicked);
                this.isSameTypeSelection = true;
            }
        }

        public SelectionArgsXP GetSelectionArgs(HashSet<ISelectableObjectBehaviour> newSelection)
        {
            if (this.isDoubleClick)
            {
                AdjustSameTypeIfDoubleClick(newSelection);
            }
            Vector3 initialSelectionPos = this.isSameTypeSelection ? this.minScreenPos : this.initialScreenPosition;
            Vector3 finalSelectionPos = this.isSameTypeSelection ? this.maxScreenPos : this.finalScreenPosition;

            return new SelectionArgsXP(this.currentSelection, newSelection, this.mainList);
        }

        public SelectionArgsXP ApplyModifiers(SelectionArgsXP args)
        {
            var collection = GetModifiersToBeApplied(this.selectionType);
            foreach (var item in collection)
            {
                if (this.isPreSelection)
                {
                    if (item.ActiveOnPreSelection)
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

        private static void UpdatePreSelectionStatus(HashSet<ISelectableObjectBehaviour> list, bool status)
        {
            foreach (var item in list)
            {
                item.IsPreSelected = status;
            }
        }

        private static void UpdateSelectionStatus(HashSet<ISelectableObjectBehaviour> list, bool status)
        {
            foreach (var item in list)
            {
                item.IsSelected = status;
            }
        }

        public HashSet<ISelectableObjectBehaviour> GetFinalSelection(SelectionArgsXP args)
        {
            if (this.isPreSelection)
            {
                UpdatePreSelectionStatus(args.ToBeAdded, true);
            }
            else
            {
                UpdateSelectionStatus(args.OldSelection, false);
                UpdateSelectionStatus(args.ToBeAdded, true);
            }
            return new HashSet<ISelectableObjectBehaviour>(args.ToBeAdded);
        }

        public HashSet<ISelectableObjectBehaviour> GetSelection(HashSet<ISelectableObjectBehaviour> newSelection)
        {
            var args = GetSelectionArgs(newSelection);
            args = ApplyModifiers(args);
            return GetFinalSelection(args);
        }

        public HashSet<ISelectableObjectBehaviour> GetUpdatedPreSelection(HashSet<ISelectableObjectBehaviour> newSelection)
        {
            var list = new HashSet<ISelectableObjectBehaviour>();
            if (this.preSelection != null && this.preSelection.Count > 0)
            {
                UpdatePreSelectionStatus(this.preSelection, false);
            }
            if (newSelection != null)
            {
                UpdatePreSelectionStatus(newSelection, true);
                list = newSelection;
            }

            return list;
        }

        public virtual HashSet<ISelectableObjectBehaviour> GetUpdatedCurrentSelection(HashSet<ISelectableObjectBehaviour> newSelection)
        {
            var list = new HashSet<ISelectableObjectBehaviour>();
            if (this.currentSelection != null && this.currentSelection.Count > 0)
            {
                UpdateSelectionStatus(this.currentSelection, false);
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
            if (this.groupNumberPressed <= 0) this.isSelecting = false;
            if (this.clicked != null) lastClicked = this.clicked;
            if (this.isDoubleClick) lastClicked = null;
            this.isDoubleClick = false;
            this.groupNumberPressed = 0;
            this.preSelection = new HashSet<ISelectableObjectBehaviour>();
        }

        public virtual HashSet<ISelectableObjectBehaviour> PerformSelection(Vector3 finalPos)
        {
            this.finalScreenPosition = finalPos;
            this.selectionType = GetSelectionType();
            var list = GetSelection(GetSelectionBySelectionType());
            return list;
        }

        public void DoPreSelection(Vector3 finalPos)
        {
            this.isPreSelection = true;
            HashSet<ISelectableObjectBehaviour> list = PerformSelection(finalPos);
            this.preSelection = GetUpdatedPreSelection(list);
        }

        public void DoSelection(Vector3 finalPos)
        {
            this.isPreSelection = false;
            UpdatePreSelectionStatus(this.preSelection, false);

            HashSet<ISelectableObjectBehaviour> list = PerformSelection(finalPos);
            this.currentSelection = GetUpdatedCurrentSelection(list);

            RestartVariables();
        }

        public void StartOfSelection(Vector3 initialPos)
        {
            this.initialScreenPosition = initialPos;
            this.isSelecting = true;
        }



    }
}
