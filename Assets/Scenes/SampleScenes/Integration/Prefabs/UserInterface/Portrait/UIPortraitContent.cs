using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RTSEngine.RTSUserInterface;
using RTSEngine.Signal;

namespace RTSEngine.Integration.Scene
{
    public class UIPortraitContent : UIContent, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image _picture;
        [SerializeField] private UIPortraitStatusBar _healthBar;
        [SerializeField] private UIPortraitStatusBar _manaBar;

        private UIMouseOver _uiMouseOver;
        private bool _clicked;

        private void Start()
        {
            SignalBus.Subscribe<MoveCameraSignal>(CenterCameraToHighlighted);
            _uiMouseOver = GetComponent<UIMouseOver>();
            if (!_uiMouseOver)
            {
                Debug.LogWarning("Your portrait prefab is missing a UIMouseOver script. One will be added.");
                _uiMouseOver = gameObject.AddComponent<UIMouseOver>();
            }
        }

        private void Update()
        {
            if (_uiMouseOver.MouseOver && _clicked)
            {
                CenterCameraToHighlighted();
            }
        }

        private void CenterCameraToHighlighted()
        {
            if (Info != null)
            {
                var selectable = (Info as UISceneIntegratedContentInfo).Selectable as IntegrationSceneObject;
                SignalBus.Fire(new CameraGoToPositionSignal() { Position = selectable.Position });
            }
        }

        public override void UpdateAppearance()
        {
            var selectable = (Info as UISceneIntegratedContentInfo).Selectable as IntegrationSceneObject;
            _picture.sprite = selectable.Picture;
            SetStatuses(selectable);
        }

        private void SetStatuses(IntegrationSceneObject selectable)
        {
            SetHealthBar<HealthStatus>(selectable, _healthBar);
            SetHealthBar<ManaStatus>(selectable, _manaBar);
            //TODO set other statuses
        }

        private void SetHealthBar<T>(IntegrationSceneObject selectable, UIPortraitStatusBar component) where T : DefaultStatus
        {
            var status = selectable.GetComponent<T>();
            if (status)
            {
                component.gameObject.SetActive(status.MaxValue != 0);
                component.StatusBar.fillAmount = status.Value / (float)status.MaxValue;
                component.StatusText.text = string.Format("{0} / {1}", status.Value, status.MaxValue);
            }
            else
            {
                component.gameObject.SetActive(false);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            this._clicked = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            this._clicked = false;
        }
    }
}