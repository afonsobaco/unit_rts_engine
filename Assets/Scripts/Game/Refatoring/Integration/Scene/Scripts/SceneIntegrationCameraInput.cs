using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RTSEngine.Refactoring
{

    public class SceneIntegrationCameraInput : DefaultCameraInput
    {
        [SerializeField] private DefaultSelectionInput selectionInput;

        public override void GetMouseMovementInput()
        {
            if (!selectionInput.IsSelecting)
                base.GetMouseMovementInput();
        }
    }

}