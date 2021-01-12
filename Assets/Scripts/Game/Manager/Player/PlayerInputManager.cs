
using System;
using System.Collections.Generic;
using UnityEngine;


namespace RTSEngine.Manager
{
    public class PlayerInputManager : MonoBehaviour
    {
        public KeyCode AditiveSelectionKeyCode = KeyCode.LeftShift;
        public KeyCode SameTypeSelectionKeyCode = KeyCode.LeftControl;
        public KeyCode groupKeyCode = KeyCode.Z;
        public float doubleClickTime = 0.3f;
        private Dictionary<KeyCode, int> groupKeys = new Dictionary<KeyCode, int>()
        {
            {KeyCode.Alpha1, 1},
            {KeyCode.Alpha2, 2},
            {KeyCode.Alpha3, 3},
            {KeyCode.Alpha4, 4},
            {KeyCode.Alpha5, 5},
            {KeyCode.Alpha6, 6},
            {KeyCode.Alpha7, 7},
            {KeyCode.Alpha8, 8},
            {KeyCode.Alpha9, 9},
            {KeyCode.Alpha0, 10}
        };
        private float lastTimeClicked;

        void Update()
        {
            SetMouseClick();

            DoGroupSelection();

            SetSelectionKeys();

        }
        void LateUpdate()
        {
            SetCameraControls();
            SetDragCameraControls();
        }

        private void SetCameraControls()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CameraManager.Instance.IsCentering = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                CameraManager.Instance.IsCentering = false;
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                CameraManager.Instance.ZoomCamera(Input.mouseScrollDelta.y);
            }

            CameraManager.Instance.DoCameraMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.mousePosition);

        }

        private static void SetDragCameraControls()
        {
            if (Input.GetMouseButtonDown(2))
            {
                CameraManager.Instance.IsPanning = true;
                CameraManager.Instance.Origin = Input.mousePosition;
            }

            if (Input.GetMouseButton(2))
            {
                CameraManager.Instance.DoCameraPanning(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")));
            }
            if (Input.GetMouseButtonUp(2))
            {
                CameraManager.Instance.IsPanning = false;
            }
        }

        private void SetSelectionKeys()
        {
            SelectionManager.Instance.IsAditiveSelection = Input.GetKey(AditiveSelectionKeyCode);
            SelectionManager.Instance.IsSameTypeSelection = Input.GetKey(SameTypeSelectionKeyCode);
            DoGroupSelection();
        }

        private void DoGroupSelection()
        {
            int keyPressed = GetAnyGroupKeyPressed();
            if (keyPressed > 0)
            {
                if (Input.GetKey(groupKeyCode))
                {
                    SelectionManager.Instance.SetGroup(keyPressed);
                }
                else
                {
                    SelectionManager.Instance.GetGroup(keyPressed);
                }
            }
        }

        private int GetAnyGroupKeyPressed()
        {
            foreach (KeyValuePair<KeyCode, int> entry in groupKeys)
            {
                if (Input.GetKeyDown(entry.Key))
                {
                    return entry.Value;
                }
            }
            return 0;
        }

        private void SetMouseClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectionManager.Instance.StartOfSelection(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                SelectionManager.Instance.DoPreSelection(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                VerifyDoubleClick();
                SelectionManager.Instance.EndOfSelection(Input.mousePosition);
            }


        }

        private void VerifyDoubleClick()
        {
            if (Time.time - lastTimeClicked <= doubleClickTime)
            {
                SelectionManager.Instance.IsDoubleClick = true;
            }
            else
            {
                lastTimeClicked = Time.time;
            }

        }
    }

}
