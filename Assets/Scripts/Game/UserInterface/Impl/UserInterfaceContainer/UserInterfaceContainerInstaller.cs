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
        [SerializeField] private UserInterfaceDefaultContainerManager _containerManager;

        private UserInterfaceContainer _containerInstance;

        public override void InstallBindings()
        {
            Container.BindFactory<UserInterfaceContainer, UserInterfaceContentComponent, PlaceholderFactory<UserInterfaceContainer, UserInterfaceContentComponent>>().FromSubContainerResolve().ByNewPrefabInstaller<UIContainerSubContainerInstaller>(_containerPrefab);

            Container.DeclareSignal<AddContentSignal>();
            Container.DeclareSignal<RemoveContentSignal>();

            Container.BindSignal<AddContentSignal>().ToMethod((s) => AddContent(s));
            Container.BindSignal<RemoveContentSignal>().ToMethod((s) => RemoveContent(s));
        }

        private void RemoveContent(RemoveContentSignal signal)
        {
            var manager = Container.Resolve<IUserInterfaceContainerManager>();
            if (_contentPrefab.ContentId.Equals(signal.Component.ContentId))
                manager.RemoveContent(signal.Component);
        }

        private void AddContent(AddContentSignal signal)
        {
            var manager = Container.Resolve<IUserInterfaceContainerManager>();
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