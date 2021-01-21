using UnityEngine;
using RTSEngine.Core.Impls;
using RTSEngine.Manager.Interfaces;
using RTSEngine.Core.Enums;
using RTSEngine.Manager.Enums;


namespace RTSEngine.Manager.Impls
{
    public class CameraManager : ICameraManager
    {

        private ISelectionManager<SelectableObject, SelectionTypeEnum, ObjectTypeEnum> selectionManager;

        private Vector3 origin;
        private bool isPanning;
        private float yPosMagicNumber = 7.08f;
        private bool isCentering;
        private ICameraSettings settings;

        public Vector3 Origin { get => origin; set => origin = value; }
        public bool IsPanning { get => isPanning; set => isPanning = value; }
        public bool IsCentering { get => isCentering; set => isCentering = value; }
        public ICameraSettings Settings { get => settings; set => settings = value; }

        public CameraManager(ISelectionManager<SelectableObject, SelectionTypeEnum, ObjectTypeEnum> selectionManager)
        {
            this.selectionManager = selectionManager;
        }

        public Vector3 DoCameraCentering(UnityEngine.Camera mainCamera)
        {
            if (selectionManager.CurrentSelection.Count != 0)
            {
                Vector3 midPoint = selectionManager.GetSelectionMainPoint();
                float z = midPoint.z - GetCameraZDistance(mainCamera);
                return new Vector3(midPoint.x, mainCamera.transform.position.y, (float)z);
            }
            return mainCamera.transform.position;
        }

        public Vector3 DoCameraInputMovement(float horizontal, float vertical, Vector3 mousePosition, float deltaTime, UnityEngine.Camera mainCamera)
        {
            if ((Mathf.Abs(horizontal) > Settings.AxisPressure || Mathf.Abs(vertical) > Settings.AxisPressure))
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
            desiredMove *= Settings.PanSpeed * mainCamera.transform.position.y;
            desiredMove *= deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, mainCamera.transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = mainCamera.transform.InverseTransformDirection(desiredMove);
            return desiredMove;
        }


        public Vector3 DoCameraZooming(float y, float deltaTime, UnityEngine.Camera mainCamera)
        {
            Vector3 vZoom = mainCamera.transform.position + mainCamera.transform.forward * (Settings.ZoomScale * deltaTime * y);
            if (vZoom.y < Settings.MinZoom)
            {
                var diff = Settings.MinZoom - vZoom.y;
                vZoom = new Vector3(mainCamera.transform.position.x, Settings.MinZoom, mainCamera.transform.position.z + diff);
            }
            else if (vZoom.y > Settings.MaxZoom)
            {
                var diff = vZoom.y - Settings.MaxZoom;
                vZoom = new Vector3(mainCamera.transform.position.x, Settings.MaxZoom, mainCamera.transform.position.z - diff);
            }
            return vZoom;
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
            if (zDistance > Settings.SizeFromMidPoint / 2)
            {
                zDistance = 0;
            }
            float clampedX = Mathf.Clamp(mainCamera.transform.position.x, -Settings.SizeFromMidPoint + (zDistance), Settings.SizeFromMidPoint - (zDistance));
            float clampedZ = Mathf.Clamp(mainCamera.transform.position.z, -Settings.SizeFromMidPoint - (zDistance / 2), Settings.SizeFromMidPoint - zDistance);
            return new Vector3(clampedX, mainCamera.transform.position.y, clampedZ);
        }

        private Vector3 DoMouseCameraMovement(Vector3 mousePosition, float deltaTime, UnityEngine.Camera mainCamera)
        {
            var mousePos = mainCamera.ScreenToViewportPoint(mousePosition);
            if (mousePos.x >= 0 && mousePos.x <= 1 && mousePos.y >= 0 && mousePos.y <= 1)
            {
                float x = DoMouseMovement(mousePos.x, mainCamera.transform.position.y, deltaTime);
                float z = DoMouseMovement(mousePos.y, mainCamera.transform.position.y, deltaTime);
                return new Vector3(x, 0f, z);
            }
            return Vector3.zero;
        }

        private float DoMouseMovement(float position, float yPos, float deltaTime)
        {
            if (position >= 0 && position < (Settings.BoundriesOffset))
            {
                return MoveCamera(-1, yPos, deltaTime);
            }
            else if (position <= 1 && position > (1 - Settings.BoundriesOffset))
            {
                return MoveCamera(1, yPos, deltaTime);
            }
            return 0f;
        }

        public float MoveCamera(float value, float yPos, float deltaTime)
        {
            var speed = (yPos * Settings.CameraSpeed) + yPosMagicNumber; //magic number!
            return value * speed * deltaTime;
        }

        public float GetCameraZDistance(UnityEngine.Camera mainCamera)
        {
            float angle = 90 - mainCamera.transform.rotation.eulerAngles.x;
            angle = Mathf.Clamp(angle, 0, 80); // prevent weird angles
            return (mainCamera.transform.position.y * Mathf.Tan(angle * Mathf.Deg2Rad));
        }

    }


}