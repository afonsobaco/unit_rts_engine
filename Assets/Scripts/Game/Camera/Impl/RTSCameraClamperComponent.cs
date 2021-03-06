using UnityEngine;

namespace RTSEngine.RTSCamera
{
    public abstract class RTSCameraClamperComponent : MonoBehaviour, IRTSCameraClamper
    {
        public abstract Vector3 ClampCameraPos(Transform cameraTransform);
    }
}