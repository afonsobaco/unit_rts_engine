using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Selection;
using RTSEngine.Selection.Util;
using System;

namespace RTSEngine.Manager
{
    public class SelectionManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private GameRuntimeSet selectableList;
        private SelectionManager manager = new SelectionManager();

        public SelectionManager Manager { get => manager; set => manager = value; }

        private void Awake()
        {
            Manager.SelectableList = selectableList;
        }
    }

    public interface ISelectionManager
    {
        Vector3 InitialScreenPosition { get; set; }
        Vector3 FinalScreenPosition { get; set; }
        int KeyPressed { get; set; }
        SelectableObject Cliked { get; set; }
        List<SelectableObject> CurrentSelection { get; set; }
        GameRuntimeSet SelectableList { get; set; }

        List<SelectableObject> GetGroup(int key);
        List<SelectableObject> GetNewSelection();
        SelectionTypeEnum GetSelectionType();
        void SetGroup(int key);
        void UpdateCurrentSelection(List<SelectableObject> value);
        Vector3 GetSelectionMainPoint();
        void DebugClass();
    }

    public class SelectionManager : AbstractSelectionManager<SelectableObject, SelectionTypeEnum>, ISelectionManager
    {

        private GameRuntimeSet selectableList;
        private Vector3 initialScreenPosition;
        private Vector3 finalScreenPosition;
        private int keyPressed = -1;
        private SelectableObject cliked;
        private Dictionary<int, List<SelectableObject>> groups = new Dictionary<int, List<SelectableObject>>();
        private List<SelectableObject> currentSelection;

        public Vector3 InitialScreenPosition { get => initialScreenPosition; set => initialScreenPosition = value; }
        public Vector3 FinalScreenPosition { get => finalScreenPosition; set => finalScreenPosition = value; }
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

        public GameRuntimeSet SelectableList { get => selectableList; set => selectableList = value; }

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
            UpdateCurrentSelection(list);
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
                KeyPressed = -1;
                return SelectionTypeEnum.CLICK;
            }
            Cliked = null;
            KeyPressed = -1;
            return SelectionTypeEnum.DRAG;
        }

        private bool IsKey()
        {
            return keyPressed != -1;
        }

        private bool IsClick()
        {
            Cliked = SelectionUtil.GetObjectClicked<SelectableObject>(InitialScreenPosition, FinalScreenPosition);
            return Cliked != null;
        }

        public void SetGroup(int key)
        {
            groups[key] = CurrentSelection;
        }

        public List<SelectableObject> GetGroup(int key)
        {
            List<SelectableObject> list;
            groups.TryGetValue(key, out list);
            if (list == null)
            {
                list = new List<SelectableObject>();
            }
            return list;
        }

        private List<SelectableObject> GetSelectionOnScreen()
        {
            return SelectionUtil.GetAllObjectsInsideSelectionArea<SelectableObject>(SelectableList.GetList(), InitialScreenPosition, FinalScreenPosition);
        }

        public void UpdateCurrentSelection(List<SelectableObject> value)
        {
            //unselect old
            if (currentSelection != null && currentSelection.Count > 0)
            {
                this.UpdateSelectionStatus(currentSelection, false);
            }
            //select new
            if (value != null)
            {
                this.UpdateSelectionStatus(value, true);
                currentSelection = value;
            }
            else
            {
                currentSelection = new List<SelectableObject>();
            }
        }

        public Vector3 GetSelectionMainPoint()
        {
            //TODO to be implemented
            return Vector3.zero;
        }

        public void DebugClass()
        {
            Debug.Log("Selection Manager Class");
        }
    }
}
