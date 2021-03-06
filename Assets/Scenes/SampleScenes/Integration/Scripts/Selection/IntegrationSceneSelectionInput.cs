using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RTSEngine.RTSUserInterface;
using RTSEngine.RTSSelection;
using Zenject;

namespace RTSEngine.Integration.Scene
{

    public class IntegrationSceneSelectionInput : DefaultSelectionInput
    {

        [SerializeField] private RectTransform _selectionBox;

        [Inject] private UserInterfaceBase userInterfaceBase;

        public override void GetAreaSelectionInput()
        {
            PreventSelection = WasGUIClicked();
            base.GetAreaSelectionInput();

            if (IsSelecting)
            {
                this._selectionBox.gameObject.SetActive(true);
                DrawSelectionBox();
            }
            else
            {
                this._selectionBox.gameObject.SetActive(false);
            }
        }

        public bool WasGUIClicked()
        {
            if (userInterfaceBase?.UserInterfaceBaseComponent?.Raycaster)
            {
                List<RaycastResult> results = new List<RaycastResult>();
                userInterfaceBase?.UserInterfaceBaseComponent?.Raycaster.Raycast(new PointerEventData(null) { position = Input.mousePosition }, results);
                return results.Count > 0;
            }
            return false;
        }

        private void DrawSelectionBox()
        {
            this._selectionBox.position = GetAreaCenter(StartScreenPoint, Input.mousePosition);
            this._selectionBox.sizeDelta = GetAreaSize(StartScreenPoint, Input.mousePosition);
        }

        public static Vector2 GetAreaSize(Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            return new Vector2(Mathf.Abs(initialScreenPosition.x - finalScreenPosition.x), Mathf.Abs(initialScreenPosition.y - finalScreenPosition.y));
        }

        public static Vector2 GetAreaCenter(Vector2 initialScreenPosition, Vector2 finalScreenPosition)
        {
            return (initialScreenPosition + finalScreenPosition) / 2;
        }

    }

}