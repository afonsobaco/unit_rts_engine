using System.Collections.Generic;
using UnityEngine;
using Zenject;


namespace RTSEngine.Manager
{
    public class SelectionManager : ISelectionManager<ISelectableObject, SelectionTypeEnum>
    {
        private ISelectionSettings settings;

        private Dictionary<int, HashSet<ISelectableObject>> groupSet = new Dictionary<int, HashSet<ISelectableObject>>();
        private HashSet<ISelectableObject> mainList = new HashSet<ISelectableObject>();
        private HashSet<ISelectableObject> currentSelection = new HashSet<ISelectableObject>();
        private HashSet<ISelectableObject> preSelection = new HashSet<ISelectableObject>();
        private ISelectableObject lastClicked;
        private ISelectableObject clicked;

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
        private SignalBus signalBus;

        public SelectionManager(SignalBus signalBus)
        {
            this.signalBus = signalBus;
            mods = new List<ISelectionModifier>()
            {
                new SameTypeSelectionModifier(this),
                new OrderOfSelectionModifier(this),
                new AdditiveSelectionModifier(this),
                new GroupRestrictorSelectionModifier(this),
                new LimitSelectionModifier(this)
            };
        }

        public ISelectionSettings GetSettings()
        {
            return settings;
        }

        public void SetSettings(ISelectionSettings value)
        {
            settings = value;
        }

        public void SetMainList(HashSet<ISelectableObject> list)
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

        public void SetCurrentSelection(HashSet<ISelectableObject> selection)
        {
            this.currentSelection = selection;
        }

        public void SetClicked(ISelectableObject selected)
        {
            this.clicked = selected;
        }

        public void SetLastClicked(ISelectableObject selected)
        {
            this.lastClicked = selected;
        }

        public void SetIsPreSelection(bool value)
        {
            this.isPreSelection = value;
        }

        public void SetPreSelection(HashSet<ISelectableObject> preSelection)
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
            Vector3 initialSelectionPos = this.isSameTypeSelection && !this.isPreSelection ? this.minScreenPos : this.initialScreenPosition;
            return initialSelectionPos;
        }

        public Vector2 GetFinalScreenPosition()
        {
            Vector3 finalSelectionPos = this.isSameTypeSelection && !this.isPreSelection ? this.maxScreenPos : this.finalScreenPosition;
            return finalSelectionPos;

        }

        public HashSet<ISelectableObject> GetCurrentSelection()
        {
            return this.currentSelection;
        }

        public int GetGroupSetNumberPressed()
        {
            return this.groupNumberPressed;
        }

        public IEnumerable<ISelectableObject> GetPreSelection()
        {
            return this.preSelection;
        }

        public void CreateGroupSet(int number)
        {
            this.groupSet[number] = this.currentSelection;
            //TODO call signal
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

        public virtual ISelectableObject GetObjectClicked()
        {
            return SelectionUtil.GetObjectClicked(this.initialScreenPosition, this.finalScreenPosition);
        }

        public virtual Dictionary<int, HashSet<ISelectableObject>> GetAllGroupSets()
        {
            return this.groupSet;
        }

        public HashSet<ISelectableObject> OrderSelection(HashSet<ISelectableObject> newSelection)
        {
            var orderedListOfSelection = new HashSet<ISelectableObject>(this.preSelection);
            orderedListOfSelection.RemoveWhere(x => !newSelection.Contains(x));
            orderedListOfSelection.UnionWith(newSelection);
            return orderedListOfSelection;
        }

        public virtual HashSet<ISelectableObject> GetDragSelection()
        {
            var selectionOnScreen = SelectionUtil.GetAllObjectsInsideSelectionArea(this.mainList, this.initialScreenPosition, this.finalScreenPosition);
            return OrderSelection(selectionOnScreen);
        }

        public virtual List<ISelectionModifier> GetModifiersToBeApplied(SelectionTypeEnum type)
        {
            return this.mods.FindAll(x => x.Type.Equals(type) || x.Type.Equals(SelectionTypeEnum.ANY));
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

        public virtual HashSet<ISelectableObject> GetSelectionBySelectionType()
        {
            HashSet<ISelectableObject> list = new HashSet<ISelectableObject>();
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

        private void AdjustSameTypeIfDoubleClick(HashSet<ISelectableObject> newSelection)
        {
            if (this.clicked != null && this.clicked == lastClicked && !this.isSameTypeSelection)
            {
                if (newSelection.Count == 0) newSelection.Add(this.clicked);
                if (this.currentSelection.Contains(this.clicked)) this.currentSelection.Remove(this.clicked);
                else this.currentSelection.Add(this.clicked);
                this.isSameTypeSelection = true;
            }
        }

        public SelectionArgsXP GetSelectionArgs(HashSet<ISelectableObject> newSelection)
        {
            if (this.isDoubleClick)
            {
                AdjustSameTypeIfDoubleClick(newSelection);
            }
            return new SelectionArgsXP(this.currentSelection, newSelection, this.mainList);
        }

        public SelectionArgsXP ApplyModifiers(SelectionArgsXP args)
        {
            var collection = GetModifiersToBeApplied(this.selectionType);
            foreach (var item in collection)
            {
                if (this.isPreSelection)
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

        public HashSet<ISelectableObject> GetFinalSelection(SelectionArgsXP args)
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

        public virtual HashSet<ISelectableObject> GetUpdatedCurrentSelection(HashSet<ISelectableObject> newSelection)
        {
            var list = new HashSet<ISelectableObject>();
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
            this.preSelection = new HashSet<ISelectableObject>();
        }

        public virtual HashSet<ISelectableObject> PerformSelection(Vector3 finalPos)
        {
            this.finalScreenPosition = finalPos;
            this.selectionType = GetSelectionType();
            var list = GetSelection(GetSelectionBySelectionType());
            return list;
        }

        public void DoPreSelection(Vector3 finalPos)
        {
            this.isPreSelection = true;
            HashSet<ISelectableObject> list = PerformSelection(finalPos);
            this.preSelection = GetUpdatedPreSelection(list);
        }

        public void DoSelection(Vector3 finalPos)
        {
            this.isPreSelection = false;
            UpdatePreSelectionStatus(this.preSelection, false);

            HashSet<ISelectableObject> list = PerformSelection(finalPos);
            this.currentSelection = GetUpdatedCurrentSelection(list);
            signalBus.Fire<UpdateGUISignal>();
            RestartVariables();
        }

        public void StartOfSelection(Vector3 initialPos)
        {
            this.initialScreenPosition = initialPos;
            this.isSelecting = true;
        }



    }
}
