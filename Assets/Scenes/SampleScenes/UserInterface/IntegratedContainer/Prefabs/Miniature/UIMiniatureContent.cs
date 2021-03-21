using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace RTSEngine.RTSUserInterface.Scene

{
    public class UIMiniatureContent : UIContent
    {
        [SerializeField] private Image _picture;
        [SerializeField] private UIMiniatureStatusBar _healthBar;
        [SerializeField] private UIMiniatureStatusBar _manaBar;
        [SerializeField] private List<GameObject> _highlights;

        public override void UpdateAppearance()
        {
            var miniature = (Info as UISceneIntegratedContentInfo).Selectable as UISceneIntegratedSelectable;
            _picture.sprite = miniature.Picture;
            _healthBar.StatusBar.fillAmount = miniature.Health / miniature.MaxHealth;
            _manaBar.StatusBar.fillAmount = miniature.Mana / miniature.MaxMana;
            _manaBar.gameObject.SetActive(miniature.MaxMana != 0);
            _highlights.ForEach(x => x.SetActive(miniature.IsHighlighted));
        }

    }
}