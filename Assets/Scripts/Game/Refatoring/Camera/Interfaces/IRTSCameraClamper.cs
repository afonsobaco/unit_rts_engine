using UnityEngine;

namespace RTSEngine.Refactoring
{
    public interface IRTSCameraClamper
    {
        Vector3 ClampCameraPos(Transform cameraTransform);
    }
}