using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class CameraInputManagerBehaviour : MonoBehaviour
    {
        private CameraManager cameraManager;


        [Inject]
        public void Construct(CameraManager cameraManager)
        {
            this.cameraManager = cameraManager;
        }

        public void LateUpdate()
        {
            this.SetCameraControls();
            this.SetCameraPanningControls();
            this.DoCameraMovement();
        }

        private void SetCameraControls()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                cameraManager.SetIsCentering(true);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                cameraManager.SetIsCentering(false);
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                Camera.main.transform.position = cameraManager.DoCameraZooming(Input.mouseScrollDelta.y, Time.deltaTime);
            }
        }

        private void DoCameraMovement()
        {
            if (cameraManager.IsCentering())
            {
                Camera.main.transform.position = cameraManager.DoCameraCentering();
            }
            else
            {
                if (cameraManager.IsPanning())
                {
                    var desired = cameraManager.DoCameraPanning(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), Time.deltaTime);
                    Camera.main.transform.Translate(desired, Space.Self);
                }
                else
                {
                    Camera.main.transform.position += cameraManager.DoCameraInputMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.mousePosition, Time.deltaTime);
                }
            }
            Camera.main.transform.position = cameraManager.ClampCameraPos();
        }

        private void SetCameraPanningControls()
        {
            if (Input.GetMouseButtonDown(2))
            {
                cameraManager.SetIsPanning(true);
            }
            if (Input.GetMouseButtonUp(2))
            {
                cameraManager.SetIsPanning(false);
            }
        }

    }
}
