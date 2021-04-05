using System.Linq;
using System.Collections;
using System.Collections.Generic;
using RTSEngine.RTSUserInterface;
using RTSEngine.Signal;
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

        private void Start()
        {
            miniatureHighlightManager = new UIMiniatureHighlightManager(_equalityComparer);
            signalBus.Subscribe<SelectionUpdateSignal>(SelectionUpdated);
            signalBus.Subscribe<PartyUpdateSignal>(UpdatePartySignal);
        }

        private void SelectionUpdated(SelectionUpdateSignal signal)
        {
            if (!signal.TransformSelection)
                miniatureHighlightManager.Highlighted = null;
            StartCoroutine(AdjustContainerForNewSelection(UIUtils.CreateContentInfoListBySelectionList(signal.Selection)));
        }

        private void UpdatePartySignal(PartyUpdateSignal signal)
        {
            signalBus.Fire(new UIUpdatePartySignal() { PartyId = (int)signal.PartyId, Selection = selection.ToArray() });
        }

        private IEnumerator AdjustContainerForNewSelection(List<UIContentInfo> infoList)
        {
            List<UIContent> children = GetUIContentChildren();
            List<UIContent> ToBeRemoved = GetToBeRemoved(infoList, children);
            List<UIContentInfo> ToBeAdded = GetToBeAdded(infoList, children);
            if (ToBeRemoved.Count > 0) yield return StartCoroutine(RemoveAllFromContainerRoutine(ToBeRemoved, false, false));
            if (ToBeAdded.Count > 0) yield return StartCoroutine(AddAllToContainerRoutine(ToBeAdded, false, false));
            SortFinalInfoList(infoList);
            yield return StartCoroutine(AfterAny());
        }

        private List<UIContent> GetToBeRemoved(List<UIContentInfo> infoList, List<UIContent> children)
        {
            var selectables = UIUtils.GetSelectableListFromContentInfoList(infoList);
            return children.Where(x => !selectables.Contains(UIUtils.GetSelectable(x.Info))).ToList();
        }

        private List<UIContentInfo> GetToBeAdded(List<UIContentInfo> infoList, List<UIContent> children)
        {
            List<IntegrationSceneObject> childrenSelectables = UIUtils.GetSelectableListFromContentList(children);
            return infoList.Where(x => !childrenSelectables.Contains(UIUtils.GetSelectable(x))).ToList();
        }

        private void SortFinalInfoList(List<UIContentInfo> infoList)
        {
            infoList.Where(x => x.Content).ToList().ForEach(x => x.Content.transform.SetSiblingIndex(infoList.IndexOf(x)));
        }

        public override void UpdateContainer(UIContainerInfo containerInfo)
        {
            List<UIContent> contentList = GetUIContentChildren();
            miniatureHighlightManager.UpdateHighlighted(containerInfo, contentList);
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

        public override void RemoveAllFromContainer(List<UIContent> contentList)
        {
            contentList.ForEach(x => selection.Remove(UIUtils.GetSelectable(x.Info)));
        }

        public override void RemoveFromContainer(UIContent content)
        {
            selection.Remove(UIUtils.GetSelectable(content.Info));
        }

        public override void ClearContainer(List<UIContent> contentList)
        {
            contentList.ForEach(x => selection.Remove(UIUtils.GetSelectable(x.Info)));
        }

        public override IEnumerator AfterAny()
        {
            miniatureHighlightManager.UpdateMiniaturesHighlight(UIUtils.GetSelectableListFromContentList(GetUIContentChildren()));
            UIUpdateHighlightSignalContent contentSignal = new UIUpdateHighlightSignalContent() { Highlighted = miniatureHighlightManager.Highlighted };
            signalBus.Fire(new UIGlobalContainerSignal() { Content = contentSignal });
            yield return StartCoroutine(base.AfterAny());
        }

        public override List<UIContent> GetUIContentChildren()
        {
            var contentList = base.GetUIContentChildren();
            contentList.RemoveAll(x => x.Info.IsBeeingRemoved);
            return contentList;
        }
    }
}