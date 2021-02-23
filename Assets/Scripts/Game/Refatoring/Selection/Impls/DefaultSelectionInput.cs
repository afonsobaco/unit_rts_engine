using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using RTSEngine.Signal;
using Zenject;


namespace RTSEngine.Refactoring
{
    public class DefaultSelectionInput : MonoBehaviour
    {
        [SerializeField] private KeyCode GroupKeyCode = KeyCode.Z;
        private bool preventSelection;
        private bool isSelecting;
        protected Vector3 _startScreenPoint;
        protected GameSignalBus _signalBus;

        public bool IsSelecting { get => isSelecting; set => isSelecting = value; }
        public bool PreventSelection { get => preventSelection; set => preventSelection = value; }

        [Inject]
        public void Construct(GameSignalBus signalBus)
        {
            this._signalBus = signalBus;
        }

        void Update()
        {
            GetAreaSelectionInput();
            GetGroupSelectionInput();
            GetOtherInputs();
        }

        public virtual void GetAreaSelectionInput()
        {
            if (Input.GetMouseButtonDown(0) && !PreventSelection)
            {
                IsSelecting = true;
                _startScreenPoint = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0) && isSelecting)
            {
                IsSelecting = false;
                _signalBus.Fire(new AreaSelectionSignal() { StartPoint = _startScreenPoint, EndPoint = Input.mousePosition });
            }
        }

        public virtual void GetGroupSelectionInput()
        {
            var keyPressed = GameUtils.GetAnyGroupKeyPressed();
            if (keyPressed > 0)
            {
                _signalBus.Fire(new GroupSelectionSignal() { GroupId = keyPressed, CreateNew = Input.GetKey(GroupKeyCode) });
            }
        }

        public virtual void GetOtherInputs()
        {
        }
    }
}