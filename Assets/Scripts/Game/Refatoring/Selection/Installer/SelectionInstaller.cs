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
            Container.Bind<IIndividualSelection>().To<IndividualSelection>().AsSingle();
            Container.Bind<IModifiersComponent>().To<Modifiers>().FromScriptableObject(_modifiersComponent).AsSingle();
            Container.Bind<IViewportHelper>().To<ViewportHelper>().FromScriptableObject(_viewportHelper).AsSingle();
            Container.Bind<IRuntimeSet<ISelectable>>().To<DefaultRuntimeSet>().FromScriptableObject(_runtimeSet).AsCached().IfNotBound();
            Container.Bind<IEqualityComparer<ISelectable>>().To<EqualityComparerComponent>().FromComponentInNewPrefab(_equalityComparer).AsCached().IfNotBound();
            Container.Bind<IComparer<IGrouping<ISelectable, ISelectable>>>().To<GroupSortComparerComponent>().FromComponentInNewPrefab(_groupSortComparer).AsCached().IfNotBound();

            foreach (var item in _modifiersComponent.GetModifiers())
            {
                Container.QueueForInject(item);
            }

            Container.DeclareSignal<SelectableObjectCreatedSignal>();
            Container.DeclareSignal<SelectableObjectUpdatedSignal>();
            Container.DeclareSignal<SelectableObjectDeletedSignal>();
            Container.DeclareSignal<AreaSelectionSignal>();
            Container.DeclareSignal<IndividualSelectionSignal>();
            Container.DeclareSignal<ChangeSelectionSignal>();
            Container.DeclareSignal<SelectionUpdateSignal>();

            Container.BindSignal<SelectableObjectCreatedSignal>().ToMethod<SelectionSignalManager>(x => x.OnSelectableObjectCreatedSignal).FromResolve();
            Container.BindSignal<SelectableObjectUpdatedSignal>().ToMethod<SelectionSignalManager>(x => x.OnSelectableObjectUpdatedSignal).FromResolve();
            Container.BindSignal<SelectableObjectDeletedSignal>().ToMethod<SelectionSignalManager>(x => x.OnSelectableObjectDeletedSignal).FromResolve();
            Container.BindSignal<AreaSelectionSignal>().ToMethod<SelectionSignalManager>(x => x.OnAreaSelectionSignal).FromResolve();
            Container.BindSignal<ChangeSelectionSignal>().ToMethod<SelectionSignalManager>(x => x.OnChangeSelectionSignal).FromResolve();
            Container.BindSignal<IndividualSelectionSignal>().ToMethod<SelectionSignalManager>(x => x.OnIndividualSelectionSignal).FromResolve();

        }
    }
}