using RTSEngine.Core;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using RTSEngine.RTSUserInterface.Utils;
using Zenject;

namespace RTSEngine.RTSUserInterface
{
    public class UserInterfaceBase
    {

        private UserInterface _userInterface;

        //TODO move to factory class
        private DefaultActionButton.Factory _actionFactory;
        private DefaultBannerButton.Factory _bannerFactory;
        private DefaultItemButton.Factory _itemFactory;
        private DefaultMiniatureButton.Factory _miniatureFactory;
        private DefaultPortraitButton.Factory _portraitFactory;
        private DefaultInfoButton.Factory _infoFactory;
        private DefaultLogText.Factory _logFactory;

        private IRuntimeSet<ISelectable> _mainList;


        public UserInterfaceBase(UserInterface userInterface, DefaultActionButton.Factory actionFactory, DefaultBannerButton.Factory bannerFactory, DefaultItemButton.Factory itemFactory, DefaultMiniatureButton.Factory miniatureFactory, DefaultPortraitButton.Factory portraitFactory, DefaultInfoButton.Factory infoFactory, IRuntimeSet<ISelectable> mainList, DefaultLogText.Factory logFactory)
        {
            _userInterface = userInterface;
            ActionFactory = actionFactory;
            BannerFactory = bannerFactory;
            ItemFactory = itemFactory;
            MiniatureFactory = miniatureFactory;
            PortraitFactory = portraitFactory;
            InfoFactory = infoFactory;
            _mainList = mainList;
            LogFactory = logFactory;
        }

        public UserInterfaceBaseComponent UserInterfaceBaseComponent { get; set; }
        public DefaultActionButton.Factory ActionFactory { get => _actionFactory; set => _actionFactory = value; }
        public DefaultBannerButton.Factory BannerFactory { get => _bannerFactory; set => _bannerFactory = value; }
        public DefaultItemButton.Factory ItemFactory { get => _itemFactory; set => _itemFactory = value; }
        public DefaultMiniatureButton.Factory MiniatureFactory { get => _miniatureFactory; set => _miniatureFactory = value; }
        public DefaultPortraitButton.Factory PortraitFactory { get => _portraitFactory; set => _portraitFactory = value; }
        public DefaultInfoButton.Factory InfoFactory { get => _infoFactory; set => _infoFactory = value; }
        public DefaultLogText.Factory LogFactory { get => _logFactory; set => _logFactory = value; }

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
            if (UserInterfaceBaseComponent.BannerPanel)
            {
                UserInterfaceUtils.ClearPanel(UserInterfaceBaseComponent.BannerPanel);
                if (_userInterface.GetParties() != null)
                {
                    foreach (var party in _userInterface.GetParties())
                    {
                        var button = CreatePrefabOnPanel(BannerFactory, UserInterfaceBaseComponent.BannerPanel, party.Key);
                    }
                }
            }
        }

        public virtual void UpdateItems()
        {
        }

        public virtual void UpdateMiniatures()
        {
            if (UserInterfaceBaseComponent.MiniaturePanel)
            {
                UserInterfaceUtils.ClearPanel(UserInterfaceBaseComponent.MiniaturePanel);
                if (_userInterface.GetActualSelection() != null)
                {
                    foreach (var selectable in _userInterface.GetActualSelection())
                    {
                        var button = CreatePrefabOnPanel(MiniatureFactory, UserInterfaceBaseComponent.MiniaturePanel, selectable);
                    }
                }
            }
        }



        public virtual void UpdatePortrait()
        {
            if (UserInterfaceBaseComponent.PortraitPanel)
            {
                UserInterfaceUtils.ClearPanel(UserInterfaceBaseComponent.PortraitPanel);
                if (_userInterface.Highlighted != null)
                {
                    var button = CreatePrefabOnPanel(PortraitFactory, UserInterfaceBaseComponent.PortraitPanel, _userInterface.Highlighted);
                }
            }
        }

        private DefaultClickable CreatePrefabOnPanel<T>(PlaceholderFactory<T> factory, RectTransform panel, object reference) where T : DefaultClickable
        {
            var instance = factory.Create();
            instance.transform.SetParent(panel, false);
            var button = instance as DefaultClickable;
            button.ObjectReference = reference;
            button.UpdateApperance();
            return button;
        }
       
        public void UpdatedObject(ISelectable selectable)
        {
            throw new NotImplementedException();
        }

        public void DeletedObject(ISelectable selectable)
        {
            DeleteFromSelection(selectable);
            DeleteFromParties(selectable);
            UpdateAll();
        }

        private void DeleteFromSelection(ISelectable selectable)
        {
            List<ISelectable> aux = _userInterface.GetActualSelection().ToList();
            aux.Remove(selectable);
            if (!aux.Contains(_userInterface.Highlighted))
            {
                _userInterface.Highlighted = null;
            }
            _userInterface.DoSelectionUpdate(aux.ToArray(), true);
        }

        private void DeleteFromParties(ISelectable selectable)
        {
            foreach (var party in _userInterface.GetParties().ToList())
            {
                var aux = party.Value.ToList();
                aux.Remove(selectable);
                if (aux.Count > 0)
                    _userInterface.GetParties()[party.Key] = aux.ToArray();
                else
                    _userInterface.GetParties().Remove(party.Key);
            }
        }
    }
}