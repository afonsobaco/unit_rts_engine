using UnityEngine;
using RTSEngine.RTSCamera;
using RTSEngine.RTSSelection;

namespace RTSEngine.Integration.Scene
{

    public class IntegrationSceneCameraInput : DefaultRTSCameraInput
    {
        [SerializeField] private DefaultSelectionInput selectionInput;

        public override void GetMouseMovementInput()
        {
            if (!selectionInput.IsSelecting)
                base.GetMouseMovementInput();
        }
    }

}