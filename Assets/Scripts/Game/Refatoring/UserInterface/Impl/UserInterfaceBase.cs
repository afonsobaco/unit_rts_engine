using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;
using UnityEngine.UI;
using System;

namespace RTSEngine.Refactoring
{
    public class UserInterfaceBase
    {
        private RectTransform _actionPanel;
        private RectTransform _bannerPanel;
        private RectTransform _itemPanel;
        private RectTransform _miniaturePanel;
        private RectTransform _portraitPanel;

        private UserInterface _userInterface;
        private DefaultActionButton.Factory _actionFactory;
        private DefaultBannerButton.Factory _bannerFactory;
        private DefaultItemButton.Factory _itemFactory;
        private DefaultMiniatureButton.Factory _miniatureFactory;
        private DefaultPortraitButton.Factory _portraitFactory;

        public UserInterfaceBase(UserInterface userInterface, DefaultActionButton.Factory actionFactory, DefaultBannerButton.Factory bannerFactory, DefaultItemButton.Factory itemFactory, DefaultMiniatureButton.Factory miniatureFactory, DefaultPortraitButton.Factory portraitFactory)
        {
            _userInterface = userInterface;
            _actionFactory = actionFactory;
            _bannerFactory = bannerFactory;
            _itemFactory = itemFactory;
            _miniatureFactory = miniatureFactory;
            _portraitFactory = portraitFactory;
        }

        public RectTransform ActionPanel { get => _actionPanel; set => _actionPanel = value; }
        public RectTransform BannerPanel { get => _bannerPanel; set => _bannerPanel = value; }
        public RectTransform ItemPanel { get => _itemPanel; set => _itemPanel = value; }
        public RectTransform MiniaturePanel { get => _miniaturePanel; set => _miniaturePanel = value; }
        public RectTransform PortraitPanel { get => _portraitPanel; set => _portraitPanel = value; }

        public void UpdateAll()
        {
            UpdateActions();
            UpdateBanners();
            UpdateItems();
            UpdateMiniatures();
            UpdatePortrait();
        }

        public virtual void UpdateActions()
        {
        }

        public virtual void UpdateBanners()
        {
            if (BannerPanel)
            {
                ClearPanel(BannerPanel);
                if (_userInterface.Parties != null)
                {
                    foreach (var party in _userInterface.Parties)
                    {
                        var button = CreatePrefabOnPanel(_bannerFactory, BannerPanel, party.Key);
                    }
                }
            }
        }

        public virtual void UpdateItems()
        {
        }

        public virtual void UpdateMiniatures()
        {
            if (MiniaturePanel)
            {
                ClearPanel(MiniaturePanel);
                if (_userInterface.Selection != null)
                {
                    foreach (var selectable in _userInterface.Selection)
                    {
                        var button = CreatePrefabOnPanel(_miniatureFactory, MiniaturePanel, selectable);
                    }
                }
            }
        }

        public virtual void UpdatePortrait()
        {
            if (PortraitPanel)
            {
                ClearPanel(PortraitPanel);
                if (_userInterface.Highlighted != null)
                {
                    var button = CreatePrefabOnPanel(_portraitFactory, PortraitPanel, _userInterface.Highlighted);
                }
            }
        }



        private DefaultClickableButton CreatePrefabOnPanel<T>(PlaceholderFactory<T> factory, RectTransform panel, object reference) where T : DefaultClickableButton
        {
            var instance = factory.Create();
            instance.transform.SetParent(panel, false);
            var button = instance as DefaultClickableButton;
            button.ObjectReference = reference;
            button.UpdateApperance();
            return button;
        }
        public void ClearPanel(RectTransform panel)
        {
            if (panel)
            {
                foreach (Transform child in panel)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
    }
}