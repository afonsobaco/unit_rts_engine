using System.Runtime.InteropServices;
using System;
using UnityEngine;
using Zenject;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public class SelectionInstaller : MonoInstaller
    {
        [SerializeField] private RuntimeSetComponent runtimeSet;

        public override void InstallBindings()
        {
            Container.Bind<SelectionManager>().AsSingle();
            Container.Bind<SelectionInterface>().AsSingle();
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
            Container.BindSignal<SelectableObjectCreatedSignal>().ToMethod<SelectionManager>(x => x.OnSelectableObjectCreatedSignal).FromResolve();
            Container.BindSignal<SelectableObjectDeletedSignal>().ToMethod<SelectionManager>(x => x.OnSelectableObjectDeletedSignal).FromResolve();
            Container.BindSignal<AreaSelectionSignal>().ToMethod<SelectionManager>(x => x.OnAreaSignal).FromResolve();
            Container.BindSignal<GroupSelectionSignal>().ToMethod<SelectionManager>(x => x.OnGroupSignal).FromResolve();
            Container.BindSignal<IndividualSelectionSignal>().ToMethod<SelectionManager>(x => x.OnIndividualSignal).FromResolve();
        }

        private ISelectionModifier[] GetModifiers()
        {
            return this.GetComponents<ISelectionModifier>();
        }

        private IRuntimeSet<ISelectable> GetMainList()
        {
            return runtimeSet.GetComponent<IRuntimeSet<ISelectable>>();
        }
    }
}