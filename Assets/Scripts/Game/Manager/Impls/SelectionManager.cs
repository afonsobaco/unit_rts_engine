using RTSEngine.Core;
using RTSEngine.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RTSEngine.Manager
{
    public class SelectionManager : BaseSelectionManager, ISelectionManager<ISelectable, SelectionTypeEnum>
    {

        private Dictionary<int, List<ISelectable>> groups = new Dictionary<int, List<ISelectable>>();
        private IRuntimeSet<ISelectable> selectableList;
        private List<IBaseSelectionMod> scriptableObjectMods;
        private List<ISelectable> currentSelection;
        private List<ISelectable> preSelection;
        private ISelectable cliked;
        private Vector3 finalScreenPosition;
        private Vector3 initialScreenPosition;
        private bool isAditiveSelection;
        private bool isDoubleClick;
        private bool isSameTypeSelection;
        private int keyPressed = 0;
        private bool isSelecting;

        public List<ISelectable> PreSelection
        {
            get
            {
                if (preSelection == null)
                {
                    preSelection = new List<ISelectable>();
                }
                return preSelection;
            }
            set
            {
                preSelection = value;
            }
        }

        public Dictionary<int, List<ISelectable>> Groups { get => groups; private set => groups = value; }
        public IRuntimeSet<ISelectable> SelectableList { get => selectableList; set => selectableList = value; }
        public List<IBaseSelectionMod> ScriptableObjectMods { get => scriptableObjectMods; set => scriptableObjectMods = value; }
        public ISelectable Cliked { get => cliked; set => cliked = value; }
        public Vector3 FinalScreenPosition { get => finalScreenPosition; set => finalScreenPosition = value; }
        public Vector3 InitialScreenPosition { get => initialScreenPosition; set => initialScreenPosition = value; }
        public bool IsAditiveSelection { get => isAditiveSelection; set => isAditiveSelection = value; }
        public bool IsDoubleClick { get => isDoubleClick; set => isDoubleClick = value; }
        public bool IsSameTypeSelection { get => isSameTypeSelection; set => isSameTypeSelection = value; }
        public bool IsSelecting { get => isSelecting; set => isSelecting = value; }
        public int KeyPressed { get => keyPressed; set => keyPressed = value; }

        public virtual List<ISelectable> CurrentSelection
        {
            get
            {
                if (currentSelection == null)
                {
                    currentSelection = new List<ISelectable>();
                }
                return currentSelection;
            }
            set
            {
                currentSelection = value;
            }
        }

        public List<ISelectable> GetNewSelection()
        {
            List<ISelectable> list = new List<ISelectable>();

            switch (GetSelectionType())
            {
                case SelectionTypeEnum.CLICK:
                    list.Add(Cliked);
                    break;
                case SelectionTypeEnum.KEY:
                    list.AddRange(GetGroup(KeyPressed));
                    break;
                case SelectionTypeEnum.DRAG:
                    list.AddRange(GetSelectionOnScreen());
                    break;
                default:
                    break;
            }
            return list;
        }


        public SelectionTypeEnum GetSelectionType()
        {
            if (IsKey())
            {
                Cliked = null;
                return SelectionTypeEnum.KEY;
            }
            else if (IsClick())
            {
                KeyPressed = 0;
                return SelectionTypeEnum.CLICK;
            }
            Cliked = null;
            KeyPressed = 0;
            return SelectionTypeEnum.DRAG;
        }

        public bool IsKey()
        {
            return KeyPressed > 0;
        }

        public bool IsClick()
        {
            Cliked = GetObjectClicked();
            return Cliked != null;
        }



        public void SetGroup(int key)
        {
            Groups[key] = CurrentSelection;
        }

        public List<ISelectable> GetGroup(int key)
        {
            List<ISelectable> list;
            Groups.TryGetValue(key, out list);
            if (list == null)
            {
                list = new List<ISelectable>();
            }
            return list;
        }


        public List<ISelectable> UpdateCurrentSelection(List<ISelectable> value)
        {
            var list = new List<ISelectable>();
            //unselect old
            if (CurrentSelection != null && CurrentSelection.Count > 0)
            {
                this.UpdateSelectionStatus(CurrentSelection, false);
            }
            //select new
            if (value != null)
            {
                this.UpdateSelectionStatus(value, true);
                list = value;
            }

            return list;
        }

        public List<ISelectable> UpdatePreSelection(List<ISelectable> value)
        {
            var list = new List<ISelectable>();
            //unselect old
            if (preSelection != null && preSelection.Count > 0)
            {
                this.UpdatePreSelectionStatus(preSelection, false);
            }
            //select new
            if (value != null)
            {
                this.UpdatePreSelectionStatus(value, true);
                list = value;
            }

            return list;
        }


        public void StartOfSelection(Vector3 initialPos)
        {
            InitialScreenPosition = initialPos;
            IsSelecting = true;
        }

        public void DoPreSelection(Vector3 finalPos)
        {
            FinalScreenPosition = finalPos;
            var list = PerformPreSelection(preSelection, GetNewSelection(), GetSelectionType());
            PreSelection = UpdatePreSelection(list);
        }
        public void EndOfSelection(Vector3 finalPos)
        {
            //TODO ajustar testes
            FinalScreenPosition = finalPos;
            var list = PerformSelection(currentSelection, GetNewSelection(), GetSelectionType());
            CurrentSelection = UpdateCurrentSelection(list);
            this.UpdatePreSelectionStatus(preSelection, false);
            if (KeyPressed <= 0)
                IsSelecting = false;
            KeyPressed = 0;

        }

        public virtual Vector3 GetSelectionMainPoint()
        {
            if (CurrentSelection.Count > 0)
            {
                return CurrentSelection[0].Position;
            }
            return Vector3.zero;
        }

        public virtual ISelectable GetObjectClicked()
        {
            return SelectionUtil.GetObjectClicked(InitialScreenPosition, FinalScreenPosition);
        }

        public virtual List<ISelectable> GetSelectionOnScreen()
        {
            return SelectionUtil.GetAllObjectsInsideSelectionArea(SelectableList.GetList(), InitialScreenPosition, FinalScreenPosition);
        }

        public void AddSelectableObject(SelectableObjectCreatedSignal signal)
        {
            SelectableList.AddToList(signal.Selectable);
        }

        public void RemoveSelectableObject(SelectableObjectDeletedSignal signal)
        {
            SelectableList.RemoveFromList(signal.Selectable);
        }

        public void Dispose()
        {
            SelectableList.GetList().Clear();
        }
        public override SelectionArgsXP ApplyModifiers(SelectionArgsXP args)
        {

            var collection = GetModifiersToBeApplied(args.SelectionType);
            foreach (var item in collection)
            {
                //TODO testar active/inactive
                if (item.Active)
                {
                    if (args.IsPreSelection)
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
            }
            return args;

        }

        public List<IBaseSelectionMod> GetModifiersToBeApplied(SelectionTypeEnum type)
        {
            return GetAllModifiers().FindAll(x => x.Type.Equals(type) || x.Type.Equals(SelectionTypeEnum.ALL));
        }

        public virtual List<IBaseSelectionMod> GetAllModifiers()
        {
            return ScriptableObjectMods.FindAll(x => x is IBaseSelectionMod).Select(x => x as IBaseSelectionMod).ToList();
        }
    }
}
