using UnityEngine;

namespace RTSEngine.Manager
{
    public interface ICameraManager
    {

        ICameraSettings CameraSettings { get; set; }
        Vector3 Origin { get; set; }
        bool IsCentering { get; set; }
        bool IsPanning { get; set; }

        Vector3 ClampCameraPos(UnityEngine.Camera mainCamera);
        Vector3 DoCameraCentering(UnityEngine.Camera mainCamera);
        Vector3 DoCameraInputMovement(float horizontal, float vertical, Vector3 mousePosition, float deltaTime, UnityEngine.Camera mainCamera);
        Vector3 DoCameraPanning(Vector2 mouseAxis, float deltaTime, UnityEngine.Camera mainCamera);
        Vector3 DoCameraZooming(float y, float deltaTime, UnityEngine.Camera mainCamera);
    }


}