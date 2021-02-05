using RTSEngine.Utils;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class PlayerInputManager : IPlayerInputManager
    {

        private ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> _selectionManager;
        private ICameraManager _cameraManager;
        private IGUIManager _GUIManager;
        private SelectionOptions _selectionOptions;

        public SelectionOptions SelectionOptions { get => this._selectionOptions; private set => this._selectionOptions = value; }

        [Inject]
        public void Construct(ISelectionManager<ISelectableObjectBehaviour, SelectionTypeEnum> selectionManager, ICameraManager cameraManager, IGUIManager gUIManager)
        {
            this._selectionManager = selectionManager;
            this._cameraManager = cameraManager;
            this._GUIManager = gUIManager;

        }

        private Dictionary<KeyCode, int> _groupKeys = new Dictionary<KeyCode, int>()
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
        private float _lastTimeClicked;

        public void SetCameraControls()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                this._cameraManager.IsCentering = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                this._cameraManager.IsCentering = false;
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                Camera.main.transform.position = this._cameraManager.DoCameraZooming(Input.mouseScrollDelta.y, Time.deltaTime, Camera.main);
            }
        }

        public void DoCameraMovement()
        {
            if (this._cameraManager.IsCentering)
            {
                Camera.main.transform.position = this._cameraManager.DoCameraCentering(Camera.main);
            }
            else
            {
                if (this._cameraManager.IsPanning)
                {
                    var desired = this._cameraManager.DoCameraPanning(new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), Time.deltaTime, Camera.main);
                    Camera.main.transform.Translate(desired, Space.Self);
                }
                else
                {
                    Camera.main.transform.position += this._cameraManager.DoCameraInputMovement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.mousePosition, Time.deltaTime, Camera.main);
                }
            }
            Camera.main.transform.position = this._cameraManager.ClampCameraPos(Camera.main);
        }

        public void SetCameraPanningControls()
        {
            if (Input.GetMouseButtonDown(2))
            {
                this._cameraManager.IsPanning = true;
                this._cameraManager.Origin = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(2))
            {
                this._cameraManager.IsPanning = false;
            }
        }

        public void SetSelectionKeys(KeyCode aditive, KeyCode sameType, KeyCode groupKeyCode)
        {
            this._selectionManager.SetKeysPressed(Input.GetKey(aditive), Input.GetKey(sameType));
        }

        public void DoGroupSelection(KeyCode groupKeyCode)
        {
            int keyPressed = GetAnyGroupKeyPressed();
            if (keyPressed > 0)
            {
                if (Input.GetKey(groupKeyCode))
                {
                    this._selectionManager.CreateGroupSet(keyPressed);
                }
                else
                {
                    this._selectionManager.SetGroupNumperPressed(keyPressed);
                    this._selectionManager.DoSelection(Input.mousePosition);
                }
            }
        }

        public int GetAnyGroupKeyPressed()
        {
            foreach (KeyValuePair<KeyCode, int> entry in this._groupKeys)
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
                if (!this._GUIManager.ClickedOnGUI(Input.mousePosition))
                {
                    this._selectionManager.StartOfSelection(Input.mousePosition);
                }
                else
                {
                    Debug.Log("GUI");
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (this._selectionManager.IsSelecting())
                {
                    this._selectionManager.DoPreSelection(Input.mousePosition);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                this._selectionManager.SetScreenBoundries(this._cameraManager.GetMinScreenBoundries(Camera.main), this._cameraManager.GetMaxScreenBoundries(Camera.main));
                VerifyDoubleClick(doubleClickTime);
                if (this._selectionManager.IsSelecting())
                {
                    this._selectionManager.DoSelection(Input.mousePosition);
                }
                //TODO create a perform double click here

            }

        }

        public void VerifyDoubleClick(float doubleClickTime)
        {
            if (Time.time - this._lastTimeClicked <= doubleClickTime)
            {
                this._selectionManager.SetDoubleClick(true);
            }
            else
            {
                this._lastTimeClicked = Time.time;
            }

        }
    }

}
