using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using RTSEngine.Signal;
using Zenject;
using System;

namespace RTSEngine.Refactoring
{
    public class DefaultRTSCameraInput : MonoBehaviour
    {
        [SerializeField] private KeyCode mousePanButton = KeyCode.Mouse2;
        [SerializeField] [Range(0, 0.2f)] private float viewportOffset = 0.01f;

        private GameSignalBus _signalBus;

        [Inject]
        public void Construct(GameSignalBus signalBus)
        {
            this._signalBus = signalBus;
        }

        public void LateUpdate()
        {
            GetCameraZoomInput();
            GetPanMovementInput();
            GetAxisMovementInput();
            GetMouseMovementInput();
            GetOtherInputs();
        }

        public virtual void GetCameraZoomInput()
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                _signalBus.Fire(new CameraZoomSignal() { Zoom = Input.mouseScrollDelta.y });
            }
        }

        public virtual void GetMouseMovementInput()
        {
            Vector2 offset = MouseIsOnOffset(Input.mousePosition);
            if (!offset.Equals(Vector2.zero))
            {
                _signalBus.Fire(new CameraMoveSignal() { Horizontal = offset.x, Vertical = offset.y });
            }
        }

        public virtual void GetAxisMovementInput()
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                _signalBus.Fire(new CameraMoveSignal() { Horizontal = Input.GetAxis("Horizontal"), Vertical = Input.GetAxis("Vertical") });
            }
        }

        public virtual void GetPanMovementInput()
        {
            if (Input.GetKey(mousePanButton))
            {
                _signalBus.Fire(new CameraPanSignal() { MouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) });
            }
        }

        public virtual void GetOtherInputs()
        {
        }

        private Vector2 MouseIsOnOffset(Vector3 mousePosition)
        {
            var position = (Vector2)Camera.main.ScreenToViewportPoint(mousePosition);
            float x = PositionOnBoundries(position.x);
            float y = PositionOnBoundries(position.y);
            return new Vector2(x, y);
        }

        private float PositionOnBoundries(float position)
        {
            if (position > 1 - viewportOffset && position <= 1)
            {
                return 1;
            }
            else if (position < viewportOffset && position >= 0)
            {
                return -1;
            }
            return 0;
        }

    }
}

