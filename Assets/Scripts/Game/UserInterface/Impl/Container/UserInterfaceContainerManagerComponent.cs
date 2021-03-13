using System;
using UnityEngine;
using System.Collections.Generic;
using RTSEngine.RTSUserInterface.Utils;
using Zenject;

namespace RTSEngine.RTSUserInterface
{

    public class UserInterfaceContainerManagerComponent : MonoBehaviour, IUserInterfaceContainerManager, IInitializable
    {
        [Inject] private UserInterfaceContainer _containerPrefab;
        [Inject] private PlaceholderFactory<UserInterfaceContentComponent> _factory;
        private UserInterfaceDefaultContainerManager defaultContainerManager;

        public void Initialize()
        {
            Debug.Log("asdasd");
            defaultContainerManager = new UserInterfaceDefaultContainerManager(_factory, _containerPrefab);
        }

        public virtual void AddContent(UserInterfaceContent content)
        {
            defaultContainerManager.AddContent(content);
        }

        public List<UserInterfaceContentComponent> GetAllContentComponents()
        {
            return defaultContainerManager.GetAllContentComponents();
        }

        public virtual void RemoveContent(UserInterfaceContentComponent component)
        {
            defaultContainerManager.RemoveContent(component);
        }


    }
}
