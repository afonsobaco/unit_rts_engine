using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{


    public int cameraSpeed;

    public float boundriesOffset;
    public float zoomScale;
    public float minZoom;
    public float maxZoom;
    public float axisPressure = 0.3f;

    public RectTransform selectionBox;
    public Vector3 InitialMousePosition { get; private set; }


    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        //If space is pressed, ignore camera controls
        if (Input.GetKey(KeyCode.Space))
        {
            CenterCameraToPosition();
        }
        else
        {
            DoCameraMovement();
        }

        DoCameraZoom();

        DoSelection();
    }

    private void DoSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InitialMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            DrawSelectionBox();
            SelectionManager.Instance.DoSelectionPreview(getSelectionArgs());
        }

        if (Input.GetMouseButtonUp(0))
        {
            SelectionManager.Instance.DoSelection(getSelectionArgs());
            selectionBox.sizeDelta = new Vector2(0, 0);
        }

    }

    private SelectionArgObject getSelectionArgs()
    {
        var args = new SelectionArgObject();
        args.MainCamera = mainCamera;
        args.InitialMousePosition = InitialMousePosition;
        args.FinalMousePosition = Input.mousePosition;
        args.SelectionBox = selectionBox;
        return args;
    }

    private void DrawSelectionBox()
    {
        Vector3 finalMousePosition = Input.mousePosition;
        var size = new Vector2(Mathf.Abs(InitialMousePosition.x - finalMousePosition.x), Mathf.Abs(InitialMousePosition.y - finalMousePosition.y));
        var center = new Vector2(Mathf.Abs(InitialMousePosition.x + finalMousePosition.x) / 2, Mathf.Abs(InitialMousePosition.y + finalMousePosition.y) / 2);
        selectionBox.position = center;
        selectionBox.sizeDelta = size;
    }

    private void DoCameraZoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            ZoomCamera(Input.mouseScrollDelta.y);
        }
    }

    private void DoCameraMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > axisPressure || Mathf.Abs(vertical) > axisPressure)
        {
            DoAxisCameraMovement(horizontal, vertical);
        }
        else
        {
            DoMouseCameraMovement();
        }
    }

    private void DoAxisCameraMovement(float horizontal, float vertical)
    {
        MoveCameraHorizontal(horizontal);
        MoveCameraVertical(vertical);
    }

    private void DoMouseCameraMovement()
    {
        var mousePos = mainCamera.ScreenToViewportPoint(Input.mousePosition);
        if (mousePos.x >= 0 && mousePos.x <= 1 && mousePos.y >= 0 && mousePos.y <= 1)
        {
            DoMouseCameraMovementBy(mousePos);
        }
    }

    private void DoMouseCameraMovementBy(Vector3 mousePos)
    {
        DoHorizontalMouseMovement(mousePos);
        DoVerticalMouseMovement(mousePos);
    }

    private void DoVerticalMouseMovement(Vector3 mousePos)
    {
        if (mousePos.y >= 0 && mousePos.y < (boundriesOffset))
        {
            MoveCameraVertical(-1);
        }
        else if (mousePos.y <= 1 && mousePos.y > (1 - boundriesOffset))
        {
            MoveCameraVertical(1);
        }
    }

    private void DoHorizontalMouseMovement(Vector3 mousePos)
    {
        if (mousePos.x >= 0 && mousePos.x < (boundriesOffset))
        {
            MoveCameraHorizontal(-1);
        }
        else if (mousePos.x <= 1 && mousePos.x > (1 - boundriesOffset))
        {
            MoveCameraHorizontal(1);
        }
    }

    private void MoveCameraVertical(float value)
    {
        this.transform.position += new Vector3(0, 0, value * cameraSpeed * Time.deltaTime);

    }

    private void MoveCameraHorizontal(float value)
    {
        this.transform.position += new Vector3(value * cameraSpeed * Time.deltaTime, 0, 0);

    }

    private void ZoomCamera(float y)
    {
        Vector3 vZoom = this.transform.position + transform.forward * (zoomScale * Time.deltaTime * y);
        if (vZoom.y < minZoom)
        {
            vZoom = new Vector3(this.transform.position.x, minZoom, this.transform.position.z);
        }
        else if (vZoom.y > maxZoom)
        {
            vZoom = new Vector3(this.transform.position.x, maxZoom, this.transform.position.z);
        }
        this.transform.position = vZoom;
    }

    private void CenterCameraToPosition()
    {
        Vector3 pos = SelectionManager.Instance.GetSelectionMainPoint();
        if (pos != Vector3.zero)
            this.transform.position = pos;
    }


}
