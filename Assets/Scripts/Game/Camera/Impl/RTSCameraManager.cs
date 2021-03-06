using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.RTSCamera
{
    public class RTSCameraManager
    {
        private IRTSCameraClamper _clamper;

        //TODO test
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

        public Vector3 DoCameraCenter(Transform cameraTransform, Vector3 desiredPosition)
        {
            float z = desiredPosition.z - GetCameraZDistance(cameraTransform);
            cameraTransform.position = new Vector3(desiredPosition.x, cameraTransform.position.y, (float)z);
            return _clamper.ClampCameraPos(cameraTransform);
        }

        private float GetCameraZDistance(Transform cameraTransform)
        {
            float angle = 90 - cameraTransform.rotation.eulerAngles.x;
            angle = Mathf.Clamp(angle, 0, 80); // prevent weird angles
            return (cameraTransform.position.y * Mathf.Tan(angle * Mathf.Deg2Rad));
        }
    }
}
