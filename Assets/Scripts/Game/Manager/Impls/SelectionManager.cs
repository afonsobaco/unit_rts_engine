using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core.Impls;
using RTSEngine.Core.Signals;
using RTSEngine.Core.Enums;
using RTSEngine.Manager.Enums;
using RTSEngine.Manager.Abstracts;
using RTSEngine.Manager.Interfaces;
using RTSEngine.Core.Interfaces;
using RTSEngine.Manager.Utils;
using RTSEngine.Manager.Impls.SelectionMods.Abstracts;

namespace RTSEngine.Manager.Impls
{
    public class SelectionManager : AbstractSelectionManager<SelectableObject, SelectionTypeEnum>, ISelectionManager<SelectableObject, SelectionTypeEnum>
    {

        private IRuntimeSet<SelectableObject> selectableList;
        private List<ScriptableObject> scriptableObjectMods;
        private Vector3 initialScreenPosition;
        private Vector3 finalScreenPosition;
        private bool isAditiveSelection;
        private bool isSameTypeSelection;
        private bool isDoubleClick;
        private int keyPressed = 0;
        private SelectableObject cliked;
        private Dictionary<int, List<SelectableObject>> groups = new Dictionary<int, List<SelectableObject>>();
        private List<SelectableObject> currentSelection;
        private List<SelectableObject> preSelection;
        private bool isSelecting;

        public Vector3 InitialScreenPosition { get => initialScreenPosition; set => initialScreenPosition = value; }
        public Vector3 FinalScreenPosition { get => finalScreenPosition; set => finalScreenPosition = value; }
        public bool IsAditiveSelection { get => isAditiveSelection; set => isAditiveSelection = value; }
        public bool IsSameTypeSelection { get => isSameTypeSelection; set => isSameTypeSelection = value; }
        public int KeyPressed { get => keyPressed; set => keyPressed = value; }
        public SelectableObject Cliked { get => cliked; set => cliked = value; }
        public List<SelectableObject> CurrentSelection
        {
            get
            {
                if (currentSelection == null)
                {
                    currentSelection = new List<SelectableObject>();
                }
                return currentSelection;
            }
            set
            {
                currentSelection = value;
            }
        }

        public List<SelectableObject> PreSelection
        {
            get
            {
                if (preSelection == null)
                {
                    preSelection = new List<SelectableObject>();
                }
                return preSelection;
            }
            set
            {
                preSelection = value;
            }
        }

        public IRuntimeSet<SelectableObject> SelectableList { get => selectableList; set => selectableList = value; }
        public bool IsDoubleClick { get => isDoubleClick; set => isDoubleClick = value; }
        public Dictionary<int, List<SelectableObject>> Groups { get => groups; private set => groups = value; }
        public bool IsSelecting { get => isSelecting; set => isSelecting = value; }
        public List<ScriptableObject> ScriptableObjectMods { get => scriptableObjectMods; set => scriptableObjectMods = value; }

        public List<SelectableObject> GetNewSelection()
        {
            List<SelectableObject> list = new List<SelectableObject>();

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

        public List<SelectableObject> GetGroup(int key)
        {
            List<SelectableObject> list;
            Groups.TryGetValue(key, out list);
            if (list == null)
            {
                list = new List<SelectableObject>();
            }
            return list;
        }


        public List<SelectableObject> UpdateCurrentSelection(List<SelectableObject> value)
        {
            var list = new List<SelectableObject>();
            //unselect old
            if (currentSelection != null && currentSelection.Count > 0)
            {
                this.UpdateSelectionStatus(currentSelection, false);
            }
            //select new
            if (value != null)
            {
                this.UpdateSelectionStatus(value, true);
                list = value;
            }

            return list;
        }

        public List<SelectableObject> UpdatePreSelection(List<SelectableObject> value)
        {
            var list = new List<SelectableObject>();
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
            var list = PerformSelection(preSelection, GetNewSelection(), GetSelectionType());
            PreSelection = UpdatePreSelection(list);
        }
        public void EndOfSelection(Vector3 finalPos)
        {
            //TODO ajustar testes
            FinalScreenPosition = finalPos;
            var list = PerformSelection(currentSelection, GetNewSelection(), GetSelectionType());
            CurrentSelection = UpdateCurrentSelection(list);
            this.UpdatePreSelectionStatus(preSelection, false);
            KeyPressed = 0;
            IsSelecting = false;

        }

        public Vector3 GetSelectionMainPoint()
        {
            if (CurrentSelection.Count > 0)
            {
                return CurrentSelection[0].transform.position;
            }
            return Vector3.zero;
        }

        public virtual SelectableObject GetObjectClicked()
        {
            return SelectionUtil.GetObjectClicked<SelectableObject>(InitialScreenPosition, FinalScreenPosition);
        }

        public virtual List<SelectableObject> GetSelectionOnScreen()
        {
            return SelectionUtil.GetAllObjectsInsideSelectionArea<SelectableObject>(SelectableList.GetList(), InitialScreenPosition, FinalScreenPosition);
        }

        public void AddSelectableObject(SelectableObjectCreatedSignal signal)
        {
            SelectableList.AddToList(signal.selectableObject);
        }

        public void RemoveSelectableObject(SelectableObjectDeletedSignal signal)
        {
            SelectableList.RemoveFromList(signal.selectableObject);
        }

        public void Dispose()
        {
            SelectableList.GetList().ForEach(x => x.gameObject.SetActive(false));
            SelectableList.GetList().Clear();
        }

        public override ISelectionArgsXP<SelectableObject, SelectionTypeEnum> ApplyModifiers(ISelectionArgsXP<SelectableObject, SelectionTypeEnum> args)
        {
            //TODO test
            // var parsedModList = Mods.FindAll(x => x is ISelectionMod<SelectableObject, SelectionTypeEnum>).Select(x => x as ISelectionMod<SelectableObject, SelectionTypeEnum>).ToList();
            // List<ISelectionMod<SelectableObject, SelectionTypeEnum>> filteredMods = GetModsBySelectionType(parsedModList, args.SelectionType);
            // filteredMods.Union(GetModsBySelectionType(parsedModList, SelectionTypeEnum.ALL));
            // foreach (var item in filteredMods)
            // {
            //     args = item.Apply(args);
            // }
            return args;

        }

        public override List<ISelectionMod<SelectableObject, SelectionTypeEnum>> GetModifiersToBeApplied(SelectionTypeEnum selectionType)
        {
            var parsedModList = ScriptableObjectMods.FindAll(x => x is ISelectionMod<SelectableObject, SelectionTypeEnum>).Select(x => x as ISelectionMod<SelectableObject, SelectionTypeEnum>).ToList();
            List<ISelectionMod<SelectableObject, SelectionTypeEnum>> modifiersToBeApplied = GetModsBySelectionType(parsedModList, selectionType);
            modifiersToBeApplied.Union(GetModsBySelectionType(parsedModList, SelectionTypeEnum.ALL));
            return modifiersToBeApplied;
        }
    }
}
