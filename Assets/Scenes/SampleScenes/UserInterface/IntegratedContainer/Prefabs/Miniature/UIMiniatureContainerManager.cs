﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.RTSUserInterface;
using System;

namespace RTSEngine.RTSUserInterface.Scene

{
    public class UIMiniatureContainerManager : UILimitedContainerManager
    {
        private UISceneIntegratedSelectable _highlighted;
        private List<UISceneIntegratedSelectable> selection = new List<UISceneIntegratedSelectable>();
        public UISceneIntegratedSelectable Highlighted { get => _highlighted; set => _highlighted = value; }

        public override IEnumerator BeforeAddAllToContainerAnimation(List<UIContentInfo> infoList)
        {
            yield return new WaitForFixedUpdate();
            yield return StartCoroutine(base.ClearContainerAnimation());
            yield return StartCoroutine(base.BeforeAddAllToContainerAnimation(infoList));
        }

        public override IEnumerator BeforeUpdateContainerAnimation(UIContainerInfo containerInfo)
        {
            yield return StartCoroutine(base.BeforeUpdateContainerAnimation(containerInfo));
            var info = containerInfo as UIMiniatureContainerInfo;
            var contentList = GetUIContentChildren();
            if (contentList.Count > 0)
            {
                if (!info.OldSelection || _highlighted == null)
                    _highlighted = (contentList[0].Info as UISceneIntegratedContentInfo).Selectable as UISceneIntegratedSelectable;
                else if (info.NextHighlight)
                    _highlighted = GetNextHighlight();
                else
                    _highlighted = GetPreviousHighlight();
            }
            else
            {
                _highlighted = null;
            }
        }

        public override UIContent AddToContainer(UIContentInfo info)
        {
            var selectable = GetSelectable(info);
            if (!selection.Contains(selectable))
            {
                selection.Add(selectable);
                UIContent uIContent = base.AddToContainer(info);
                return uIContent;
            }
            return null;
        }

        public override void UpdateContainer(UIContainerInfo containerInfo)
        {
            UpdateMiniaturesHighlight(containerInfo as UIMiniatureContainerInfo);
            base.UpdateContainer(containerInfo);
        }

        public override IEnumerator AfterAddAllToContainerAnimation(List<UIContentInfo> infoList)
        {
            SortMiniaturePanel();
            yield return StartCoroutine(base.AfterAddAllToContainerAnimation(infoList));
            yield return StartCoroutine(base.UpdateContainerAnimation(new UIMiniatureContainerInfo() { ContainerId = container.ContainerId }));
            yield return new WaitForFixedUpdate();
        }

        public override IEnumerator AfterRemoveAllFromContainerAnimation(List<UIContent> contentList)
        {
            yield return StartCoroutine(base.AfterRemoveAllFromContainerAnimation(contentList));
            contentList.ForEach(x => selection.Remove(GetSelectable(x.Info)));
            yield return StartCoroutine(base.UpdateContainerAnimation(new UIMiniatureContainerInfo() { ContainerId = container.ContainerId }));
        }

        public override IEnumerator AfterRemoveFromContainerAnimation(UIContent content)
        {
            yield return StartCoroutine(base.AfterRemoveFromContainerAnimation(content));
            selection.Remove(GetSelectable(content.Info));
            yield return StartCoroutine(base.UpdateContainerAnimation(new UIMiniatureContainerInfo() { ContainerId = container.ContainerId }));
        }

        public override IEnumerator AfterClearContainerAnimation(List<UIContent> contentList)
        {
            yield return StartCoroutine(base.AfterClearContainerAnimation(contentList));
            contentList.ForEach(x => selection.Remove(GetSelectable(x.Info)));
            yield return StartCoroutine(base.UpdateContainerAnimation(new UIMiniatureContainerInfo() { ContainerId = container.ContainerId }));
        }

        public override IEnumerator AfterUpdateContainerAnimation(UIContainerInfo containerInfo)
        {
            yield return StartCoroutine(base.AfterUpdateContainerAnimation(containerInfo));
            signalBus.Fire(new UIGlobalContainerSignal() { Content = new UIUpdateHighlightSignalContent() { Highlighted = _highlighted } });
        }

        private UISceneIntegratedSelectable GetPreviousHighlight()
        {
            var result = selection.Last();
            int index = selection.IndexOf(_highlighted);
            if (index > 0)
            {
                result = selection[index - 1];
            }
            return selection.Find(x => AreSameType(result, x));
        }

        private UISceneIntegratedSelectable GetNextHighlight()
        {
            var result = selection[0];
            int index = selection.IndexOf(_highlighted);
            if (index < selection.Count - 1)
            {
                var next = selection.GetRange(index, selection.Count - index).Find(x => !AreSameType(_highlighted, x));
                if (next != null)
                {
                    result = next;
                }
            }
            return result;
        }

        private void UpdateMiniaturesHighlight(UIMiniatureContainerInfo info)
        {
            foreach (var item in GetUIContentChildren())
            {
                UISceneIntegratedSelectable selectable = GetSelectable(item.Info);
                selectable.IsHighlighted = AreSameType(_highlighted, selectable);
            }
        }



        private bool AreSameType(UISceneIntegratedSelectable first, UISceneIntegratedSelectable second)
        {
            return first.Type == second.Type;
        }

        private UISceneIntegratedSelectable GetSelectable(UIContentInfo info)
        {
            return (info as UISceneIntegratedContentInfo).Selectable as UISceneIntegratedSelectable;
        }

        //TODO move to selection manager
        private void SortMiniaturePanel()
        {
            List<UIContent> contents = GetUIContentChildren();
            contents.Sort(new UIMiniatureComparer());

            List<UISceneIntegratedSelectable> sortedSelection = new List<UISceneIntegratedSelectable>();
            for (var i = 0; i < contents.Count; i++)
            {
                contents[i].transform.SetSiblingIndex(i);
                sortedSelection.Add(GetSelectable(contents[i].Info));
            }
            selection = sortedSelection;
        }

        public override List<UIContent> GetUIContentChildren()
        {
            var contentList = base.GetUIContentChildren();
            contentList.RemoveAll(x => x.Info.IsBeeingRemoved);
            return contentList;
        }
    }

    internal class UIMiniatureComparer : IComparer<UIContent>
    {
        public int Compare(UIContent x, UIContent y)
        {
            var xKey = ((x.Info as UISceneIntegratedContentInfo).Selectable as UISceneIntegratedSelectable).Type;
            var yKey = ((y.Info as UISceneIntegratedContentInfo).Selectable as UISceneIntegratedSelectable).Type;
            return xKey - yKey;
        }
    }
}