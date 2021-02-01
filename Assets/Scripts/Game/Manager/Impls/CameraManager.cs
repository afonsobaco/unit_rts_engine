using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace RTSEngine.Manager
{
    public class CameraManager : ICameraManager
    {

        private ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager;

        private Vector3 origin;
        private bool isPanning;
        private float yPosMagicNumber = 7.08f;
        private bool isCentering;
        private ICameraSettings settings;

        public Vector3 Origin { get => origin; set => origin = value; }
        public bool IsPanning { get => isPanning; set => isPanning = value; }
        public bool IsCentering { get => isCentering; set => isCentering = value; }
        public ICameraSettings CameraSettings { get => settings; set => settings = value; }

        public CameraManager(ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager)
        {
            this.selectionManager = selectionManager;
        }

        public Vector3 DoCameraCentering(UnityEngine.Camera mainCamera)
        {
            if (selectionManager.GetCurrentSelection().Count != 0)
            {
                Vector3 midPoint = GetSelectionMainPoint(mainCamera, selectionManager.GetCurrentSelection());
                float z = midPoint.z - GetCameraZDistance(mainCamera);
                return new Vector3(midPoint.x, mainCamera.transform.position.y, (float)z);
            }
            return mainCamera.transform.position;
        }

        private Vector3 GetSelectionMainPoint(UnityEngine.Camera mainCamera, HashSet<ISelectableObjectBehaviour> selectableObjectBehaviours)
        {
            if (selectableObjectBehaviours.Count == 0)
            {
                return new Vector3(mainCamera.transform.position.x, 0, mainCamera.transform.position.z);
            }
            return selectableObjectBehaviours.First().Position;
        }

        public Vector3 DoCameraInputMovement(float horizontal, float vertical, Vector3 mousePosition, float deltaTime, UnityEngine.Camera mainCamera)
        {
            if ((Mathf.Abs(horizontal) > CameraSettings.AxisPressure || Mathf.Abs(vertical) > CameraSettings.AxisPressure))
            {
                return DoAxisCameraMovement(horizontal, vertical, deltaTime, mainCamera);
            }
            else
            {
                return DoMouseCameraMovement(mousePosition, deltaTime, mainCamera);
            }
        }

        public Vector3 DoCameraPanning(Vector2 mouseAxis, float deltaTime, UnityEngine.Camera mainCamera)
        {
            Vector3 desiredMove = new Vector3(-mouseAxis.x, 0, -mouseAxis.y);
            desiredMove *= CameraSettings.PanSpeed * mainCamera.transform.position.y;
            desiredMove *= deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, mainCamera.transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = mainCamera.transform.InverseTransformDirection(desiredMove);
            return desiredMove;
        }

        public Vector3 DoCameraZooming(float y, float deltaTime, UnityEngine.Camera mainCamera)
        {
            Vector3 vZoom = CalcVZoom(mainCamera.transform.position, mainCamera.transform.forward, CameraSettings.ZoomScale * deltaTime * y);
            if (vZoom.y < CameraSettings.MinZoom)
            {
                vZoom = clampZoomOnY(mainCamera.transform.position, mainCamera.transform.forward, CameraSettings.MinZoom);
            }
            else if (vZoom.y > CameraSettings.MaxZoom)
            {
                vZoom = clampZoomOnY(mainCamera.transform.position, mainCamera.transform.forward, CameraSettings.MaxZoom);
            }
            return vZoom;
        }

        private static Vector3 CalcVZoom(Vector3 position, Vector3 forward, float k)
        {
            return position + (k * forward);
        }

        private static Vector3 clampZoomOnY(Vector3 position, Vector3 forward, float value)
        {
            var k = (value - position.y) / forward.y;
            return CalcVZoom(position, forward, k);
        }

        public Vector3 DoAxisCameraMovement(float horizontal, float vertical, float deltaTime, UnityEngine.Camera mainCamera)
        {
            float horizontalMovement = MoveCamera(horizontal, mainCamera.transform.position.y, deltaTime);
            float verticalMovement = MoveCamera(vertical, mainCamera.transform.position.y, deltaTime);

            return new Vector3(horizontalMovement, 0, verticalMovement);
        }

        public Vector3 ClampCameraPos(Camera mainCamera)
        {
            float zDistance = GetCameraZDistance(mainCamera);
            if (zDistance > CameraSettings.SizeFromMidPoint / 2)
            {
                zDistance = 0;
            }
            float clampedX = Mathf.Clamp(mainCamera.transform.position.x, -CameraSettings.SizeFromMidPoint + (zDistance), CameraSettings.SizeFromMidPoint - (zDistance));
            float clampedZ = Mathf.Clamp(mainCamera.transform.position.z, -CameraSettings.SizeFromMidPoint - (zDistance / 2), CameraSettings.SizeFromMidPoint - zDistance);
            return new Vector3(clampedX, mainCamera.transform.position.y, clampedZ);
        }

        private Vector3 DoMouseCameraMovement(Vector3 mousePosition, float deltaTime, UnityEngine.Camera mainCamera)
        {
            if (!selectionManager.IsSelecting())
            {
                var mousePos = mainCamera.ScreenToViewportPoint(mousePosition);
                if (mousePos.x >= 0 && mousePos.x <= 1 && mousePos.y >= 0 && mousePos.y <= 1)
                {
                    float x = DoMouseMovement(mousePos.x, mainCamera.transform.position.y, deltaTime);
                    float z = DoMouseMovement(mousePos.y, mainCamera.transform.position.y, deltaTime);
                    return new Vector3(x, 0f, z);
                }
            }
            return Vector3.zero;
        }

        private float DoMouseMovement(float position, float yPos, float deltaTime)
        {
            if (position >= 0 && position < (CameraSettings.BoundriesOffset))
            {
                return MoveCamera(-1, yPos, deltaTime);
            }
            else if (position <= 1 && position > (1 - CameraSettings.BoundriesOffset))
            {
                return MoveCamera(1, yPos, deltaTime);
            }
            return 0f;
        }

        public float MoveCamera(float value, float yPos, float deltaTime)
        {
            var speed = (yPos * CameraSettings.CameraSpeed) + yPosMagicNumber; //magic number!
            return value * speed * deltaTime;
        }

        public float GetCameraZDistance(UnityEngine.Camera mainCamera)
        {
            float angle = 90 - mainCamera.transform.rotation.eulerAngles.x;
            angle = Mathf.Clamp(angle, 0, 80); // prevent weird angles
            return (mainCamera.transform.position.y * Mathf.Tan(angle * Mathf.Deg2Rad));
        }

        public Vector3 GetMinScreenBoundries(UnityEngine.Camera mainCamera)
        {
            return mainCamera.ViewportToScreenPoint(CameraSettings.MinViewportPoint);
        }

        public Vector3 GetMaxScreenBoundries(UnityEngine.Camera mainCamera)
        {
            return mainCamera.ViewportToScreenPoint(CameraSettings.MaxViewportPoint);
        }

    }


}