using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class RTSCameraManager
    {
        private IRTSCameraClamper _clamper;

        public RTSCameraManager(IRTSCameraClamper clamper)
        {
            _clamper = clamper;
        }

        public Vector3 DoCameraMovement(Transform cameraTransform, Vector3 desiredMovement)
        {
            cameraTransform.position += desiredMovement;
            return _clamper.ClampCameraPos(cameraTransform);
        }

        public Vector3 DoCameraPan(Transform cameraTransform, Vector3 desiredPan)
        {
            desiredPan = Quaternion.Euler(new Vector3(0f, cameraTransform.rotation.eulerAngles.y, 0f)) * desiredPan;
            desiredPan = cameraTransform.InverseTransformDirection(desiredPan);
            cameraTransform.Translate(desiredPan, Space.Self);
            return _clamper.ClampCameraPos(cameraTransform);
        }

        public Vector3 DoCameraZoom(Transform cameraTransform, Vector3 desiredZoom)
        {
            float deltaTime = Time.deltaTime;
            cameraTransform.position += desiredZoom;
            return _clamper.ClampCameraPos(cameraTransform);
        }
    }
}
