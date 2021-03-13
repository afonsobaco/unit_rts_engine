using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface.Utils;
using Zenject;
using System;

namespace RTSEngine.RTSUserInterface
{

    [CreateAssetMenu(fileName = "UserInterfaceContainer", menuName = "RTS Engine/UserInterfaceContainer", order = 0)]

    public class UserInterfaceContainerInstaller : ScriptableObjectInstaller<UserInterfaceContainerInstaller>
    {
        [SerializeField] private UserInterfaceContainer _containerPrefab;
        [SerializeField] private UserInterfaceContentComponent _contentPrefab;
        [SerializeField] private UserInterfaceContainerManagerComponent _containerManager;

        // [SerializeField] private string contentId;
        private UserInterfaceContainer _containerInstance;

        private DiContainer _subContainer;

        public override void InstallBindings()
        {
            _subContainer = Container.CreateSubContainer();
            _subContainer.Bind<UserInterfaceContainer>()
                .FromComponentInNewPrefab(_containerPrefab)
                .AsSingle()
                .OnInstantiated(UpdateUserInterfaceContainer);
            _subContainer.BindFactory<UserInterfaceContentComponent, PlaceholderFactory<UserInterfaceContentComponent>>()
                .FromComponentInNewPrefab(_contentPrefab)
                .OnInstantiated<UserInterfaceContentComponent>(SetParent);

            _subContainer.Resolve<UserInterfaceContainer>(); //force nonlazy on subcontainer;

            Container.DeclareSignal<AddContentSignal>();
            Container.BindSignal<AddContentSignal>()
                .ToMethod((s) => AddContent(s));

            Container.DeclareSignal<RemoveContentSignal>();
            Container.BindSignal<RemoveContentSignal>()
                .ToMethod((s) => RemoveContent(s));
            AddManager(_subContainer);
        }

        private void AddManager(DiContainer _subContainer)
        {

            if (_containerManager)
            {
                Container.Bind<IUserInterfaceContainerManager>().To<UserInterfaceContainerManagerComponent>().FromComponentInNewPrefab(_containerManager).AsCached().MoveIntoAllSubContainers();
                
            }
            else
            {
                _subContainer.Bind<IUserInterfaceContainerManager>().To<UserInterfaceDefaultContainerManager>().AsCached();
            }
        }

        private void RemoveContent(RemoveContentSignal signal)
        {
            var manager = _subContainer.Resolve<IUserInterfaceContainerManager>();
            if (_contentPrefab.ContentId.Equals(signal.Component.ContentId))
                manager.RemoveContent(signal.Component);
        }

        private void AddContent(AddContentSignal signal)
        {
            var manager = _subContainer.Resolve<IUserInterfaceContainerManager>();
            if (_contentPrefab.ContentId.Equals(signal.Content.ContentId))
                manager.AddContent(signal.Content);
        }

        private void UpdateUserInterfaceContainer(InjectContext ctx, object arg2)
        {
            if (!ctx.Container.IsValidating)
            {
                this._containerInstance = arg2 as UserInterfaceContainer;
                _containerInstance.AddToCanvas();
                _containerInstance.Clear();
            }
        }

        private void SetParent(InjectContext ctx, UserInterfaceContentComponent prefab)
        {
            prefab.transform.SetParent(_containerInstance.Panel, false);
        }
    }
}