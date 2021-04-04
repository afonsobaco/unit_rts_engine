using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{
    public class UIContainerBaseManager : MonoBehaviour, IInitializable
    {
        [Inject] protected UIContainer container;
        [Inject] protected PlaceholderFactory<UIContent> factory;

        //Coroutines        
        public IEnumerator AddAllToContainerRoutine(List<UIContentInfo> infoList)
        {
            MarkIsBeingAdded(infoList, true);
            yield return StartCoroutine(BeforeAddAllToContainerAnimation(infoList));
            AddAllToContainer(infoList);
            yield return StartCoroutine(AfterAddAllToContainerAnimation(infoList));
            MarkIsBeingAdded(infoList, false);
        }

        public IEnumerator AddToContainerRoutine(UIContentInfo contentInfo)
        {
            MarkIsBeingAdded(contentInfo, true);
            yield return StartCoroutine(BeforeAddToContainerAnimation(contentInfo));
            AddToContainer(contentInfo);
            yield return StartCoroutine(AfterAddToContainerAnimation(contentInfo));
            MarkIsBeingAdded(contentInfo, false);
        }

        public IEnumerator ClearContainerRoutine()
        {
            List<UIContent> contentList = GetUIContentChildren();
            MarkToBeRemoved(contentList);
            yield return StartCoroutine(BeforeClearContainerAnimation(contentList));
            ClearContainer(contentList);
            yield return StartCoroutine(AfterClearContainerAnimation(contentList));
            RemoveAllMarked(contentList);
        }

        public IEnumerator RemoveAllFromContainerRoutine(List<UIContent> contentList)
        {
            MarkToBeRemoved(contentList);
            yield return StartCoroutine(BeforeRemoveAllFromContainerAnimation(contentList));
            RemoveAllFromContainer(contentList);
            yield return StartCoroutine(AfterRemoveAllFromContainerAnimation(contentList));
            RemoveAllMarked(contentList);
        }

        public IEnumerator RemoveFromContainerRoutine(UIContent content)
        {
            MarkToBeRemoved(content);
            yield return StartCoroutine(BeforeRemoveFromContainerAnimation(content));
            RemoveFromContainer(content);
            yield return StartCoroutine(AfterRemoveFromContainerAnimation(content));
            RemoveAllMarked(content);
        }

        public IEnumerator UpdateContainerRoutine(UIContainerInfo containerInfo)
        {
            yield return StartCoroutine(BeforeUpdateContainerAnimation(containerInfo));
            UpdateContainer(containerInfo);
            yield return StartCoroutine(AfterUpdateContainerAnimation(containerInfo));
        }

        // Before
        public virtual IEnumerator BeforeAddAllToContainerAnimation(List<UIContentInfo> infoList) { yield return null; }
        public virtual IEnumerator BeforeAddToContainerAnimation(UIContentInfo contentInfo) { yield return null; }
        public virtual IEnumerator BeforeClearContainerAnimation(List<UIContent> contentList) { yield return null; }
        public virtual IEnumerator BeforeRemoveAllFromContainerAnimation(List<UIContent> contentList) { yield return null; }
        public virtual IEnumerator BeforeRemoveFromContainerAnimation(UIContent content) { yield return null; }
        public virtual IEnumerator BeforeUpdateContainerAnimation(UIContainerInfo containerInfo) { yield return null; }

        // Main code
        public virtual List<UIContent> AddAllToContainer(List<UIContentInfo> infoList) { return null; }
        public virtual UIContent AddToContainer(UIContentInfo info) { return null; }
        public virtual void ClearContainer(List<UIContent> contentList) { }
        public virtual void RemoveAllFromContainer(List<UIContent> contentList) { }
        public virtual void RemoveFromContainer(UIContent content) { }
        public virtual void UpdateContainer(UIContainerInfo containerInfo) { }

        //After
        public virtual IEnumerator AfterAddAllToContainerAnimation(List<UIContentInfo> infoList) { yield return null; }
        public virtual IEnumerator AfterAddToContainerAnimation(UIContentInfo contentInfo) { yield return null; }
        public virtual IEnumerator AfterClearContainerAnimation(List<UIContent> contentList) { yield return null; }
        public virtual IEnumerator AfterRemoveAllFromContainerAnimation(List<UIContent> contentList) { yield return null; }
        public virtual IEnumerator AfterRemoveFromContainerAnimation(UIContent content) { yield return null; }
        public virtual IEnumerator AfterUpdateContainerAnimation(UIContainerInfo containerInfo) { yield return null; }

        //Others        
        public bool IsContainer(UIContainerBaseSignal signal)
        {
            if (signal == null || signal.ContainerInfo == null)
            {
                return false;
            }
            return signal.ContainerInfo.ContainerId.Equals(container.ContainerId);
        }

        private void MarkToBeRemoved(UIContent content)
        {
            MarkToBeRemoved(new List<UIContent>() { content });
        }

        private void RemoveAllMarked(UIContent content)
        {
            RemoveAllMarked(new List<UIContent>() { content });
        }

        private void MarkToBeRemoved(List<UIContent> contentList)
        {
            contentList.ForEach(x => x.Info.IsBeeingRemoved = true);
        }

        private void RemoveAllMarked(List<UIContent> contentList)
        {
            contentList.ForEach(x => x.Dispose());
        }

        private void MarkIsBeingAdded(UIContentInfo contentInfo, bool value)
        {
            MarkIsBeingAdded(new List<UIContentInfo>() { contentInfo }, value);
        }

        private void MarkIsBeingAdded(List<UIContentInfo> infoList, bool value)
        {
            infoList.ForEach(x => x.IsBeeingAdded = value);
        }
        public virtual void Initialize()
        {
            if (!this.container)
                Debug.LogError("Missing container. Update the installer.");
            if (this.factory == null)
                Debug.LogError("Missing prefab factory. Update the installer.");
            this.transform.SetParent(container.transform, false);
        }

        public virtual List<UIContent> GetUIContentChildren()
        {
            return container.ContentPlaceholder.GetComponentsInChildren<UIContent>().ToList();
        }
    }
}