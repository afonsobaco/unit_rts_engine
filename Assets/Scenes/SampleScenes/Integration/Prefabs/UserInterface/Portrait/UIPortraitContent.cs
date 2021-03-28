using UnityEngine;
using UnityEngine.UI;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class UIPortraitContent : UIContent
    {

        [SerializeField] private Image _picture;
        [SerializeField] private UIPortraitStatusBar _healthBar;
        [SerializeField] private UIPortraitStatusBar _manaBar;

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
    }
}