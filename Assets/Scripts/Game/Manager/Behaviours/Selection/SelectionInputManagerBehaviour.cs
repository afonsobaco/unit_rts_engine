using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RTSEngine.Manager
{
    public class SelectionInputManagerBehaviour : MonoBehaviour
    {
        [SerializeField] private KeyCode _aditiveSelectionKeyCode = KeyCode.LeftShift;
        [SerializeField] private KeyCode _sameTypeSelectionKeyCode = KeyCode.LeftControl;
        [SerializeField] private KeyCode _groupKeyCode = KeyCode.Z;
        [SerializeField] private float _doubleClickTime = 0.3f;
        private float _lastTimeClicked;

        private SelectionOptions _selectionOptions;
        private SelectionManager _selectionManager;

        public void Update()
        {
            this.SetSelectionKeys(_aditiveSelectionKeyCode, _sameTypeSelectionKeyCode, _groupKeyCode);

            this.SetMouseClick(_doubleClickTime);

            this.DoGroupSelection(_groupKeyCode);
        }

        //TODO use gameutil
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

        [Inject]
        public void Constructor(SelectionManager selectionManager)
        {
            this._selectionManager = selectionManager;
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
            if (_selectionManager.CanSelect())
            {

                if (Input.GetMouseButtonDown(0))
                {
                    this._selectionManager.StartOfSelection(Input.mousePosition);
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
                    VerifyDoubleClick(doubleClickTime);
                    if (this._selectionManager.IsSelecting())
                    {
                        this._selectionManager.DoSelection(Input.mousePosition);
                    }
                    //TODO create a perform double click here
                }

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
