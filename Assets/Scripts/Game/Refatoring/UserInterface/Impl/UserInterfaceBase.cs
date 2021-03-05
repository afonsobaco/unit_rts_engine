using UnityEngine;
using Zenject;
using RTSEngine.Core;
using System.Linq;

namespace RTSEngine.Refactoring
{
    public class UserInterfaceBase : ITickable
    {

        private UserInterface _userInterface;
        private DefaultActionButton.Factory _actionFactory;
        private DefaultBannerButton.Factory _bannerFactory;
        private DefaultItemButton.Factory _itemFactory;
        private DefaultMiniatureButton.Factory _miniatureFactory;
        private DefaultPortraitButton.Factory _portraitFactory;

        private IRuntimeSet<ISelectable> _mainList;


        public UserInterfaceBase(UserInterface userInterface, DefaultActionButton.Factory actionFactory, DefaultBannerButton.Factory bannerFactory, DefaultItemButton.Factory itemFactory, DefaultMiniatureButton.Factory miniatureFactory, DefaultPortraitButton.Factory portraitFactory, IRuntimeSet<ISelectable> mainList)
        {
            _userInterface = userInterface;
            _actionFactory = actionFactory;
            _bannerFactory = bannerFactory;
            _itemFactory = itemFactory;
            _miniatureFactory = miniatureFactory;
            _portraitFactory = portraitFactory;
            _mainList = mainList;
        }

        public UserInterfaceBaseComponent UserInterfaceBaseComponent { get; set; }

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
                ClearPanel(UserInterfaceBaseComponent.BannerPanel);
                if (_userInterface.Parties != null)
                {
                    foreach (var party in _userInterface.Parties)
                    {
                        var button = CreatePrefabOnPanel(_bannerFactory, UserInterfaceBaseComponent.BannerPanel, party.Key);
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
                ClearPanel(UserInterfaceBaseComponent.MiniaturePanel);
                if (_userInterface.GetActualSelection() != null)
                {
                    foreach (var selectable in _userInterface.GetActualSelection())
                    {
                        var button = CreatePrefabOnPanel(_miniatureFactory, UserInterfaceBaseComponent.MiniaturePanel, selectable);
                    }
                }
            }
        }

        public virtual void UpdatePortrait()
        {
            if (UserInterfaceBaseComponent.PortraitPanel)
            {
                ClearPanel(UserInterfaceBaseComponent.PortraitPanel);
                if (_userInterface.Highlighted != null)
                {
                    var button = CreatePrefabOnPanel(_portraitFactory, UserInterfaceBaseComponent.PortraitPanel, _userInterface.Highlighted);
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

        public void Tick()
        {
            var aux = _userInterface.GetActualSelection().Where(x => _mainList.GetMainList().Contains(x)).ToArray();
            if (!_mainList.GetMainList().Contains(_userInterface.Highlighted))
            {
                _userInterface.Highlighted = null;
            }
            _userInterface.DoSelectionUpdate(aux, true);
            UpdateAll();
        }
    }
}