using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Commons;


namespace RTSEngine.Refactoring
{

    [CreateAssetMenu(fileName = "SelectionInstallerSO", menuName = "Installers/SelectionInstallerSO")]
    public class SelectionInstallerSO : ScriptableObjectInstaller<SelectionInstallerSO>
    {
        [SerializeField] private EqualityComparerComponent equalityComparer;
        [SerializeField] private GroupingComparerComponent groupingComparer;
        [SerializeField] private RuntimeSetComponent runtimeSetComponent;
        [Space]
        [SerializeField] private ModifiersSO modifiersComponent;
        [SerializeField] private ViewportHelper viewportHelper;

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
            Container.Bind<IRuntimeSet<ISelectable>>().To<RuntimeSetComponent>().FromComponentInNewPrefab(runtimeSetComponent).AsSingle().NonLazy();
            Container.Bind<IModifiersComponent>().To<ModifiersSO>().FromScriptableObject(modifiersComponent).AsSingle();
            Container.Bind<IViewportHelper>().To<ViewportHelper>().FromScriptableObject(viewportHelper).AsSingle();
            Container.Bind<IEqualityComparer<ISelectable>>().To<EqualityComparerComponent>().FromComponentInNewPrefab(equalityComparer).AsSingle();
            Container.Bind<IComparer<IGrouping<ISelectable, ISelectable>>>().To<GroupingComparerComponent>().FromComponentInNewPrefab(groupingComparer).AsSingle();

            foreach (var item in modifiersComponent.GetModifiers())
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