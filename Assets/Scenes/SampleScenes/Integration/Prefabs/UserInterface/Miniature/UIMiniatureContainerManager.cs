using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface;
using RTSEngine.Signal;
using System;
using Zenject;
using RTSEngine.Core;

namespace RTSEngine.Integration.Scene
{
    public class UIMiniatureContainerManager : UILimitedContainerManager
    {
        private List<IntegrationSceneObject> selection = new List<IntegrationSceneObject>();
        private UIMiniatureHighlightManager miniatureHighlightManager;

        [Inject]
        private IEqualityComparer<ISelectable> _equalityComparer;

        private bool isUISelection;

        private void Start()
        {
            miniatureHighlightManager = new UIMiniatureHighlightManager(_equalityComparer);
            signalBus.Subscribe<SelectionUpdateSignal>(SelectionUpdated);
        }

        private void SelectionUpdated(SelectionUpdateSignal signal)
        {
            if (signal.Selection.Length > 0)
            {
                isUISelection = signal.IsUISelection;
                StartCoroutine(AdjustContainerForNewSelection(UIUtils.CreateContentInfoBySelection(signal.Selection)));
            }
            else
                StartCoroutine(ClearContainerRoutine());
        }

        private IEnumerator AdjustContainerForNewSelection(List<UIContentInfo> infoList)
        {
            List<UIContent> children = GetUIContentChildren();
            List<UIContent> ToBeRemoved = GetToBeRemoved(infoList, children);
            List<UIContentInfo> ToBeAdded = GetToBeAdded(infoList, children);
            if (ToBeRemoved.Count > 0) yield return StartCoroutine(RemoveAllFromContainerRoutine(ToBeRemoved));
            if (ToBeAdded.Count > 0) yield return StartCoroutine(AddAllToContainerRoutine(ToBeAdded));
            SortFinalInfoList(infoList);
            UpdateMiniatureHighlight(infoList);
            yield return StartCoroutine(AfterAny());
        }

        private List<UIContent> GetToBeRemoved(List<UIContentInfo> infoList, List<UIContent> children)
        {
            var selectables = UIUtils.GetSelectableFromContentInfo(infoList);
            return children.Where(x => !selectables.Contains(UIUtils.GetSelectable(x.Info))).ToList();
        }

        private List<UIContentInfo> GetToBeAdded(List<UIContentInfo> infoList, List<UIContent> children)
        {
            List<IntegrationSceneObject> childrenSelectables = UIUtils.GetSelectableFromContent(children);
            return infoList.Where(x => !childrenSelectables.Contains(UIUtils.GetSelectable(x))).ToList();
        }

        private void SortFinalInfoList(List<UIContentInfo> infoList)
        {
            infoList.Where(x => x.Content).ToList().ForEach(x => x.Content.transform.SetSiblingIndex(infoList.IndexOf(x)));
        }

        private void UpdateMiniatureHighlight(List<UIContentInfo> infoList)
        {
            miniatureHighlightManager.UpdateMiniaturesHighlight(UIUtils.GetSelectableFromContentInfo(infoList), isUISelection);
        }

        public override void UpdateContainer(UIContainerInfo containerInfo)
        {
            List<UIContent> contentList = GetUIContentChildren();
            miniatureHighlightManager.UpdateHighlighted(containerInfo, contentList);
            miniatureHighlightManager.UpdateMiniaturesHighlight(UIUtils.GetSelectableFromContent(contentList), true);
        }

        public override UIContent AddToContainer(UIContentInfo info)
        {
            var selectable = UIUtils.GetSelectable(info);
            if (!selection.Contains(selectable))
            {
                selection.Add(selectable);
                UIContent uIContent = base.AddToContainer(info);
                return uIContent;
            }
            return null;
        }

        public override IEnumerator AfterRemoveAllFromContainerAnimation(List<UIContent> contentList)
        {
            yield return StartCoroutine(base.AfterRemoveAllFromContainerAnimation(contentList));
            contentList.ForEach(x => selection.Remove(UIUtils.GetSelectable(x.Info)));
        }

        public override IEnumerator AfterRemoveFromContainerAnimation(UIContent content)
        {
            yield return StartCoroutine(base.AfterRemoveFromContainerAnimation(content));
            selection.Remove(UIUtils.GetSelectable(content.Info));
        }

        public override IEnumerator AfterClearContainerAnimation(List<UIContent> contentList)
        {
            yield return StartCoroutine(base.AfterClearContainerAnimation(contentList));
            contentList.ForEach(x => selection.Remove(UIUtils.GetSelectable(x.Info)));
        }

        public override IEnumerator AfterUpdateContainerAnimation(UIContainerInfo containerInfo)
        {
            yield return StartCoroutine(base.AfterUpdateContainerAnimation(containerInfo));
            signalBus.Fire(new UIGlobalContainerSignal() { Content = new UIUpdateHighlightSignalContent() { Highlighted = miniatureHighlightManager.Highlighted } });
        }

        public override List<UIContent> GetUIContentChildren()
        {
            var contentList = base.GetUIContentChildren();
            contentList.RemoveAll(x => x.Info.IsBeeingRemoved);
            return contentList;
        }
    }
}