using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public partial class SelectionManager : MonoBehaviour
{



    public static SelectionManager Instance { get; private set; }

    [SerializeField]
    private List<SelectableObject> mainList = new List<SelectableObject>();

    [SerializeField]
    private List<SelectableObject> selection = new List<SelectableObject>();

    [SerializeField]
    private List<SelectableObject> selectionPreview = new List<SelectableObject>();

    public float clickDelayTime = 0.3f;
    private float lastSelectionTime;

    private Dictionary<int, List<SelectableObject>> groupSelection = new Dictionary<int, List<SelectableObject>>();
    private LastClickedReference lastSelectedObject;

    private List<SelectableObject.SelectableTypeEnum> canGroup = new List<SelectableObject.SelectableTypeEnum>(){
        SelectableObject.SelectableTypeEnum.UNIT,
        SelectableObject.SelectableTypeEnum.BUILDING
    };

    private List<SelectableObject.SelectableTypeEnum> canSelectMultipleOnDrag = new List<SelectableObject.SelectableTypeEnum>(){
        SelectableObject.SelectableTypeEnum.UNIT
    };
    private List<SelectableObject.SelectableTypeEnum> canSelectMultiple = new List<SelectableObject.SelectableTypeEnum>(){
        SelectableObject.SelectableTypeEnum.UNIT,
        SelectableObject.SelectableTypeEnum.BUILDING
    };

    private Dictionary<SelectableObject.SelectableTypeEnum, int> precedenceSelectionOrder = new Dictionary<SelectableObject.SelectableTypeEnum, int>(){
        {SelectableObject.SelectableTypeEnum.UNIT, 1},
        {SelectableObject.SelectableTypeEnum.BUILDING, 2},
        {SelectableObject.SelectableTypeEnum.CONSUMABLE, 3},
        {SelectableObject.SelectableTypeEnum.ENVIRONMENT, 4}
    };

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

    public void RemoveFromMainList(SelectableObject selectableObject)
    {
        this.mainList.Remove(selectableObject);
    }

    public void AddToMainList(SelectableObject selectableObject)
    {
        this.mainList.Add(selectableObject);
    }

    public Vector3 GetSelectionMainPoint()
    {
        if (selection.Count > 0)
        {
            return GetSelectedMidPoint();
        }
        return Vector3.zero;
    }

    public void SetGroup(int keyPressed)
    {
        if (selection.Count > 0)
        {
            var selectionGroup = selection.FindAll((a) => { return canGroup.Contains(a.type); });
            groupSelection[keyPressed] = selectionGroup;
        }
        else
        {
            groupSelection.Remove(keyPressed);
        }
    }

    public void GetGroup(int keyPressed)
    {
        UpdateSelectionStatus(selection, false);
        if (!groupSelection.TryGetValue(keyPressed, out selection))
        {
            selection = new List<SelectableObject>();
        }
        UpdateSelectionStatus(selection, true);

    }

    public void DoSelectionPreview(SelectionArgObject args)
    {
        List<SelectableObject> previousSelection = new List<SelectableObject>();
        previousSelection.AddRange(selectionPreview);
        selectionPreview = new List<SelectableObject>();
        if (SingleSelection(args) == null)
        {
            List<SelectableObject> found = FindSelectableObjectsInArea(args);
            selectionPreview.AddRange(found.FindAll(a => !selection.Contains(a)));
        }
        UpdateSelectionStatus(previousSelection, false);
        UpdateSelectionStatus(selectionPreview, true);
    }

    public void DoSelection(SelectionArgObject args)
    {
        List<SelectableObject> previousSelection = new List<SelectableObject>();
        previousSelection.AddRange(selection);
        SelectableObject selectableObject = SingleSelection(args);
        if (selectableObject != null)
        {
            DoSingleSelection(args, selectableObject);
        }
        else
        {
            DoSelectionInRange(args);
        }
        UpdateSelectionStatus(previousSelection, false);
        UpdateSelectionStatus(selection, true);
        selectionPreview = new List<SelectableObject>();
    }

    private void DoSingleSelection(SelectionArgObject args, SelectableObject selected)
    {
        if (IsDoubleClicked(selected) || args.MultipleSelection)
        {
            SelectAllFromTypeInsideRange(args, selected);
        }
        else
        {
            SingleSelection(selected);
            selection = AdjustMultipleSelection(selected, selection, selected.type);
            lastSelectionTime = Time.time;
        }
    }
    private bool IsDoubleClicked(SelectableObject selectableObject)
    {
        if (lastSelectedObject == null)
        {
            return false;
        }
        float timBetweenClicks = (Time.time - lastSelectionTime);
        return selection.Count > 0 && selectableObject == lastSelectedObject.SelectedObject && timBetweenClicks < clickDelayTime;
    }

    private void SelectAllFromTypeInsideRange(SelectionArgObject args, SelectableObject selected)
    {
        args.Position = args.MainCamera.ViewportToScreenPoint(new Vector2(.5f, .5f));
        args.SizeDelta = new Vector2(args.MainCamera.pixelWidth, args.MainCamera.pixelHeight);
        DoSelectionInRangeFromSameType(args, selected);
    }

    private void SingleSelection(SelectableObject selected)
    {
        lastSelectedObject = new LastClickedReference();
        lastSelectedObject.SelectedObject = selected;
        if (AditionalSelectionIsActive())
        {
            AddRemoveSelection(selected);
        }
        else
        {
            if (selection.Count > 0)
            {
                selection = new List<SelectableObject>();
            }
            AddToSelection(selected);
        }
    }
    private void DoSelectionInRange(SelectionArgObject args)
    {
        if (!AditionalSelectionIsActive())
        {
            selection = new List<SelectableObject>();
        }

        List<SelectableObject> found = FindSelectableObjectsInArea(args);
        selection.AddRange(found.FindAll(a => !selection.Contains(a)));
    }

    private void DoSelectionInRangeFromSameType(SelectionArgObject args, SelectableObject selected)
    {
        if (!AditionalSelectionIsActive())
        {
            selection = new List<SelectableObject>();
        }
        if (lastSelectedObject.IsSelected)
        {
            List<SelectableObject> selectableObjects = FindSelectableObjectsInArea(args, selected);
            selectableObjects.ForEach(o => selection.Remove(o));
            if (selection.Count == 0)
            {
                selection.AddRange(selectableObjects);
            }
        }
        else
        {
            List<SelectableObject> found = FindSelectableObjectsInArea(args, selected);
            selection.AddRange(found.FindAll(a => !selection.Contains(a)));
        }

    }

    private List<SelectableObject> FindSelectableObjectsInArea(SelectionArgObject args)
    {
        return FindSelectableObjectsInArea(args, null);
    }

    private List<SelectableObject> FindSelectableObjectsInArea(SelectionArgObject args, SelectableObject selected)
    {

        List<SelectableObject> list = new List<SelectableObject>();

        foreach (var obj in mainList)
        {
            var screenPointPosition = args.MainCamera.WorldToScreenPoint(obj.transform.position);
            bool isObjectInsideArea = screenPointPosition.x > args.Min.x && screenPointPosition.x < args.Max.x && screenPointPosition.y > args.Min.y && screenPointPosition.y < args.Max.y;
            if (isObjectInsideArea)
            {
                bool mustAddSelectable = selected == null || selected.typeStr.Equals(obj.typeStr);
                if (mustAddSelectable)
                {
                    list.Add(obj);
                }
            }
        }

        return NormalizeList(selected, list);
    }

    private List<SelectableObject> NormalizeList(SelectableObject selected, List<SelectableObject> list)
    {
        SelectableObject.SelectableTypeEnum type = GetMinPrecedenceOrder(list);
        List<SelectableObject> filteredListByType = list.FindAll(s => s.type == type);
        filteredListByType = AdjustMultipleSelection(selected, filteredListByType, type);
        return filteredListByType;
    }

    private SelectableObject.SelectableTypeEnum GetMinPrecedenceOrder(List<SelectableObject> list)
    {
        var minPrecedence = 999;
        SelectableObject.SelectableTypeEnum type = SelectableObject.SelectableTypeEnum.NONE;
        list.ForEach(s =>
        {
            int objPrecedence;
            if (precedenceSelectionOrder.TryGetValue(s.type, out objPrecedence) && minPrecedence > objPrecedence)
            {
                minPrecedence = objPrecedence;
                type = s.type;
            }
        });
        return type;
    }

    private List<SelectableObject> AdjustMultipleSelection(SelectableObject selected, List<SelectableObject> list, SelectableObject.SelectableTypeEnum type)
    {
        if (!canSelectMultiple.Contains(type) && list.Count > 0)
        {
            return new List<SelectableObject>() { selected };
        }
        return list;
    }

    private bool AditionalSelectionIsActive()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    private SelectableObject SingleSelection(SelectionArgObject args)
    {
        var initialObject = GetSelectableObjectInPoint(args.MainCamera, args.Min);
        var finalObject = GetSelectableObjectInPoint(args.MainCamera, args.Max);
        if (initialObject != null && initialObject.Equals(finalObject))
        {
            return initialObject;
        }
        return null;
    }

    private SelectableObject GetSelectableObjectInPoint(Camera mainCamera, Vector3 mousePosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            return hit.transform.gameObject.GetComponent<SelectableObject>();
        }
        return null;
    }

    private void AddRemoveSelection(SelectableObject selectableObject)
    {
        if (!selection.Contains(selectableObject))
        {
            AddToSelection(selectableObject);
        }
        else
        {
            RemoveFromSelection(selectableObject);
        }
    }

    private void AddToSelection(SelectableObject selectableObject)
    {
        if (!selection.Contains(selectableObject))
            selection.Add(selectableObject);
    }

    private void RemoveFromSelection(SelectableObject selectableObject)
    {
        if (selection.Contains(selectableObject))
            selection.Remove(selectableObject);
    }

    private void UpdateSelectionStatus(List<SelectableObject> selectionList, Boolean status)
    {
        foreach (var item in selectionList)
        {
            item.IsSelected = status;
        }
    }

    private Vector3 GetSelectedMidPoint()
    {
        Vector3 sum = Vector3.zero;
        foreach (var item in selection)
        {
            sum += item.transform.position;
        }
        return sum / selection.Count;
    }

    private class LastClickedReference
    {
        private SelectableObject _selectedObject;

        public SelectableObject SelectedObject
        {
            get
            {
                return _selectedObject;
            }
            set
            {
                _selectedObject = value;
                IsSelected = _selectedObject.IsSelected;
            }
        }
        public bool IsSelected { get; private set; }

    }

}

