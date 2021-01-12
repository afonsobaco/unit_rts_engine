using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RTSEngine.Core;
using RTSEngine.Selection;
using RTSEngine.Selection.Mod;
using RTSEngine.Selection.Util;
using System;

namespace RTSEngine.Manager
{
    public class SelectionManager : MonoBehaviour
    {

        [SerializeField] private Camera mainCamera;
        [SerializeField] private ISelectionSettings<SelectableObject, SelectableTypeEnum> selectionSettings;
        [SerializeField] private RectTransform selectionBox;
        [SerializeField] private Transform mods;

        private List<SelectableObject> selection = new List<SelectableObject>();
        private List<SelectableObject> preSelection = new List<SelectableObject>();
        private Dictionary<int, List<SelectableObject>> groups = new Dictionary<int, List<SelectableObject>>();
        private bool isSelecting;
        private Vector3 initialClickPosition;
        private Vector3 finalClickPosition;

        public static SelectionManager Instance { get; private set; }
        public bool IsAditiveSelection { get; set; }
        public bool IsSameTypeSelection { get; set; }
        public bool IsDoubleClick { get; set; }
        public bool IsSelecting
        {
            get { return isSelecting; }
            set
            {
                selectionBox.gameObject.SetActive(value);
                isSelecting = value;
            }
        }

        public SelectableObject ObjectClicked { get; private set; }
        public ISelectionSettings<SelectableObject, SelectableTypeEnum> SelectionSettings { get => selectionSettings; }


        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

    

        void Update()
        {
            if (IsSelecting)
            {
                DrawSelectionBox();
            }
        }

        public void StartOfSelection(Vector3 initialPos)
        {
            initialClickPosition = initialPos;
            Instance.IsSelecting = true;
        }

        public void EndOfSelection(Vector3 finalPos)
        {
            finalClickPosition = finalPos;
            DoSelection();
            FinalizeSelection();
        }

        private void FinalizeSelection()
        {
            HideSelectionBox();
            preSelection.ForEach(a => a.IsPreSelected = false);
            preSelection = new List<SelectableObject>();
            Instance.IsSelecting = false;
            Instance.IsDoubleClick = false;
        }


        public void DoSelection()
        {
            SelectableObject clicked = GetSelectableObjectClicked();
            List<SelectableObject> newSelection = GetPrimarySelection(clicked);
            newSelection = PerformSelection(newSelection, clicked);
        }

        private List<SelectableObject> PerformSelection(List<SelectableObject> newSelection, SelectableObject clicked)
        {
            newSelection = ApplyModsToSelection(selection, newSelection, clicked);
            SwitchSelectionStatusFromOldToNewList(selection, newSelection);
            selection = newSelection;
            return newSelection;
        }

        public void DoPreSelection(Vector3 finalPos)
        {
            finalClickPosition = finalPos;
            List<SelectableObject> newSelection = GetPrimaryPreSelection();
            newSelection = ApplyModsToPreSelection(preSelection, newSelection);
            SwitchPreSelectionStatusFromOldToNewList(preSelection, newSelection);
            preSelection = newSelection;
        }

        private List<SelectableObject> GetPrimarySelection(SelectableObject clicked)
        {
            List<SelectableObject> newSelection = new List<SelectableObject>();
            if (clicked)
            {
                newSelection.Add(clicked);
            }
            else
            {
                newSelection = SelectionUtil.GetAllObjectsInsideSelectionArea<SelectableObject>(SelectableObjectMainList.Instance.List, initialClickPosition, finalClickPosition, mainCamera);
            }
            return newSelection;
        }
        private List<SelectableObject> GetPrimaryPreSelection()
        {
            return SelectionUtil.GetAllObjectsInsideSelectionArea<SelectableObject>(SelectableObjectMainList.Instance.List, initialClickPosition, finalClickPosition, mainCamera);
        }

        private List<SelectableObject> ApplyModsToSelection(List<SelectableObject> oldSelection, List<SelectableObject> newSelection, SelectableObject clicked)
        {
            SelectionArgs args = GetSelectionArgs(oldSelection, newSelection);
            args.Clicked = clicked;
            args.IsPreSelection = false;
            return ApplyMods(args);
        }

        private List<SelectableObject> ApplyModsToPreSelection(List<SelectableObject> oldSelection, List<SelectableObject> newSelection)
        {
            SelectionArgs args = GetSelectionArgs(oldSelection, newSelection);
            args.Clicked = null;
            args.IsPreSelection = true;
            return ApplyMods(args);
        }

        private List<SelectableObject> ApplyMods(SelectionArgs args)
        {
            if (args.NewList.Count != 0 || args.IsAditive || args.IsSameType)
            {
                if (args.IsAditive)
                {
                    args.NewList = args.OldList.Union(args.NewList).ToList();
                }
                foreach (var mod in mods.GetComponents<IAbstractSelectionMod<SelectableObject, SelectableTypeEnum>>())
                {
                    // args.NewList = mod.ApplyMod(args);
                };
            }
            return args.NewList;
        }

        private void SwitchSelectionStatusFromOldToNewList(List<SelectableObject> oldSelection, List<SelectableObject> newSelection)
        {
            oldSelection.ForEach(a =>
            {
                a.IsSelected = newSelection.Contains(a);
            });
            newSelection.ForEach(a =>
            {
                a.IsSelected = true;
            });
        }

        private void SwitchPreSelectionStatusFromOldToNewList(List<SelectableObject> oldSelection, List<SelectableObject> newSelection)
        {
            oldSelection.ForEach(a => a.IsPreSelected = newSelection.Contains(a));
            newSelection.ForEach(a => a.IsPreSelected = true);
        }

        private SelectableObject GetSelectableObjectClicked()
        {
            return SelectionUtil.GetObjectClicked<SelectableObject>(initialClickPosition, finalClickPosition, mainCamera);
        }

        private void DrawSelectionBox()
        {
            selectionBox.position = SelectionUtil.GetAreaCenter(initialClickPosition, finalClickPosition);
            selectionBox.sizeDelta = SelectionUtil.GetAreaSize(initialClickPosition, finalClickPosition);
        }

        private void HideSelectionBox()
        {
            selectionBox.position = Vector3.zero;
            selectionBox.sizeDelta = Vector2.zero;
        }

        private SelectionArgs GetSelectionArgs(List<SelectableObject> oldSelection, List<SelectableObject> newSelection)
        {
            SelectionArgs args = new SelectionArgs();
            args.MainList =SelectableObjectMainList.Instance.List;
            args.NewList = newSelection;
            args.IsAditive = IsAditiveSelection;
            args.PreSelectionList = preSelection;
            if (IsAditiveSelection)
            {
                args.OldList = oldSelection;
            }
            else
            {
                args.OldList = new List<SelectableObject>();
            }
            args.IsDoubleClick = IsDoubleClick;
            args.IsSameType = IsSameTypeSelection;
            args.Camera = mainCamera;
            return args;
        }

        public Vector3 GetSelectionMainPoint()
        {
            if (selection.Count == 0)
            {
                return Vector3.zero;
            }
            return selection[0].transform.position;
        }

        public void SetGroup(int keyPressed)
        {
            groups[keyPressed] = selection;
        }

        public void GetGroup(int keyPressed)
        {
            List<SelectableObject> list;
            groups.TryGetValue(keyPressed, out list);
            if (list == null)
            {
                list = new List<SelectableObject>();
            }
            PerformSelection(list, null);
        }

        public List<SelectableObject> GetSelection()
        {
            return selection;
        }
    }


}