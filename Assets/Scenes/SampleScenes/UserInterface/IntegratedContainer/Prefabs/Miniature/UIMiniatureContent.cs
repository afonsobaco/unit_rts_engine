using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene

{
    public class UIMiniatureContent : UIContent
    {
        [SerializeField] private Image _picture;
        [SerializeField] private Image _healthBar;
        [SerializeField] private Image _manaBar;

        public override void UpdateAppearance()
        {
            var miniature = Info as UIMiniatureContentInfo;
            _picture.sprite = miniature.Picture;
            _healthBar.fillAmount = miniature.Health / miniature.MaxHealth;
            if (miniature.MaxMana == 0)
            {
                GetGameObject(_manaBar.gameObject).SetActive(false);
            }
            else
            {
                GetGameObject(_manaBar.gameObject).SetActive(true);
                _manaBar.fillAmount = miniature.Mana / miniature.MaxMana;
            }
        }

        private GameObject GetGameObject(GameObject component)
        {
            UIMiniatureRootComponent uIMiniatureRootComponent = component.GetComponentInParent<UIMiniatureRootComponent>();
            if (uIMiniatureRootComponent)
            {
                return uIMiniatureRootComponent.gameObject;
            }
            return component;
        }
    }
}