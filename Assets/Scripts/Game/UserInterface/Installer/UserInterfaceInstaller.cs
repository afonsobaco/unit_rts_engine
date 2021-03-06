using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;
using RTSEngine.Commons;
using UnityEngine.UI;
using System;

namespace RTSEngine.RTSUserInterface
{
    [CreateAssetMenu(fileName = "UserInterfaceInstaller", menuName = "Installers/UserInterfaceInstaller")]

    public class UserInterfaceInstaller : ScriptableObjectInstaller<UserInterfaceInstaller>
    {
    //     [SerializeField] private EqualityComparerComponent _equalityComparer;
    //     [SerializeField] private GroupSortComparerComponent _groupSortComparer;
    //     [SerializeField] private DefaultRuntimeSet _runtimeSet;

    //     //TODO move prefabs to Scriptable Object
    //     [SerializeField] private DefaultActionButton _actionPrefab;
    //     [SerializeField] private DefaultBannerButton _bannerPrefab;
    //     [SerializeField] private DefaultItemButton _itemPrefab;
    //     [SerializeField] private DefaultMiniatureButton _miniaturePrefab;
    //     [SerializeField] private DefaultPortraitButton _portraitPrefab;
    //     [SerializeField] private DefaultInfoButton _infoPrefab;
    //     [SerializeField] private DefaultLogText _logPrefab;
    //     [SerializeField] private UserInterfaceBaseComponent _userInterfacePrefab;

        public override void InstallBindings()
        {
            // Container.Bind<UserInterfaceSignalManager>().AsSingle();
            // Container.Bind<UserInterfaceManager>().AsSingle();
            // Container.Bind<UserInterface>().AsSingle();
            // Container.Bind<IUserInterfaceLogManager>().To<DefaultUserInterfaceLogManager>().AsSingle();
            // Container.Bind<UserInterfaceBase>().AsSingle().OnInstantiated<UserInterfaceBase>(UpdateUserInterfaceBase).NonLazy();
            // Container.Bind<IRuntimeSet<ISelectable>>().To<DefaultRuntimeSet>().FromScriptableObject(_runtimeSet).AsCached().IfNotBound();
            // Container.Bind<IEqualityComparer<ISelectable>>().To<EqualityComparerComponent>().FromComponentInNewPrefab(_equalityComparer).AsCached().IfNotBound();
            // Container.Bind<IComparer<IGrouping<ISelectable, ISelectable>>>().To<GroupSortComparerComponent>().FromComponentInNewPrefab(_groupSortComparer).AsCached().IfNotBound();

            // //External In
            // Container.DeclareSignal<SelectionUpdateSignal>();
            // Container.DeclareSignal<PartyUpdateSignal>();
            // Container.DeclareSignal<SelectableObjectUpdatedSignal>();
            // Container.DeclareSignal<SelectableObjectDeletedSignal>();

            // //Internal
            // Container.DeclareSignal<AlternateSubGroupSignal>();
            // Container.DeclareSignal<MiniatureClickedSignal>();
            // Container.DeclareSignal<PortraitClickedSignal>();
            // Container.DeclareSignal<BannerClickedSignal>();
            // Container.DeclareSignal<MapClickedSignal>();
            // Container.DeclareSignal<ActionClickedSignal>();

            // //External Out
            // Container.DeclareSignal<CameraGoToPositionSignal>();
            // Container.DeclareSignal<IndividualSelectionSignal>();
            // Container.DeclareSignal<ChangeSelectionSignal>();

            // Container.BindSignal<SelectionUpdateSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnSelectionUpdate).FromResolve();
            // Container.BindSignal<PartyUpdateSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnPartyUpdate).FromResolve();
            // Container.BindSignal<SelectableObjectUpdatedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnSelectableObjectUpdatedSignal).FromResolve();
            // Container.BindSignal<SelectableObjectDeletedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnSelectableObjectDeletedSignal).FromResolve();

            // Container.BindSignal<AlternateSubGroupSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnAlternateSubGroup).FromResolve();
            // Container.BindSignal<MiniatureClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnMiniatureClicked).FromResolve();
            // Container.BindSignal<PortraitClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnPortraitClicked).FromResolve();
            // Container.BindSignal<BannerClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnBannerClicked).FromResolve();
            // Container.BindSignal<MapClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnMapClicked).FromResolve();
            // Container.BindSignal<ActionClickedSignal>().ToMethod<UserInterfaceSignalManager>(x => x.OnActionClicked).FromResolve();

            // //TODO user prefab factories
            // Container.BindFactory<DefaultMiniatureButton, DefaultMiniatureButton.Factory>().FromComponentInNewPrefab(_miniaturePrefab);
            // Container.BindFactory<DefaultPortraitButton, DefaultPortraitButton.Factory>().FromComponentInNewPrefab(_portraitPrefab);
            // Container.BindFactory<DefaultBannerButton, DefaultBannerButton.Factory>().FromComponentInNewPrefab(_bannerPrefab);
            // Container.BindFactory<DefaultItemButton, DefaultItemButton.Factory>().FromComponentInNewPrefab(_itemPrefab);
            // Container.BindFactory<DefaultActionButton, DefaultActionButton.Factory>().FromComponentInNewPrefab(_actionPrefab);
            // Container.BindFactory<DefaultInfoButton, DefaultInfoButton.Factory>().FromComponentInNewPrefab(_infoPrefab);
            // Container.BindFactory<DefaultLogText, DefaultLogText.Factory>().FromComponentInNewPrefab(_logPrefab);
        }

        // private void UpdateUserInterfaceBase(InjectContext ctx, UserInterfaceBase userInterfaceBase)
        // {
        //     if (!ctx.Container.IsValidating)
        //     {
        //         GameObject gameObject = Container.InstantiatePrefab(_userInterfacePrefab);
        //         userInterfaceBase.UserInterfaceBaseComponent = gameObject.GetComponent<UserInterfaceBaseComponent>();
        //     }
        // }
    }
}