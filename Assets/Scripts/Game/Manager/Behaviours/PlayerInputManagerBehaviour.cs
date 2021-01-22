
using System;
using UnityEngine;
using Zenject;
using RTSEngine.Manager.Interfaces;

namespace RTSEngine.Manager.Impls
{
    public class PlayerInputManagerBehaviour : MonoBehaviour
    {
        public KeyCode AditiveSelectionKeyCode = KeyCode.LeftShift;
        public KeyCode SameTypeSelectionKeyCode = KeyCode.LeftControl;
        public KeyCode groupKeyCode = KeyCode.Z;
        public float doubleClickTime = 0.3f;

        [Inject]
        public IPlayerInputManager Manager { get; private set; }

        public void Update()
        {

            Manager.SetSelectionKeys(AditiveSelectionKeyCode, SameTypeSelectionKeyCode, groupKeyCode);

            Manager.SetMouseClick(doubleClickTime);

            Manager.DoGroupSelection(groupKeyCode);
        }

        public void LateUpdate()
        {
            Manager.SetCameraControls();
            Manager.SetCameraPanningControls();
            Manager.DoCameraMovement();
        }
    }
}
