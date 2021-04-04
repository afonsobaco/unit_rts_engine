using System.Collections;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

namespace RTSEngine.RTSUserInterface
{
    public class UIContainerManager : UIContainerBaseManager
    {
        [Inject] protected SignalBus signalBus;

        // Signals
        public void AddAllContentSignal(UIAddAllContentSignal signal)
        {
            if (IsContainer(signal)) StartCoroutine(AddAllToContainerRoutine(signal.InfoList, true, true));
        }
        public void AddContentSignal(UIAddContentSignal signal)
        {
            if (IsContainer(signal)) StartCoroutine(AddToContainerRoutine(signal.Info, true, true));
        }
        public void ClearContainerSignal(UIClearContainerSignal signal)
        {
            if (IsContainer(signal)) StartCoroutine(ClearContainerRoutine(true, true));
        }
        public void RemoveAllContentSignal(UIRemoveAllContentSignal signal)
        {
            if (IsContainer(signal)) StartCoroutine(RemoveAllFromContainerRoutine(signal.ContentList, true, true));
        }
        public void RemoveContentSignal(UIRemoveContentSignal signal)
        {
            if (IsContainer(signal)) StartCoroutine(RemoveFromContainerRoutine(signal.Content, true, true));
        }
        public void UpdateContainerSignal(UIUpdateContainerSignal signal)
        {
            if (IsContainer(signal)) StartCoroutine(UpdateContainerRoutine(signal.ContainerInfo, true, true));
        }

        public virtual void GlobalContainerSignal(UIGlobalContainerSignal signal) { }

        public override List<UIContent> AddAllToContainer(List<UIContentInfo> infoList)
        {
            List<UIContent> result = new List<UIContent>();
            foreach (var info in infoList)
            {
                UIContent item = AddToContainer(info);
                if (item)
                    result.Add(item);
                else
                    break;
            }
            return result;
        }

        public override UIContent AddToContainer(UIContentInfo info)
        {
            var newContent = factory.Create();
            newContent.Info = info;
            info.Content = newContent;
            return newContent;
        }

        public virtual IEnumerator BeforeAny() { yield return null; }

        public virtual IEnumerator AfterAny()
        {
            GetUIContentChildren().ForEach(x => x.UpdateAppearance());
            yield return null;
        }

        public IEnumerator AddAllToContainerRoutine(List<UIContentInfo> infoList, bool executeBeforeAny, bool executeAfterAny)
        {
            if (executeBeforeAny) yield return StartCoroutine(BeforeAny());
            yield return StartCoroutine(AddAllToContainerRoutine(infoList));
            if (executeAfterAny) yield return StartCoroutine(AfterAny());
        }

        public IEnumerator AddToContainerRoutine(UIContentInfo contentInfo, bool executeBeforeAny, bool executeAfterAny)
        {
            if (executeBeforeAny) yield return StartCoroutine(BeforeAny());
            yield return StartCoroutine(AddToContainerRoutine(contentInfo));
            if (executeAfterAny) yield return StartCoroutine(AfterAny());
        }
        public IEnumerator ClearContainerRoutine(bool executeBeforeAny, bool executeAfterAny)
        {
            if (executeBeforeAny) yield return StartCoroutine(BeforeAny());
            yield return StartCoroutine(ClearContainerRoutine());
            if (executeAfterAny) yield return StartCoroutine(AfterAny());
        }

        public IEnumerator RemoveAllFromContainerRoutine(List<UIContent> contentList, bool executeBeforeAny, bool executeAfterAny)
        {
            if (executeBeforeAny) yield return StartCoroutine(BeforeAny());
            yield return StartCoroutine(RemoveAllFromContainerRoutine(contentList));
            if (executeAfterAny) yield return StartCoroutine(AfterAny());
        }
        public IEnumerator RemoveFromContainerRoutine(UIContent content, bool executeBeforeAny, bool executeAfterAny)
        {
            if (executeBeforeAny) yield return StartCoroutine(BeforeAny());
            yield return StartCoroutine(RemoveFromContainerRoutine(content));
            if (executeAfterAny) yield return StartCoroutine(AfterAny());
        }
        public IEnumerator UpdateContainerRoutine(UIContainerInfo containerInfo, bool executeBeforeAny, bool executeAfterAny)
        {
            if (executeBeforeAny) yield return StartCoroutine(BeforeAny());
            yield return StartCoroutine(UpdateContainerRoutine(containerInfo));
            if (executeAfterAny) yield return StartCoroutine(AfterAny());
        }

    }
}