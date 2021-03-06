﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Signal;

namespace RTSEngine.RTSCamera
{
    public class RTSCameraSignalManager
    {
        private RTSCameraManager _cameraManager;
        private RTSCamera _camera;

        public RTSCameraSignalManager(RTSCameraManager cameraManager, RTSCamera camera)
        {
            this._cameraManager = cameraManager;
            this._camera = camera;
        }

        public void OnCameraMoveSignal(CameraMoveSignal signal)
        {
            Transform transform = Camera.main.transform;
            var desiredMovement = _camera.GetCameraMovement(signal.Horizontal, signal.Vertical, transform.position.y, Time.deltaTime);
            transform.position = _cameraManager.DoCameraMovement(transform, desiredMovement);
        }

        public void OnCameraPanSignal(CameraPanSignal signal)
        {
            Transform transform = Camera.main.transform;
            var desiredPan = _camera.GetCameraPan(signal.MouseAxis, transform.position.y, Time.deltaTime);
            transform.position = _cameraManager.DoCameraPan(transform, desiredPan);
        }

        public void OnCameraZoomSignal(CameraZoomSignal signal)
        {
            Transform transform = Camera.main.transform;
            var desiredZoom = _camera.GetCameraZoom(signal.Zoom, transform.forward, Time.deltaTime);
            transform.position = _cameraManager.DoCameraZoom(transform, desiredZoom);
        }
        public void OnCameraGoToPositionSignal(CameraGoToPositionSignal signal)
        {
            Transform transform = Camera.main.transform;
            transform.position = _cameraManager.DoCameraCenter(transform, signal.Position);
        }

    }
}
