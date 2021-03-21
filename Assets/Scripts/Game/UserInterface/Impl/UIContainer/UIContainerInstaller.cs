using System;
using UnityEngine;
using Zenject;

namespace RTSEngine.RTSUserInterface
{
    [CreateAssetMenu(fileName = "UIContainerInstaller", menuName = "RTS Engine/UIContainerInstaller", order = 0)]
    public class UIContainerInstaller : ScriptableObjectInstaller<UIContainerInstaller>
    {

        [SerializeField] private UIContainer _containerPrefab;
        [SerializeField] private UIContent _contentPrefab;
        [SerializeField] private UIContainerManager _manager;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIContainerFacade>().FromSubContainerResolve().ByMethod(InstallUIContainer).AsCached();

            Container.DeclareSignal<UIAddContentSignal>();
            Container.DeclareSignal<UIRemoveContentSignal>();
            Container.DeclareSignal<UIAddAllContentSignal>();
            Container.DeclareSignal<UIRemoveAllContentSignal>();
            Container.DeclareSignal<UIClearContainerSignal>();
            Container.DeclareSignal<UIUpdateContainerSignal>();
        }

        void InstallUIContainer(DiContainer subContainer)
        {
            subContainer.Bind<UIContainerFacade>().AsSingle();
            subContainer.BindInterfacesAndSelfTo<UIContainer>().FromComponentInNewPrefab(_containerPrefab).AsSingle();
            subContainer.BindInterfacesAndSelfTo<UIContainerManager>().FromComponentInNewPrefab(_manager).AsSingle();
            subContainer.BindFactory<UIContent, PlaceholderFactory<UIContent>>().FromMonoPoolableMemoryPool(x => x.FromComponentInNewPrefab(_contentPrefab).UnderTransform(GetContainerParent));
            subContainer.BindSignal<UIAddContentSignal>().ToMethod<UIContainerManager>(x => x.AddContentSignal).FromResolve();
            subContainer.BindSignal<UIRemoveContentSignal>().ToMethod<UIContainerManager>(x => x.RemoveContentSignal).FromResolve();
            subContainer.BindSignal<UIAddAllContentSignal>().ToMethod<UIContainerManager>(x => x.AddAllContentSignal).FromResolve();
            subContainer.BindSignal<UIRemoveAllContentSignal>().ToMethod<UIContainerManager>(x => x.RemoveAllContentSignal).FromResolve();
            subContainer.BindSignal<UIClearContainerSignal>().ToMethod<UIContainerManager>(x => x.ClearContainerSignal).FromResolve();
            subContainer.BindSignal<UIUpdateContainerSignal>().ToMethod<UIContainerManager>(x => x.UpdateContainerSignal).FromResolve();
        }

        private Transform GetContainerParent(InjectContext context)
        {
            var container = context.Container.Resolve<UIContainer>();
            return container.ContentPlaceholder.transform;
        }
    }
}