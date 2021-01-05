using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private RectTransform selectionBox;
    private SelectionArgObject selectionArgObject;
    private Dictionary<KeyCode, int> groupKeys = new Dictionary<KeyCode, int>()
    {
        {KeyCode.Alpha1, 1},
        {KeyCode.Alpha2, 2},
        {KeyCode.Alpha3, 3},
        {KeyCode.Alpha4, 4},
        {KeyCode.Alpha5, 5},
        {KeyCode.Alpha6, 6},
        {KeyCode.Alpha7, 7},
        {KeyCode.Alpha8, 8},
        {KeyCode.Alpha9, 9},
        {KeyCode.Alpha0, 10}
    };


    void Start()
    {
        selectionArgObject = new SelectionArgObject();
        selectionArgObject.MainCamera = mainCamera;
        selectionArgObject.SelectionBox = selectionBox;
        selectionArgObject.SelectionBox.gameObject.SetActive(false);

    }

    void Update()
    {
        DoSelection();

        DoGroupSelection();
    }

    private void DoGroupSelection()
    {
        int keyPressed = getAnyGroupKeyPressed();
        if (keyPressed > 0)
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                SelectionManager.Instance.SetGroup(keyPressed);
            }
            else
            {
                SelectionManager.Instance.GetGroup(keyPressed);
            }
        }
    }

    private int getAnyGroupKeyPressed()
    {
        foreach (KeyValuePair<KeyCode, int> entry in groupKeys)
        {
            if (Input.GetKeyDown(entry.Key))
            {
                return entry.Value;
            }
        }
        return 0;
    }

    private void DoSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionArgObject.InitialMousePosition = Input.mousePosition;
            selectionArgObject.SelectionBox.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            selectionArgObject.FinalMousePosition = Input.mousePosition;
            SelectionManager.Instance.DoSelectionPreview(selectionArgObject);
            DrawSelectionBox();
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectionManager.Instance.DoSelection(selectionArgObject);
            selectionArgObject.SelectionBox.gameObject.SetActive(false);
        }

    }

    private void DrawSelectionBox()
    {
        var size = new Vector2(Mathf.Abs(selectionArgObject.InitialMousePosition.x - selectionArgObject.FinalMousePosition.x), Mathf.Abs(selectionArgObject.InitialMousePosition.y - selectionArgObject.FinalMousePosition.y));
        var center = new Vector2(Mathf.Abs(selectionArgObject.InitialMousePosition.x + selectionArgObject.FinalMousePosition.x) / 2, Mathf.Abs(selectionArgObject.InitialMousePosition.y + selectionArgObject.FinalMousePosition.y) / 2);
        selectionArgObject.SelectionBox.position = center;
        selectionArgObject.SelectionBox.sizeDelta = size;
    }

}
