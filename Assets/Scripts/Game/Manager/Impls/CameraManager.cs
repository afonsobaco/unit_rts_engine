using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace RTSEngine.Manager
{
    public class CameraManager : ICameraManager, IInitializable
    {

        private ICameraSettings _settings;
        private Vector3 _origin;
        private bool _isPanning;
        private float yPosMagicNumber = 7.08f;
        private bool _isCentering;
        private Camera _mainCamera;

        public Vector3 GetOrigin()
        {
            return _origin;
        }

        public void SetOrigin(Vector3 value)
        {
            _origin = value;
        }

        public bool IsPanning()
        {
            return _isPanning;
        }

        public void SetIsPanning(bool value)
        {
            _isPanning = value;
        }

        public bool IsCentering()
        {
            return _isCentering;
        }

        public void SetIsCentering(bool value)
        {
            _isCentering = value;
        }

        public ICameraSettings GetCameraSettings()
        {
            return _settings;
        }

        public void SetCameraSettings(ICameraSettings value)
        {
            _settings = value;
        }

        public void Initialize()
        {
            this._mainCamera = Camera.main;
        }

        public void DoProfileInfoClick(SelectedPortraitClickSignal signal)
        {
            Debug.Log("ProfileInfo Clicked: " + signal.Selectable.Index);
            //TODO ajust
        }

        public Vector3 DoCameraCentering(Vector3 position)
        {
            float z = position.z - GetCameraZDistance();
            return new Vector3(position.x, _mainCamera.transform.position.y, (float)z);
        }

        private Vector3 GetSelectionMainPoint(HashSet<ISelectableObject> selectableObjectBehaviours)
        {
            if (selectableObjectBehaviours.Count == 0)
            {
                return new Vector3(_mainCamera.transform.position.x, 0, _mainCamera.transform.position.z);
            }
            return selectableObjectBehaviours.First().Position;
        }

        public Vector3 DoCameraInputMovement(float horizontal, float vertical, Vector3 mousePosition, float deltaTime)
        {
            if ((Mathf.Abs(horizontal) > GetCameraSettings().AxisPressure || Mathf.Abs(vertical) > GetCameraSettings().AxisPressure))
            {
                return DoAxisCameraMovement(horizontal, vertical, deltaTime);
            }
            else
            {
                return DoMouseCameraMovement(mousePosition, deltaTime);
            }
        }

        public Vector3 DoCameraPanning(Vector2 mouseAxis, float deltaTime)
        {
            Vector3 desiredMove = new Vector3(-mouseAxis.x, 0, -mouseAxis.y);
            desiredMove *= GetCameraSettings().PanSpeed * _mainCamera.transform.position.y;
            desiredMove *= deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, _mainCamera.transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = _mainCamera.transform.InverseTransformDirection(desiredMove);
            return desiredMove;
        }

        public Vector3 DoCameraZooming(float y, float deltaTime)
        {
            Vector3 vZoom = CalcVZoom(_mainCamera.transform.position, _mainCamera.transform.forward, GetCameraSettings().ZoomScale * deltaTime * y);
            if (vZoom.y < GetCameraSettings().MinZoom)
            {
                vZoom = clampZoomOnY(_mainCamera.transform.position, _mainCamera.transform.forward, GetCameraSettings().MinZoom);
            }
            else if (vZoom.y > GetCameraSettings().MaxZoom)
            {
                vZoom = clampZoomOnY(_mainCamera.transform.position, _mainCamera.transform.forward, GetCameraSettings().MaxZoom);
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

        public Vector3 DoAxisCameraMovement(float horizontal, float vertical, float deltaTime)
        {
            float horizontalMovement = MoveCamera(horizontal, _mainCamera.transform.position.y, deltaTime);
            float verticalMovement = MoveCamera(vertical, _mainCamera.transform.position.y, deltaTime);

            return new Vector3(horizontalMovement, 0, verticalMovement);
        }

        public Vector3 ClampCameraPos()
        {
            float zDistance = GetCameraZDistance();
            if (zDistance > GetCameraSettings().SizeFromMidPoint / 2)
            {
                zDistance = 0;
            }
            float clampedX = Mathf.Clamp(_mainCamera.transform.position.x, -GetCameraSettings().SizeFromMidPoint + (zDistance), GetCameraSettings().SizeFromMidPoint - (zDistance));
            float clampedZ = Mathf.Clamp(_mainCamera.transform.position.z, -GetCameraSettings().SizeFromMidPoint - (zDistance / 2), GetCameraSettings().SizeFromMidPoint - zDistance);
            return new Vector3(clampedX, _mainCamera.transform.position.y, clampedZ);
        }

        private Vector3 DoMouseCameraMovement(Vector3 mousePosition, float deltaTime)
        {
            //TODO adjust
            // if (!selectionManager.IsSelecting())
            // {
            //     var mousePos = _mainCamera.ScreenToViewportPoint(mousePosition);
            //     if (mousePos.x >= 0 && mousePos.x <= 1 && mousePos.y >= 0 && mousePos.y <= 1)
            //     {
            //         float x = DoMouseMovement(mousePos.x, _mainCamera.transform.position.y, deltaTime);
            //         float z = DoMouseMovement(mousePos.y, _mainCamera.transform.position.y, deltaTime);
            //         return new Vector3(x, 0f, z);
            //     }
            // }
            return Vector3.zero;
        }

        private float DoMouseMovement(float position, float yPos, float deltaTime)
        {
            if (position >= 0 && position < (GetCameraSettings().BoundriesOffset))
            {
                return MoveCamera(-1, yPos, deltaTime);
            }
            else if (position <= 1 && position > (1 - GetCameraSettings().BoundriesOffset))
            {
                return MoveCamera(1, yPos, deltaTime);
            }
            return 0f;
        }

        public float MoveCamera(float value, float yPos, float deltaTime)
        {
            var speed = (yPos * GetCameraSettings().CameraSpeed) + yPosMagicNumber; //magic number!
            return value * speed * deltaTime;
        }

        public float GetCameraZDistance()
        {
            float angle = 90 - _mainCamera.transform.rotation.eulerAngles.x;
            angle = Mathf.Clamp(angle, 0, 80); // prevent weird angles
            return (_mainCamera.transform.position.y * Mathf.Tan(angle * Mathf.Deg2Rad));
        }

        public Vector3 GetMinScreenBoundries()
        {
            return _mainCamera.ViewportToScreenPoint(GetCameraSettings().MinViewportPoint);
        }

        public Vector3 GetMaxScreenBoundries()
        {
            return _mainCamera.ViewportToScreenPoint(GetCameraSettings().MaxViewportPoint);
        }

    }


}