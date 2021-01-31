using RTSEngine.Utils;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class PlayerInputManager : IPlayerInputManager
    {

        private ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager;
        private ICameraManager cameraManager;
        private SelectionOptions selectionOptions;

        public SelectionOptions SelectionOptions { get => selectionOptions; private set => selectionOptions = value; }

        [Inject]
        public void Construct(ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager, ICameraManager cameraManager)
        {
            this.selectionManager = selectionManager;
            this.cameraManager = cameraManager;

            this.selectionManager.SetScreenBoundries(this.cameraManager.GetMinScreenBoundries(Camera.main), this.cameraManager.GetMaxScreenBoundries(Camera.main));
        }

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

        public void SetCameraControls()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                cameraManager.IsCentering = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                cameraManager.IsCentering = false;
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                Camera.main.transform.position = cameraManager.DoCameraZooming(Input.mouseScrollDelta.y, Time.deltaTime, Camera.main);
            }
        }

        public void DoCameraMovement()
        {
            if (cameraManager.IsCentering)
            {
                Camera.main.transform.position = cameraManager.DoCameraCentering(Camera.main);
            }
            else
            {
                if (cameraManager.IsPanning)
                {
                    var desired = cameraManager.DoCameraPanning(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), Time.deltaTime, Camera.main);
                    Camera.main.transform.Translate(desired, Space.Self);
                }
                else
                {
                    Camera.main.transform.position += cameraManager.DoCameraInputMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.mousePosition, Time.deltaTime, Camera.main);
                }
            }
            Camera.main.transform.position = cameraManager.ClampCameraPos(Camera.main);
        }

        public void SetCameraPanningControls()
        {
            if (Input.GetMouseButtonDown(2))
            {
                cameraManager.IsPanning = true;
                cameraManager.Origin = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(2))
            {
                cameraManager.IsPanning = false;
            }
        }

        public void SetSelectionKeys(KeyCode aditive, KeyCode sameType, KeyCode groupKeyCode)
        {
            selectionManager.SetKeysPressed(Input.GetKey(aditive), Input.GetKey(sameType));
        }

        public void DoGroupSelection(KeyCode groupKeyCode)
        {
            int keyPressed = GetAnyGroupKeyPressed();
            if (keyPressed > 0)
            {
                if (Input.GetKey(groupKeyCode))
                {
                    selectionManager.CreateGroupSet(keyPressed);
                }
                else
                {
                    selectionManager.SetGroupNumperPressed(keyPressed);
                    selectionManager.DoSelection(Input.mousePosition);
                }
            }
        }

        public int GetAnyGroupKeyPressed()
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

        public void SetMouseClick(float doubleClickTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectionManager.StartOfSelection(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                selectionManager.DoPreSelection(Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                VerifyDoubleClick(doubleClickTime);
                selectionManager.DoSelection(Input.mousePosition);
                //TODO create a perform double click here
            }

        }

        public void VerifyDoubleClick(float doubleClickTime)
        {
            if (Time.time - lastTimeClicked <= doubleClickTime)
            {
                selectionManager.SetDoubleClick(true);
            }
            else
            {
                lastTimeClicked = Time.time;
            }

        }
    }

}
