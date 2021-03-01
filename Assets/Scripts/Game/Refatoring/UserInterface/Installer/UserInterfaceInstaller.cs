using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;
using UnityEngine.UI;
using System;

namespace RTSEngine.Refactoring
{
    public class UserInterfaceInstaller : MonoInstaller
    {
        [SerializeField] private DefaultActionButton _actionPrefab;
        [SerializeField] private DefaultBannerButton _bannerPrefab;
        [SerializeField] private DefaultItemButton _itemPrefab;
        [SerializeField] private DefaultMiniatureButton _miniaturePrefab;
        [SerializeField] private DefaultPortraitButton _portraitPrefab;
        [SerializeField] private RectTransform _actionPanel;
        [SerializeField] private RectTransform _bannerPanel;
        [SerializeField] private RectTransform _itemPanel;
        [SerializeField] private RectTransform _miniaturePanel;
        [SerializeField] private RectTransform _portraitPanel;

        public override void Start()
        {
            ClearPanel(_portraitPanel);
            ClearPanel(_itemPanel);
            ClearPanel(_miniaturePanel);
            ClearPanel(_bannerPanel);
            ClearPanel(_actionPanel);
        }

        public override void InstallBindings()
        {
            Container.Bind<UserInterfaceSignalManager>().AsSingle();
            Container.Bind<UserInterfaceManager>().AsSingle();
            Container.Bind<UserInterface>().AsSingle();
            Container.Bind<UserInterfaceBase>().AsSingle().OnInstantiated<UserInterfaceBase>(UpdateUserInterfaceBase);

            Container.DeclareSignal<SelectionUpdateSignal>();
            Container.DeclareSignal<PartyUpdateSignal>();
            Container.DeclareSignal<AlternateSubGroupSignal>();
            Container.DeclareSignal<MiniatureClickedSignal>();
            Container.DeclareSignal<PortraitClickedSignal>();
            Container.DeclareSignal<BannerClickedSignal>();
            Container.DeclareSignal<MapClickedSignal>();
            Container.DeclareSignal<ActionClickedSignal>();
            Container.DeclareSignal<CameraGoToPositionSignal>();
            Container.DeclareSignal<ChangeSelectionSignal>();

            Container.BindSignal<SelectionUpdateSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnSelectionUpdate).FromResolve();
            Container.BindSignal<PartyUpdateSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnPartyUpdate).FromResolve();
            Container.BindSignal<AlternateSubGroupSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnAlternateSubGroup).FromResolve();
            Container.BindSignal<MiniatureClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnMiniatureClicked).FromResolve();
            Container.BindSignal<PortraitClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnPortraitClicked).FromResolve();
            Container.BindSignal<BannerClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnBannerClicked).FromResolve();
            Container.BindSignal<MapClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnMapClicked).FromResolve();
            Container.BindSignal<ActionClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnActionClicked).FromResolve();

            Container.BindFactory<DefaultMiniatureButton, DefaultMiniatureButton.Factory>().FromComponentInNewPrefab(_miniaturePrefab);
            Container.BindFactory<DefaultPortraitButton, DefaultPortraitButton.Factory>().FromComponentInNewPrefab(_portraitPrefab);
            Container.BindFactory<DefaultBannerButton, DefaultBannerButton.Factory>().FromComponentInNewPrefab(_bannerPrefab);
            Container.BindFactory<DefaultItemButton, DefaultItemButton.Factory>().FromComponentInNewPrefab(_itemPrefab);
            Container.BindFactory<DefaultActionButton, DefaultActionButton.Factory>().FromComponentInNewPrefab(_actionPrefab);
        }

        private void UpdateUserInterfaceBase(InjectContext ctx, UserInterfaceBase userInterfaceBase)
        {
            userInterfaceBase.ActionPanel = _actionPanel;
            userInterfaceBase.BannerPanel = _bannerPanel;
            userInterfaceBase.ItemPanel = _itemPanel;
            userInterfaceBase.MiniaturePanel = _miniaturePanel;
            userInterfaceBase.PortraitPanel = _portraitPanel;
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