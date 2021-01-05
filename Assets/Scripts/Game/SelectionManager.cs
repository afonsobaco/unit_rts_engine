using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public partial class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    [SerializeField]
    private List<SelectableObject> allSelectable = new List<SelectableObject>();
    [SerializeField]
    private List<SelectableObject> selection = new List<SelectableObject>();

    [SerializeField]
    private List<SelectableObject> selectionPreview = new List<SelectableObject>();

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
        this.allSelectable.Remove(selectableObject);
    }

    public void AddToMainList(SelectableObject selectableObject)
    {
        this.allSelectable.Add(selectableObject);
    }

    public Vector3 GetSelectionMainPoint()
    {
        if (selection.Count > 0)
        {
            Vector3 midPoint = GetSelectedMidPoint();
            float z = midPoint.z - (transform.position.y * Mathf.Tan((90 - this.transform.rotation.eulerAngles.x) * Mathf.Deg2Rad));
            return new Vector3(midPoint.x, transform.position.y, (float)z);
        }
        return Vector3.zero;
    }

    public void DoSelectionPreview(SelectionArgObject args)
    {
        List<SelectableObject> previousSelection = new List<SelectableObject>();
        previousSelection.AddRange(selectionPreview);
        selectionPreview = new List<SelectableObject>();
        if (SingleSelection(args) == null)
        {
            selectionPreview.AddRange(PerformBoxSelection(args, selection));
        }
        UpdateSelectionStatus(previousSelection, false);
        UpdateSelectionStatus(selectionPreview, true);
    }

    public void DoSelection(SelectionArgObject args)
    {
        List<SelectableObject> previousSelection = new List<SelectableObject>();
        previousSelection.AddRange(selection);
        previousSelection.AddRange(selection);
        SelectableObject selectableObject = SingleSelection(args);
        if (selectableObject != null)
        {
            PerformSingleSelection(selectableObject);
        }
        else
        {
            if (!AditionalSelectionIsActive())
            {
                selection = new List<SelectableObject>();
            }
            selection.AddRange(PerformBoxSelection(args, selection));
        }
        UpdateSelectionStatus(previousSelection, false);
        UpdateSelectionStatus(selection, true);
        selectionPreview = new List<SelectableObject>();
    }


    private void PerformSingleSelection(SelectableObject selected)
    {
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


    private List<SelectableObject> PerformBoxSelection(SelectionArgObject args, List<SelectableObject> selectionList)
    {
        return GetNewSelectableObjectsInArea(args, selectionList);
    }

    private List<SelectableObject> GetNewSelectableObjectsInArea(SelectionArgObject args, List<SelectableObject> exceptionList)
    {
        var min = new Vector2(args.SelectionBox.position.x - (args.SelectionBox.sizeDelta.x / 2), args.SelectionBox.position.y - (args.SelectionBox.sizeDelta.y / 2));
        var max = new Vector2(args.SelectionBox.position.x + (args.SelectionBox.sizeDelta.x / 2), args.SelectionBox.position.y + (args.SelectionBox.sizeDelta.y / 2));
        List<SelectableObject> list = new List<SelectableObject>();
        foreach (var obj in allSelectable)
        {
            var screenPointPosition = args.MainCamera.WorldToScreenPoint(obj.transform.position);
            if (screenPointPosition.x > min.x && screenPointPosition.x < max.x && screenPointPosition.y > min.y && screenPointPosition.y < max.y)
            {
                if (!exceptionList.Contains(obj))
                    list.Add(obj);
            }
        }
        return list;
    }

    private bool AditionalSelectionIsActive()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }

    private SelectableObject SingleSelection(SelectionArgObject args)
    {
        var initialObject = GetSelectableObjectInPoint(args.MainCamera, args.InitialMousePosition);
        var finalObject = GetSelectableObjectInPoint(args.MainCamera, args.FinalMousePosition);
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
            item.Selected = status;
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

}

