using UnityEngine;
using Zenject;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Utils;

namespace RTSEngine.Refactoring
{
    public class SelectionInstaller : MonoInstaller
    {
        [SerializeField] private RuntimeSetComponent runtimeSetComponent;
        [SerializeField] private ModifiersComponent modifiersComponent;

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
            Container.Bind<IModifiersComponent>().To<ModifiersComponent>().FromComponentInNewPrefab(modifiersComponent).AsSingle();
            Container.QueueForInject(modifiersComponent);

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


        // public RuntimeSetComponent GetRuntimeSet()
        // {
        //     return runtimeSet;
        // }
    }
}