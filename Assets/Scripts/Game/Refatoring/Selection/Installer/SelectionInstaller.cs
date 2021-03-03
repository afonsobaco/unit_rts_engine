using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Commons;

namespace RTSEngine.Refactoring
{

    [CreateAssetMenu(fileName = "SelectionInstaller", menuName = "Installers/SelectionInstaller")]
    public class SelectionInstaller : ScriptableObjectInstaller<SelectionInstaller>
    {
        [SerializeField] private EqualityComparerComponent _equalityComparer;
        [SerializeField] private GroupSortComparerComponent _groupSortComparer;
        [SerializeField] private DefaultRuntimeSet _runtimeSet;

        [Space]
        [SerializeField] private Modifiers _modifiersComponent;
        [SerializeField] private ViewportHelper _viewportHelper;

        public override void InstallBindings()
        {
            Container.Bind<SelectionSignalManager>().AsSingle();
            Container.Bind<SelectionManager>().AsSingle();
            Container.Bind<Selection>().AsSingle();
            Container.Bind<ModifiersInterface>().AsSingle();
            Container.Bind<IAreaSelectionType>().To<PolyAreaSelectionType>().AsSingle();
            Container.Bind<IAreaSelection>().To<AreaSelection>().AsSingle();
            Container.Bind<IPartySelection>().To<PartySelection>().AsSingle();
            Container.Bind<IIndividualSelection>().To<IndividualSelection>().AsSingle();
            Container.Bind<IModifiersComponent>().To<Modifiers>().FromScriptableObject(_modifiersComponent).AsSingle();
            Container.Bind<IViewportHelper>().To<ViewportHelper>().FromScriptableObject(_viewportHelper).AsSingle();
            Container.Bind<IRuntimeSet<ISelectable>>().To<DefaultRuntimeSet>().FromScriptableObject(_runtimeSet).AsSingle().IfNotBound();
            Container.Bind<IEqualityComparer<ISelectable>>().To<EqualityComparerComponent>().FromComponentInNewPrefab(_equalityComparer).AsSingle().IfNotBound();
            Container.Bind<IComparer<IGrouping<ISelectable, ISelectable>>>().To<GroupSortComparerComponent>().FromComponentInNewPrefab(_groupSortComparer).AsSingle().IfNotBound();

            foreach (var item in _modifiersComponent.GetModifiers())
            {
                Container.QueueForInject(item);
            }

            Container.DeclareSignal<SelectableObjectCreatedSignal>();
            Container.DeclareSignal<SelectableObjectDeletedSignal>();
            Container.DeclareSignal<AreaSelectionSignal>();
            Container.DeclareSignal<PartySelectionSignal>();
            Container.DeclareSignal<IndividualSelectionSignal>();
            Container.DeclareSignal<SelectionUpdateSignal>();

            Container.BindSignal<SelectableObjectCreatedSignal>().ToMethod<SelectionSignalManager>(x => x.OnSelectableObjectCreatedSignal).FromResolve();
            Container.BindSignal<SelectableObjectDeletedSignal>().ToMethod<SelectionSignalManager>(x => x.OnSelectableObjectDeletedSignal).FromResolve();
            Container.BindSignal<AreaSelectionSignal>().ToMethod<SelectionSignalManager>(x => x.OnAreaSignal).FromResolve();
            Container.BindSignal<PartySelectionSignal>().ToMethod<SelectionSignalManager>(x => x.OnPartySignal).FromResolve();
            Container.BindSignal<IndividualSelectionSignal>().ToMethod<SelectionSignalManager>(x => x.OnIndividualSignal).FromResolve();

        }
    }
}