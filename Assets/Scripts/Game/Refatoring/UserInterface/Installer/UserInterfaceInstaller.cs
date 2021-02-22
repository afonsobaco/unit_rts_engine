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

        [SerializeField] private RectTransform portraitPanel;
        [SerializeField] private RectTransform itemPanel;
        [SerializeField] private RectTransform miniaturePanel;
        [SerializeField] private RectTransform bannerPanel;
        [SerializeField] private RectTransform actionPanel;
        [SerializeField] private GameObject bannerPrefab;
        [SerializeField] private GameObject miniaturePrefab;
        [SerializeField] private GameObject portraitPrefab;
        [SerializeField] private GameObject actionPrefab;
        [SerializeField] private GameObject itemPrefab;

        public override void InstallBindings()
        {
            Container.Bind<UserInterfaceSignalManager>().AsSingle();
            Container.Bind<UserInterfaceManager>().AsSingle();
            Container.Bind<UserInterface>().AsSingle();

            Container.DeclareSignal<SelectionUpdateSignal>();
            Container.DeclareSignal<GroupUpdateSignal>();
            Container.DeclareSignal<AlternateSubGroupSignal>();
            Container.DeclareSignal<MiniatureClickedSignal>();
            Container.DeclareSignal<PortraitClickedSignal>();
            Container.DeclareSignal<BannerClickedSignal>();
            Container.DeclareSignal<MapClickedSignal>();
            Container.DeclareSignal<ActionClickedSignal>();
            Container.DeclareSignal<CameraGoToPositionSignal>();
            Container.DeclareSignal<ChangeSelectionSignal>();

            Container.BindSignal<SelectionUpdateSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnSelectionUpdate).FromResolve();
            Container.BindSignal<GroupUpdateSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnGroupUpdate).FromResolve();
            Container.BindSignal<AlternateSubGroupSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnAlternateSubGroup).FromResolve();
            Container.BindSignal<MiniatureClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnMiniatureClicked).FromResolve();
            Container.BindSignal<PortraitClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnPortraitClicked).FromResolve();
            Container.BindSignal<BannerClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnBannerClicked).FromResolve();
            Container.BindSignal<MapClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnMapClicked).FromResolve();
            Container.BindSignal<ActionClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnActionClicked).FromResolve();

            Container.BindFactory<DefaultMiniatureButton, DefaultMiniatureButton.Factory>()
                .FromComponentInNewPrefab(miniaturePrefab).OnInstantiated<DefaultMiniatureButton>(MiniatureCreated);
        }

        private void MiniatureCreated(InjectContext ctx, DefaultMiniatureButton button)
        {
            button.transform.SetParent(miniaturePanel.transform, false);
        }
    }
}