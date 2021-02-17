using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public class RTSCameraSignalManager
    {
        private RTSCameraManager _cameraInterface;
        private RTSCamera _camera;

        public RTSCameraSignalManager(RTSCameraManager cameraInterface, RTSCamera camera)
        {
            this._cameraInterface = cameraInterface;
            this._camera = camera;
        }

        public void OnCameraMoveSignal(CameraMoveSignal signal)
        {
            Transform transform = Camera.main.transform;
            var desiredMovement = _camera.GetCameraMovement(signal.Horizontal, signal.Vertical, transform.position.y, Time.deltaTime);
            transform.position = _cameraInterface.DoCameraMovement(transform, desiredMovement);
        }

        public void OnCameraPanSignal(CameraPanSignal signal)
        {
            Transform transform = Camera.main.transform;
            var desiredPan = _camera.GetCameraPan(signal.MouseAxis, transform.position.y, Time.deltaTime);
            transform.position = _cameraInterface.DoCameraPan(transform, desiredPan);
        }

        public void OnCameraZoomSignal(CameraZoomSignal signal)
        {
            Transform transform = Camera.main.transform;
            var desiredZoom = _camera.GetCameraZoom(signal.Zoom, transform.forward, Time.deltaTime);
            transform.position = _cameraInterface.DoCameraZoom(transform, desiredZoom);
        }

    }
}
