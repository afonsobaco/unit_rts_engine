using UnityEngine;

namespace RTSEngine.Manager.Interfaces
{
    public interface IPlayerInputManager
    {
        void SetSelectionKeys(KeyCode aditiveSelectionKeyCode, KeyCode sameTypeSelectionKeyCode, KeyCode groupKeyCode);
        void SetMouseClick(float doubleClickTime);
        void DoGroupSelection(KeyCode groupKeyCode);
        void SetCameraControls();
        void SetCameraPanningControls();
        void DoCameraMovement();
    }

}
