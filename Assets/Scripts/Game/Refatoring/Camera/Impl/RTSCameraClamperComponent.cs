using UnityEngine;

namespace RTSEngine.Refactoring
{
    public abstract class RTSCameraClamperComponent : MonoBehaviour, IRTSCameraClamper
    {
        public abstract Vector3 ClampCameraPos(Transform cameraTransform);
    }
}