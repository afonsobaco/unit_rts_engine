using UnityEngine;
public class SelectionArgObject
{
    private Camera mainCamera;
    private Vector2 sizeDelta;
    private Vector2 position;
    private Vector2 min;
    private Vector2 max;

    private bool multipleSelection;

    public Vector2 SizeDelta
    {
        get { return sizeDelta; }
        set
        {
            sizeDelta = value;
            Min = GetMin();
            Max = GetMax();
        }
    }

    private Vector2 GetMax()
    {
        return new Vector2(position.x + (sizeDelta.x / 2), position.y + (sizeDelta.y / 2));
    }

    private Vector2 GetMin()
    {
        return new Vector2(position.x - (sizeDelta.x / 2), position.y - (sizeDelta.y / 2));
    }

    public Vector2 Position
    {
        get { return position; }
        set
        {
            position = value;
            Min = new Vector2(position.x - (sizeDelta.x / 2), position.y - (sizeDelta.y / 2));
            Max = new Vector2(position.x + (sizeDelta.x / 2), position.y + (sizeDelta.y / 2));
        }
    }
    public Camera MainCamera { get => mainCamera; set => mainCamera = value; }
    public Vector2 Min { get => min; set => min = value; }
    public Vector2 Max { get => max; set => max = value; }
    public bool MultipleSelection { get => multipleSelection; set => multipleSelection = value; }
}


