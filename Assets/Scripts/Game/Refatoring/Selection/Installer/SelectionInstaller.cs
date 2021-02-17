using UnityEngine;
using Zenject;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class SelectionInstaller : MonoInstaller
    {
        [SerializeField] private RuntimeSetComponent runtimeSet;
        [SerializeField] private ModifiersComponent modifiers;

        public override void InstallBindings()
        {
            Container.Bind<SelectionSignalManager>().AsSingle();
            Container.Bind<SelectionManager>().AsSingle();
            Container.Bind<Selection>().AsSingle();
            Container.Bind<ModifiersInterface>().AsSingle();
            Container.Bind<IAreaSelectionType>().To<PolyAreaSelectionType>().AsSingle();
            Container.Bind<IAreaSelection>().To<AreaSelection>().AsSingle();
            Container.Bind<IGroupSelection>().To<GroupSelection>().AsSingle();
            Container.Bind<IIndividualSelection>().To<IndividualSelection>().AsSingle();
            Container.Bind<ISelectionModifier[]>().FromMethod(GetModifiers);
            Container.Bind<IRuntimeSet<ISelectable>>().FromMethod(GetMainList);
            Container.DeclareSignal<SelectableObjectCreatedSignal>();
            Container.DeclareSignal<SelectableObjectDeletedSignal>();
            Container.DeclareSignal<AreaSelectionSignal>();
            Container.DeclareSignal<GroupSelectionSignal>();
            Container.DeclareSignal<IndividualSelectionSignal>();
            Container.BindSignal<SelectableObjectCreatedSignal>().ToMethod<SelectionSignalManager>(x => x.OnSelectableObjectCreatedSignal).FromResolve();
            Container.BindSignal<SelectableObjectDeletedSignal>().ToMethod<SelectionSignalManager>(x => x.OnSelectableObjectDeletedSignal).FromResolve();
            Container.BindSignal<AreaSelectionSignal>().ToMethod<SelectionSignalManager>(x => x.OnAreaSignal).FromResolve();
            Container.BindSignal<GroupSelectionSignal>().ToMethod<SelectionSignalManager>(x => x.OnGroupSignal).FromResolve();
            Container.BindSignal<IndividualSelectionSignal>().ToMethod<SelectionSignalManager>(x => x.OnIndividualSignal).FromResolve();
        }

        private ISelectionModifier[] GetModifiers()
        {
            return modifiers.GetComponents<ISelectionModifier>();
        }

        private IRuntimeSet<ISelectable> GetMainList()
        {
            return runtimeSet.GetComponent<IRuntimeSet<ISelectable>>();
        }
    }
}