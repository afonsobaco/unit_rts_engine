using UnityEngine;

namespace RTSEngine.Manager
{
    public interface IPlayerInputManager
    {
        void DoCameraMovement();
        void DoGroupSelection(KeyCode groupKeyCode);
        void SetCameraControls();
        void SetCameraPanningControls();
        void SetMouseClick(float doubleClickTime);
        void SetSelectionKeys(KeyCode aditiveSelectionKeyCode, KeyCode sameTypeSelectionKeyCode, KeyCode groupKeyCode);
    }

}
