using System;
using UnityEngine;
using System.Collections.Generic;
using RTSEngine.RTSUserInterface.Utils;
using Zenject;

namespace RTSEngine.RTSUserInterface
{

    public class UserInterfaceDefaultContainerManager : MonoBehaviour, IUserInterfaceContainerManager
    {
        protected UserInterfaceContainer _containerPrefab;
        protected PlaceholderFactory<UserInterfaceContentComponent> _factory;

        [Inject]
        public void Constructor(PlaceholderFactory<UserInterfaceContentComponent> factory, UserInterfaceContainer containerPrefab)
        {
            this._factory = factory;
            this._containerPrefab = containerPrefab;
        }

        public virtual void AddContent(UserInterfaceContent content)
        {

            var component = _factory.Create();
            component.Content = content;
            component.Content.UpdateAppearance();
        }
        public virtual void RemoveContent(UserInterfaceContentComponent component)
        {
            GameObject.Destroy(component.gameObject);
        }

        public virtual List<UserInterfaceContentComponent> GetAllContentComponents()
        {
            List<UserInterfaceContentComponent> result = new List<UserInterfaceContentComponent>();
            foreach (Transform child in _containerPrefab.transform)
            {
                var comp = child.GetComponent<UserInterfaceContentComponent>();
                if (comp)
                {
                    result.Add(comp);
                }
            }
            return result;
        }
    }
}
