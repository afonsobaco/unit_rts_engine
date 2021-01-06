using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public KeyCode selectionKeyCode = KeyCode.LeftControl;
    public KeyCode groupKeyCode = KeyCode.LeftControl;

    public Camera mainCamera;
    [SerializeField]
    private RectTransform selectionBox;
    private SelectionArgObject selectionArgObject;
    private Vector2 initialMousePosition;
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
        selectionBox.gameObject.SetActive(false);

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
            if (Input.GetKey(groupKeyCode))
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
            selectionArgObject.SizeDelta = Vector2.zero;
            selectionBox.gameObject.SetActive(true);
            initialMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            SelectionManager.Instance.DoSelectionPreview(selectionArgObject);
            DrawSelectionBox();
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectionArgObject.MultipleSelection = Input.GetKey(selectionKeyCode);
            SelectionManager.Instance.DoSelection(selectionArgObject);
            selectionBox.gameObject.SetActive(false);
        }

    }

    private void DrawSelectionBox()
    {
        Vector3 finalMousePosition = Input.mousePosition;
        var size = new Vector2(Mathf.Abs(initialMousePosition.x - finalMousePosition.x), Mathf.Abs(initialMousePosition.y - finalMousePosition.y));
        var center = new Vector2(Mathf.Abs(initialMousePosition.x + finalMousePosition.x) / 2, Mathf.Abs(initialMousePosition.y + finalMousePosition.y) / 2);
        selectionBox.position = center;
        selectionBox.sizeDelta = size;
        selectionArgObject.Position = selectionBox.position;
        selectionArgObject.SizeDelta = selectionBox.sizeDelta;

    }

}
