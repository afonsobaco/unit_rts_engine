using UnityEngine;

namespace RTSEngine.RTSCamera
{
    public interface IRTSCameraClamper
    {
        Vector3 ClampCameraPos(Transform cameraTransform);
    }
}