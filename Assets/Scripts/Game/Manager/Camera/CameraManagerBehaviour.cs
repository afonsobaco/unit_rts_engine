using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.RTSCamera;
using Zenject;

namespace RTSEngine.Manager
{

    public class CameraManagerBehaviour : MonoBehaviour
    {

        private ICameraManager manager;
        public CameraSettings settings;

        public ICameraManager Manager { get => manager; private set => manager = value; }

        [Inject]
        public void Construct(ICameraManager manager)
        {
            this.Manager = manager;
            this.Manager.Settings = settings;
        }


    }

    public interface ICameraManager
    {
        Vector3 Origin { get; set; }
        bool IsPanning { get; set; }
        bool IsCentering { get; set; }
        ICameraSettings Settings { get; set; }

        Vector3 DoCameraMovement(float horizontal, float vertical, Vector3 mousePosition, float deltaTime, UnityEngine.Camera mainCamera);
        Vector3 CenterCameraToSelection(UnityEngine.Camera mainCamera);
        Vector3 DoCameraPanning(Vector2 mouseAxis, UnityEngine.Camera mainCamera);
        Vector3 ZoomCamera(float y, UnityEngine.Camera mainCamera);
    }

    public class CameraManager : ICameraManager
    {

        private ISelectionManager<SelectableObject> selectionManager;


        private Vector3 origin;
        private bool isPanning;
        private float yPosMagicNumber = 7.08f;
        private bool isCentering;
        private ICameraSettings settings;

        public Vector3 Origin { get => origin; set => origin = value; }
        public bool IsPanning { get => isPanning; set => isPanning = value; }
        public bool IsCentering { get => isCentering; set => isCentering = value; }
        public ICameraSettings Settings { get => settings; set => settings = value; }

        public CameraManager(ISelectionManager<SelectableObject> selectionManager)
        {
            this.selectionManager = selectionManager;
        }

        public Vector3 DoCameraMovement(float horizontal, float vertical, Vector3 mousePosition, float deltaTime, UnityEngine.Camera mainCamera)
        {
            Vector3 result = Vector3.zero;
            if (isCentering)
            {
                result = CenterCameraToSelection(mainCamera);
            }
            else if (!isPanning)
            {
                if ((Mathf.Abs(horizontal) > Settings.AxisPressure || Mathf.Abs(vertical) > Settings.AxisPressure))
                {
                    result = DoAxisCameraMovement(horizontal, vertical, deltaTime, mainCamera);
                }
                else
                {
                    result = DoMouseCameraMovement(mousePosition, deltaTime, mainCamera);
                }

                result += mainCamera.transform.position;
            }
            return ClampCameraPos(mainCamera, result);

        }

        public Vector3 DoCameraPanning(Vector2 mouseAxis, UnityEngine.Camera mainCamera)
        {
            if (!isPanning || IsCentering)
            {
                return Vector3.zero;
            }

            Vector3 desiredMove = new Vector3(-mouseAxis.x, 0, -mouseAxis.y);

            desiredMove *= (Settings.PanSpeed * mainCamera.transform.position.y);
            desiredMove *= Time.deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, mainCamera.transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = mainCamera.transform.InverseTransformDirection(desiredMove);
            return desiredMove;
        }

        public Vector3 DoAxisCameraMovement(float horizontal, float vertical, float deltaTime, UnityEngine.Camera mainCamera)
        {
            float horizontalMovement = MoveCamera(horizontal, mainCamera.transform.position.y, deltaTime);
            float verticalMovement = MoveCamera(vertical, mainCamera.transform.position.y, deltaTime);

            return new Vector3(horizontalMovement, 0, verticalMovement);
        }

        public Vector3 ClampCameraPos(Camera mainCamera, Vector3 finalPos)
        {
            float zDistance = GetCameraZDistance(mainCamera);
            float clampedX = Mathf.Clamp(finalPos.x, zDistance - Settings.SizeFromMidPoint, Settings.SizeFromMidPoint - zDistance);
            float clampedZ = Mathf.Clamp(finalPos.z, -Settings.SizeFromMidPoint - zDistance / 2, Settings.SizeFromMidPoint - zDistance);
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

        public float MoveCamera(float value, float yPos, float time)
        {
            var speed = (yPos * Settings.CameraSpeed) + yPosMagicNumber; //magic number!
            return value * speed * time;
        }

        public Vector3 ZoomCamera(float y, UnityEngine.Camera mainCamera)
        {
            Vector3 vZoom = mainCamera.transform.position + mainCamera.transform.forward * (Settings.ZoomScale * Time.deltaTime * y);
            if (vZoom.y < Settings.MinZoom)
            {
                vZoom = new Vector3(mainCamera.transform.position.x, Settings.MinZoom, mainCamera.transform.position.z);
            }
            else if (vZoom.y > Settings.MaxZoom)
            {
                vZoom = new Vector3(mainCamera.transform.position.x, Settings.MaxZoom, mainCamera.transform.position.z);
            }
            return vZoom;
        }

        public Vector3 CenterCameraToSelection(UnityEngine.Camera mainCamera)
        {
            if (selectionManager.CurrentSelection.Count != 0)
            {
                Vector3 midPoint = selectionManager.GetSelectionMainPoint();
                float z = midPoint.z - GetCameraZDistance(mainCamera);
                return new Vector3(midPoint.x, mainCamera.transform.position.y, (float)z);
            }
            return mainCamera.transform.position;
        }

        public float GetCameraZDistance(UnityEngine.Camera mainCamera)
        {
            float angle = 90 - mainCamera.transform.rotation.eulerAngles.x;
            angle = Mathf.Clamp(angle, 0, 80); // prevent weird angles
            return (mainCamera.transform.position.y * Mathf.Tan(angle * Mathf.Deg2Rad));
        }

    }


}