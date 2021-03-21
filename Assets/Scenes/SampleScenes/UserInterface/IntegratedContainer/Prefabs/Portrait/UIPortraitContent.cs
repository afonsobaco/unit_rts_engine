using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene

{
    public class UIPortraitContent : UIContent
    {

        [SerializeField] private Image _picture;
        [SerializeField] private UIPortraitStatusBar _healthBar;
        [SerializeField] private UIPortraitStatusBar _manaBar;

        public override void UpdateAppearance()
        {
            var miniature = (Info as UISceneIntegratedContentInfo).Selectable as UISceneIntegratedSelectable;
            _picture.sprite = miniature.Picture;
            SetHealth(miniature);
            SetMana(miniature);
        }

        private void SetMana(UISceneIntegratedSelectable miniature)
        {
            float actualMana = miniature.Mana / miniature.MaxMana;
            _manaBar.StatusBar.fillAmount = actualMana;
            _manaBar.StatusText.text = string.Format("{0} / {1}", actualMana * 100, miniature.MaxMana);
            _manaBar.gameObject.SetActive(miniature.MaxMana != 0);
        }

        private void SetHealth(UISceneIntegratedSelectable miniature)
        {
            float actualHealth = miniature.Health / miniature.MaxHealth;
            _healthBar.StatusBar.fillAmount = actualHealth;
            _healthBar.StatusText.text = string.Format("{0} / {1}", actualHealth * 100, miniature.MaxHealth);
        }
    }
}