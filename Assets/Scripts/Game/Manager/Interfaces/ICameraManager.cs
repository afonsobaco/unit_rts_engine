using UnityEngine;
namespace RTSEngine.Manager
{
    public interface ICameraManager
    {
        void SetCameraSettings(ICameraSettings value);
        bool IsCentering();
        void SetIsCentering(bool value);
        bool IsPanning();
        void SetIsPanning(bool value);
        void SetCanMove(bool value);
        void SetPointOfInterest(ISelectableObject selected);
        Vector3 DoAxisCameraMovement(float horizontal, float vertical, float deltaTime);
        Vector3 ClampCameraPos();
        Vector3 DoCameraCentering();
        Vector3 DoCameraInputMovement(float horizontal, float vertical, Vector3 mousePosition, float deltaTime);
        Vector3 DoCameraPanning(Vector2 mouseAxis, float deltaTime);
        Vector3 DoCameraZooming(float y, float deltaTime);
        float GetCameraZDistance();
        float MoveCamera(float value, float yPos, float deltaTime);
        Vector3 GetMinScreenBoundries();
        Vector3 GetMaxScreenBoundries();
        void DoSelectedPortraitClick(SelectedPortraitClickSignal signal);
    }
}