using Zenject;
using UnityEngine;
using System;

namespace RTSEngine.RTSUserInterface
{
    public class UIContainerManager : MonoBehaviour, IInitializable
    {
        [Inject] private UIContainer container;
        [Inject] PlaceholderFactory<UIContent> factory;

        public void Initialize()
        {
            Debug.Log("Initialized Manager for " + container.ContainerId);
            this.transform.SetParent(container.transform, false);
        }

        public void AddContent(UIAddContentSignal signal)
        {
            if (signal.Info.ContainerId.Equals(container.ContainerId))
            {
                var newContent = factory.Create();
                newContent.Info = signal.Info;
                newContent.transform.SetParent(container.Placeholder.transform, false);
            }
        }

        public void RemoveContent(UIRemoveContentSignal signal)
        {
            if (signal.Content.Info.ContainerId.Equals(container.ContainerId))
            {
                Destroy(signal.Content.gameObject);
            }
        }
    }
}