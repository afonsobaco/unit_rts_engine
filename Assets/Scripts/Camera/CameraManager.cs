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
        var mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
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
        mainCamera.transform.position += new Vector3(0, 0, value * cameraSpeed * Time.deltaTime);

    }

    private void MoveCameraHorizontal(float value)
    {
        mainCamera.transform.position += new Vector3(value * cameraSpeed * Time.deltaTime, 0, 0);

    }

    private void ZoomCamera(float y)
    {
        Vector3 vZoom = mainCamera.transform.position + transform.forward * (zoomScale * Time.deltaTime * y);
        if (vZoom.y < minZoom)
        {
            vZoom = new Vector3(mainCamera.transform.position.x, minZoom, mainCamera.transform.position.z);
        }
        else if (vZoom.y > maxZoom)
        {
            vZoom = new Vector3(mainCamera.transform.position.x, maxZoom, mainCamera.transform.position.z);
        }
        mainCamera.transform.position = vZoom;
    }

    private void CenterCameraToPosition()
    {
        Vector3 midPoint = SelectionManager.Instance.GetSelectionMainPoint();
        float z = midPoint.z - (mainCamera.transform.position.y * Mathf.Tan((90 - mainCamera.transform.rotation.eulerAngles.x) * Mathf.Deg2Rad));
        var pos = new Vector3(midPoint.x, mainCamera.transform.position.y, (float)z);
        if (pos != Vector3.zero)
            mainCamera.transform.position = pos;
    }
}
