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
        private Vector3 _starScreenPoint;
        private GameSignalBus _signalBus;

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
            if (Input.GetMouseButtonDown(0))
            {
                _starScreenPoint = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                _signalBus.Fire(new AreaSelectionSignal() { StartPoint = _starScreenPoint, EndPoint = Input.mousePosition });
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