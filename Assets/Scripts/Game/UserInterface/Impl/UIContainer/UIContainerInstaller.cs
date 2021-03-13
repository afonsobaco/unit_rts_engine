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
        }

        void InstallUIContainer(DiContainer subContainer)
        {
            subContainer.Bind<UIContainerFacade>().AsSingle();
            subContainer.BindInterfacesAndSelfTo<UIContainer>().FromComponentInNewPrefab(_containerPrefab).AsSingle();
            subContainer.BindInterfacesAndSelfTo<UIContainerManager>().FromComponentInNewPrefab(_manager).AsSingle();
            subContainer.BindFactory<UIContent, PlaceholderFactory<UIContent>>().FromComponentInNewPrefab(_contentPrefab);
            subContainer.BindSignal<UIAddContentSignal>().ToMethod<UIContainerManager>(x => x.AddContent).FromResolve();
            subContainer.BindSignal<UIRemoveContentSignal>().ToMethod<UIContainerManager>(x => x.RemoveContent).FromResolve();
        }
    }
}