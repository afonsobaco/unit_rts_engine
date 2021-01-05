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

    void Start()
    {
        selectionArgObject = new SelectionArgObject();
        selectionArgObject.MainCamera = mainCamera;
        selectionArgObject.SelectionBox = selectionBox;
        selectionArgObject.SelectionBox.gameObject.SetActive(false);

    }

    void LateUpdate()
    {
        DoSelection();
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
