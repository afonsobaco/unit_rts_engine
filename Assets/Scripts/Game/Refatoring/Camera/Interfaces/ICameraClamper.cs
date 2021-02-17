using UnityEngine;

namespace RTSEngine.Refactoring
{
    public interface ICameraClamper
    {
        Vector3 ClampCameraPos(Transform cameraTransform);
    }
}