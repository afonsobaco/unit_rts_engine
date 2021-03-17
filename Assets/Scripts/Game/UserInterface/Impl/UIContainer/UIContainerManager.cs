using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;
using System.Linq;
using System;

namespace RTSEngine.RTSUserInterface
{
    public class UIContainerManager : MonoBehaviour, IInitializable
    {
        [Inject] protected UIContainer container;
        [Inject] protected PlaceholderFactory<UIContent> factory;

        public void Initialize()
        {
            this.transform.SetParent(container.transform, false);
        }

        public void AddContent(UIAddContentSignal signal)
        {
            if (signal.Info.ContainerId.Equals(container.ContainerId))
            {
                AddToContainer(signal);
            }
        }

        public virtual UIContent AddToContainer(UIAddContentSignal signal)
        {
            var newContent = factory.Create();
            newContent.Info = signal.Info;
            newContent.transform.SetParent(container.ContentPlaceholder.transform, false);
            newContent.UpdateAppearance();
            StartCoroutine(PosAddToContainer(newContent));
            return newContent;
        }

        public virtual IEnumerator PosAddToContainer(UIContent content)
        {
            yield return null;
        }

        public void RemoveContent(UIRemoveContentSignal signal)
        {
            if (signal.Content.Info.ContainerId.Equals(container.ContainerId))
            {
                RemoveFromContainer(signal);
            }
        }

        public virtual void RemoveFromContainer(UIRemoveContentSignal signal)
        {
            StartCoroutine(BeforeRemoveFromContainer(signal.Content));
        }

        public virtual IEnumerator BeforeRemoveFromContainer(UIContent content)
        {
            yield return null;
            Destroy(content.gameObject);
        }


        public List<UIContent> GetUIContentChildren()
        {
            return container.ContentPlaceholder.GetComponentsInChildren<UIContent>().Where(x => x.Info.ContainerId.Equals(container.ContainerId)).ToList();
        }
    }
}