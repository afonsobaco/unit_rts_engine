using UnityEngine;
namespace RTSEngine.Manager
{
    public interface ICameraManager
    {
        ICameraSettings GetCameraSettings();
        void SetCameraSettings(ICameraSettings value);
        Vector3 GetOrigin();
        void SetOrigin(Vector3 value);
        bool IsCentering();
        void SetIsCentering(bool value);
        bool IsPanning();
        void SetIsPanning(bool value);
        Vector3 DoAxisCameraMovement(float horizontal, float vertical, float deltaTime);
        Vector3 ClampCameraPos();
        Vector3 DoCameraCentering(Vector3 position);
        Vector3 DoCameraInputMovement(float horizontal, float vertical, Vector3 mousePosition, float deltaTime);
        Vector3 DoCameraPanning(Vector2 mouseAxis, float deltaTime);
        Vector3 DoCameraZooming(float y, float deltaTime);
        float GetCameraZDistance();
        float MoveCamera(float value, float yPos, float deltaTime);
        Vector3 GetMinScreenBoundries();
        Vector3 GetMaxScreenBoundries();
        void DoProfileInfoClick(SelectedPortraitClickSignal signal);
    }
}