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
        private bool preventSelection;
        private bool isSelecting;
        private Vector3 _startScreenPoint;
        private GameSignalBus _signalBus;

        public bool IsSelecting { get => isSelecting; set => isSelecting = value; }
        public bool PreventSelection { get => preventSelection; set => preventSelection = value; }
        public Vector3 StartScreenPoint { get => _startScreenPoint; set => _startScreenPoint = value; }
        public GameSignalBus SignalBus { get => _signalBus; set => _signalBus = value; }

        [Inject]
        public void Construct(GameSignalBus signalBus)
        {
            this.SignalBus = signalBus;
        }

        void Update()
        {
            GetAreaSelectionInput();
            GetOtherInputs();
        }

        public virtual void GetAreaSelectionInput()
        {
            if (Input.GetMouseButtonDown(0) && !PreventSelection)
            {
                IsSelecting = true;
                StartScreenPoint = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0) && isSelecting)
            {
                IsSelecting = false;
                SignalBus.Fire(new AreaSelectionSignal() { StartPoint = StartScreenPoint, EndPoint = Input.mousePosition });
            }
        }

        public virtual void GetOtherInputs()
        {
        }
    }
}