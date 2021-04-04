using System.Collections.Generic;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class UIPortraitContainerManager : UILimitedContainerManager
    {
        public override void GlobalContainerSignal(UIGlobalContainerSignal signal)
        {
            if (signal.Content is UIUpdateHighlightSignalContent)
            {
                var content = signal.Content as UIUpdateHighlightSignalContent;
                if (content.Highlighted != null)
                    UpdateOrAddNew(content);
                else
                    StartCoroutine(base.ClearContainerRoutine());
            }
        }

        private void UpdateOrAddNew(UIUpdateHighlightSignalContent content)
        {
            List<UIContent> contentList = GetUIContentChildren();
            if (contentList.Count > 0)
            {
                (contentList[0].Info as UISceneIntegratedContentInfo).Selectable = content.Highlighted;
                StartCoroutine(base.AfterAny());
            }
            else
            {
                UISceneIntegratedContentInfo info = new UISceneIntegratedContentInfo() { Selectable = content.Highlighted };
                StartCoroutine(base.AddToContainerRoutine(info, true, true));
            }
        }

        public override UIContent AddToContainer(UIContentInfo info)
        {
            return base.AddToContainer(info);
        }

    }

}