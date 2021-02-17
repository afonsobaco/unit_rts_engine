using UnityEngine;

namespace RTSEngine.Refactoring
{
    public abstract class RTSCameraClamperComponent : MonoBehaviour, ICameraClamper
    {
        public abstract Vector3 ClampCameraPos(Transform cameraTransform);
    }
}