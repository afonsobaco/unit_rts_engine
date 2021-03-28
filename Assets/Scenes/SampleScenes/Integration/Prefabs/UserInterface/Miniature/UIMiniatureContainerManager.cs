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
        [Inject] private IEqualityComparer<ISelectable> _equalityComparer;

        private IntegrationSceneObject _highlighted;
        private List<IntegrationSceneObject> selection = new List<IntegrationSceneObject>();
        public IntegrationSceneObject Highlighted { get => _highlighted; set => _highlighted = value; }

        private void Start()
        {
            signalBus.Subscribe<SelectionUpdateSignal>(SelectionUpdated);
        }

        private void SelectionUpdated(SelectionUpdateSignal signal)
        {
            List<UIContentInfo> infoList = GetSelection(signal.Selection);
            UIContainerInfo miniatureContainerInfo = new UIMiniatureContainerInfo() { ContainerId = "MiniatureContainer" };
            signalBus.Fire(new UIAddAllContentSignal() { ContainerInfo = miniatureContainerInfo, InfoList = infoList });
        }

        private List<UIContentInfo> GetSelection(ISelectable[] selection)
        {
            List<UIContentInfo> result = new List<UIContentInfo>();
            for (var i = 0; i < selection.Length; i++)
            {
                var info = new UISceneIntegratedContentInfo();
                info.Selectable = selection[i];
                result.Add(info);
            }
            return result;
        }

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
                    _highlighted = (contentList[0].Info as UISceneIntegratedContentInfo).Selectable as IntegrationSceneObject;
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

        private IntegrationSceneObject GetPreviousHighlight()
        {
            var result = selection.Last();
            int index = selection.IndexOf(_highlighted);
            if (index > 0)
            {
                result = selection[index - 1];
            }
            return selection.Find(x => AreSameType(result, x));
        }

        private IntegrationSceneObject GetNextHighlight()
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
                IntegrationSceneObject selectable = GetSelectable(item.Info);
                selectable.IsHighlighted = AreSameType(_highlighted, selectable);
            }
        }

        private IntegrationSceneObject GetSelectable(UIContentInfo info)
        {
            return (info as UISceneIntegratedContentInfo).Selectable as IntegrationSceneObject;
        }

        public override List<UIContent> GetUIContentChildren()
        {
            var contentList = base.GetUIContentChildren();
            contentList.RemoveAll(x => x.Info.IsBeeingRemoved);
            return contentList;
        }

        private bool AreSameType(IntegrationSceneObject result, IntegrationSceneObject x)
        {
            return _equalityComparer.Equals(result, x);
        }
    }
}