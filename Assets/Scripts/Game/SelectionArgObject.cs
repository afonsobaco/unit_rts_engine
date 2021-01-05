using UnityEngine;
public class SelectionArgObject
{
    private Camera mainCamera;
    private Vector3 initialMousePosition;
    private Vector3 finalMousePosition;
    private RectTransform selectionBox;

    public Camera MainCamera { get => mainCamera; set => mainCamera = value; }
    public Vector3 InitialMousePosition { get => initialMousePosition; set => initialMousePosition = value; }
    public Vector3 FinalMousePosition { get => finalMousePosition; set => finalMousePosition = value; }
    public RectTransform SelectionBox { get => selectionBox; set => selectionBox = value; }
}


