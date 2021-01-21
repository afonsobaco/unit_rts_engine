using UnityEngine;

namespace RTSEngine.Manager.Interfaces
{
    public interface ICameraManager
    {
        Vector3 Origin { get; set; }
        bool IsPanning { get; set; }
        bool IsCentering { get; set; }
        ICameraSettings Settings { get; set; }

        Vector3 DoCameraCentering(UnityEngine.Camera mainCamera);
        Vector3 DoCameraMovement(float horizontal, float vertical, Vector3 mousePosition, float deltaTime, UnityEngine.Camera mainCamera);
        Vector3 DoCameraPanning(Vector2 mouseAxis, float deltaTime, UnityEngine.Camera mainCamera);
        Vector3 DoCameraZoom(float y, float deltaTime, UnityEngine.Camera mainCamera);
    }


}