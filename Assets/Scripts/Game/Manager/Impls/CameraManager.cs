using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace RTSEngine.Manager
{
    public class CameraManager : ICameraManager, IInitializable
    {

        private ICameraSettings _settings;
        private bool _isPanning;
        private float yPosMagicNumber = 7.08f;
        private bool _isCentering;
        private bool _canMove;
        private Camera _mainCamera;
        private ISelectableObject _pointOfInterest;

        public void OnSelectionChange(PrimaryObjectSelectedSignal signal)
        {
            SetPointOfInterest(signal.Selectable);
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

        public void SetCameraSettings(ICameraSettings value)
        {
            _settings = value;
        }

        public void Initialize()
        {
            this._mainCamera = Camera.main;
        }

        public void DoSelectedPortraitClick(SelectedPortraitClickSignal signal)
        {
            SetPointOfInterest(signal.Selectable);
            SetIsCentering(signal.Type == KeyButtonType.PRESSED);
        }

        public Vector3 DoCameraCentering()
        {
            if (this._pointOfInterest == null)
            {
                return _mainCamera.transform.position;
            }
            float z = this._pointOfInterest.Position.z - GetCameraZDistance();
            return new Vector3(this._pointOfInterest.Position.x, _mainCamera.transform.position.y, (float)z);
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
            if ((Mathf.Abs(horizontal) > _settings.AxisPressure || Mathf.Abs(vertical) > _settings.AxisPressure))
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
            desiredMove *= _settings.PanSpeed * _mainCamera.transform.position.y;
            desiredMove *= deltaTime;
            desiredMove = Quaternion.Euler(new Vector3(0f, _mainCamera.transform.eulerAngles.y, 0f)) * desiredMove;
            desiredMove = _mainCamera.transform.InverseTransformDirection(desiredMove);
            return desiredMove;
        }

        public Vector3 DoCameraZooming(float y, float deltaTime)
        {
            Vector3 vZoom = CalcVZoom(_mainCamera.transform.position, _mainCamera.transform.forward, _settings.ZoomScale * deltaTime * y);
            if (vZoom.y < _settings.MinZoom)
            {
                vZoom = clampZoomOnY(_mainCamera.transform.position, _mainCamera.transform.forward, _settings.MinZoom);
            }
            else if (vZoom.y > _settings.MaxZoom)
            {
                vZoom = clampZoomOnY(_mainCamera.transform.position, _mainCamera.transform.forward, _settings.MaxZoom);
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
            if (zDistance > _settings.SizeFromMidPoint / 2)
            {
                zDistance = 0;
            }
            float clampedX = Mathf.Clamp(_mainCamera.transform.position.x, -_settings.SizeFromMidPoint + (zDistance), _settings.SizeFromMidPoint - (zDistance));
            float clampedZ = Mathf.Clamp(_mainCamera.transform.position.z, -_settings.SizeFromMidPoint - (zDistance / 2), _settings.SizeFromMidPoint - zDistance);
            return new Vector3(clampedX, _mainCamera.transform.position.y, clampedZ);
        }

        private Vector3 DoMouseCameraMovement(Vector3 mousePosition, float deltaTime)
        {
            //TODO adjust
            if (!_canMove)
            {
                var mousePos = _mainCamera.ScreenToViewportPoint(mousePosition);
                if (mousePos.x >= 0 && mousePos.x <= 1 && mousePos.y >= 0 && mousePos.y <= 1)
                {
                    float x = DoMouseMovement(mousePos.x, _mainCamera.transform.position.y, deltaTime);
                    float z = DoMouseMovement(mousePos.y, _mainCamera.transform.position.y, deltaTime);
                    return new Vector3(x, 0f, z);
                }
            }
            return Vector3.zero;
        }

        private float DoMouseMovement(float position, float yPos, float deltaTime)
        {
            if (position >= 0 && position < (_settings.BoundriesOffset))
            {
                return MoveCamera(-1, yPos, deltaTime);
            }
            else if (position <= 1 && position > (1 - _settings.BoundriesOffset))
            {
                return MoveCamera(1, yPos, deltaTime);
            }
            return 0f;
        }

        public float MoveCamera(float value, float yPos, float deltaTime)
        {
            var speed = (yPos * _settings.CameraSpeed) + yPosMagicNumber; //magic number!
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
            return _mainCamera.ViewportToScreenPoint(_settings.MinViewportPoint);
        }

        public Vector3 GetMaxScreenBoundries()
        {
            return _mainCamera.ViewportToScreenPoint(_settings.MaxViewportPoint);
        }

        public void SetCanMove(bool value)
        {
            this._canMove = value;
        }

        public void CanMoveSignal(CanMoveSignal signal)
        {
            this._canMove = signal.Value;
        }

        public void SetPointOfInterest(ISelectableObject selected)
        {
            this._pointOfInterest = selected;
        }
    }


}