using Zenject;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{
    public class UIContainerSubContainerInstaller : Installer<UIContainerSubContainerInstaller>
    {
        //     private UserInterfaceContainer _uiContainer;
        //     private UserInterfaceDefaultContainerManager _uiManager;

        //     public UIContainerSubContainerInstaller(UserInterfaceContainer uiContainer)
        //     {
        //         this._uiContainer = uiContainer;
        //         this._uiManager = this._uiContainer.transform.GetComponent<UserInterfaceDefaultContainerManager>();
        //     }

        //     public override void InstallBindings()
        //     {
        //         Container.Bind<UIContainerFacade>().AsSingle();
        //         // Container.BindFactory<UserInterfaceContainer, UIContainerFacade, UIContainerFacade.Factory>().FromSubContainerResolve().ByNewPrefabInstaller<UIContainerSubContainerInstaller>(_containerPrefab);

        //         if (this._uiManager)
        //         {
        //             Container.Bind<IUserInterfaceContainerManager>().FromInstance(this._uiManager).AsCached();
        //         }
        //         else
        //         {
        //             Container.Bind<IUserInterfaceContainerManager>().To<UserInterfaceDefaultContainerManager>().FromNewComponentOnRoot().AsCached();
        //             this._uiManager = Container.Resolve<UserInterfaceDefaultContainerManager>();
        //         }

        //         Container.BindSignal<AddContentSignal>().ToMethod((s) => AddContent(s));
        //         Container.BindSignal<RemoveContentSignal>().ToMethod((s) => RemoveContent(s));
        //     }

        //     private void RemoveContent(RemoveContentSignal signal)
        //     {
        //         if (this._uiManager && IsContainer(signal.Component.ContentId))
        //             _uiContainer.ContainerManager.RemoveContent(signal.Component);
        //     }

        //     private void AddContent(AddContentSignal signal)
        //     {
        //         if (this._uiManager && IsContainer(signal.Content.ContentId))
        //             _uiContainer.ContainerManager.AddContent(signal.Content);
        //     }

        //     private bool IsContainer(string id)
        //     {
        //         if (_uiContainer && _uiContainer.ContentPrefab)
        //             return _uiContainer.ContentPrefab.ContentId.Equals(id);
        //         return false;
        //     }

        public override void InstallBindings()
        {
            Container.Bind<UIContainerFacade>().AsSingle();
        }
    }
}