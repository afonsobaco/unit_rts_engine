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
        [SerializeField] private KeyCode PartyKeyCode = KeyCode.Z;
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
            GetPartySelectionInput();
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

        public virtual void GetPartySelectionInput()
        {
            var keyPressed = GameUtils.GetAnyPartyKeyPressed();
            if (keyPressed > 0)
            {
                _signalBus.Fire(new PartySelectionSignal() { PartyId = keyPressed, CreateNew = Input.GetKey(PartyKeyCode) });
            }
        }

        public virtual void GetOtherInputs()
        {
        }
    }
}