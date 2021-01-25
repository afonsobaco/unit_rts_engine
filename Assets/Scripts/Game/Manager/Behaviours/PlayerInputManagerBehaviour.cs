using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class PlayerInputManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private KeyCode aditiveSelectionKeyCode = KeyCode.LeftShift;
        [SerializeField] private KeyCode sameTypeSelectionKeyCode = KeyCode.LeftControl;
        [SerializeField] private KeyCode groupKeyCode = KeyCode.Z;
        [SerializeField] private float doubleClickTime = 0.3f;

        [Inject]
        public IPlayerInputManager Manager { get; private set; }
        public KeyCode AditiveSelectionKeyCode { get => aditiveSelectionKeyCode; set => aditiveSelectionKeyCode = value; }
        public KeyCode SameTypeSelectionKeyCode { get => sameTypeSelectionKeyCode; set => sameTypeSelectionKeyCode = value; }
        public KeyCode GroupKeyCode { get => groupKeyCode; set => groupKeyCode = value; }
        public float DoubleClickTime { get => doubleClickTime; set => doubleClickTime = value; }

        public void Update()
        {

            Manager.SetSelectionKeys(AditiveSelectionKeyCode, SameTypeSelectionKeyCode, GroupKeyCode);

            Manager.SetMouseClick(DoubleClickTime);

            Manager.DoGroupSelection(GroupKeyCode);
        }

        public void LateUpdate()
        {
            Manager.SetCameraControls();
            Manager.SetCameraPanningControls();
            Manager.DoCameraMovement();
        }
    }
}
