using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using System;
using RTSEngine.Signal;

/**
*
* This class is just for tests
* It should be used only as a reference for real implementation
*
*/
public class SceneHelper : MonoBehaviour
{
    [SerializeField] private float viewportOffset;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void Update()
    {

        if (Input.mouseScrollDelta.y != 0)
        {
            _signalBus.Fire(new CameraZoomSignal() { Zoom = Input.mouseScrollDelta.y });
        }

        if (Input.GetMouseButton(2))
        {
            _signalBus.Fire(new CameraPanSignal() { MouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) });
        }
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            _signalBus.Fire(new CameraMoveSignal() { Horizontal = Input.GetAxis("Horizontal"), Vertical = Input.GetAxis("Vertical") });
        }
        else
        {
            Vector2 offset = MouseIsOnOffset(Input.mousePosition);
            if (!offset.Equals(Vector2.zero))
            {
                _signalBus.Fire(new CameraMoveSignal() { Horizontal = offset.x, Vertical = offset.y });
            }
        }
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
