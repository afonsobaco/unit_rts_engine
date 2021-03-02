﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RTSEngine.Signal;
using Zenject;

namespace RTSEngine.Refactoring
{

    public class SelectionInput : DefaultSelectionInput
    {

        [SerializeField] private RectTransform _selectionBox;
        [SerializeField] private GraphicRaycaster raycaster; public override void GetAreaSelectionInput()
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
            if (raycaster)
            {
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(new PointerEventData(null) { position = Input.mousePosition }, results);
                return results.Count > 0;
            }
            return false;
        }

        private void DrawSelectionBox()
        {
            this._selectionBox.position = GetAreaCenter(_startScreenPoint, Input.mousePosition);
            this._selectionBox.sizeDelta = GetAreaSize(_startScreenPoint, Input.mousePosition);
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