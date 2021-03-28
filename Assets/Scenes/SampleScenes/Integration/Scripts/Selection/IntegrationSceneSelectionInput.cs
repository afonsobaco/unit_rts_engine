using UnityEngine;
using RTSEngine.RTSSelection;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{

    public class IntegrationSceneSelectionInput : DefaultSelectionInput
    {

        [SerializeField] private RectTransform _selectionBox;
        [SerializeField] private Canvas _uiCanvas;
        private UIMouseOver _uiMouseOver;

        private void Start()
        {
            _uiMouseOver = _uiCanvas.GetComponent<UIMouseOver>();
            if (!_uiMouseOver)
            {
                Debug.LogError("You need to add a UIMouseOver to be able to avoid UI clicks");
            }
        }

        public override void GetAreaSelectionInput()
        {
            PreventSelection = _uiMouseOver.MouseOver;

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

        private void DrawSelectionBox()
        {
            var canvasScale = _uiCanvas.scaleFactor;
            this._selectionBox.position = GetAreaCenter(StartScreenPoint, Input.mousePosition);
            this._selectionBox.sizeDelta = GetAreaSize(StartScreenPoint, Input.mousePosition) / canvasScale;
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