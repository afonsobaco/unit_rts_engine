using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using RTSEngine.Utils;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class UIPortraitContainerManager : UILimitedContainerManager
    {
        public override void GlobalContainerSignal(UIGlobalContainerSignal signal)
        {
            if (signal.Content is UIUpdateHighlightSignalContent)
            {
                UIUpdateHighlightSignalContent highlightedSignal = (signal.Content as UIUpdateHighlightSignalContent);
                if (highlightedSignal.Highlighted != null)
                {
                    UISceneIntegratedContentInfo contentInfo = new UISceneIntegratedContentInfo() { Selectable = highlightedSignal.Highlighted };
                    StartCoroutine(base.AddToContainerAnimation(contentInfo));
                }
                else
                {
                    StartCoroutine(base.ClearContainerAnimation());
                }
            }
        }

        public override IEnumerator BeforeAddToContainerAnimation(UIContentInfo contentInfo)
        {
            yield return new WaitForFixedUpdate();
            yield return StartCoroutine(base.BeforeAddToContainerAnimation(contentInfo));
        }

        public override IEnumerator AfterAddToContainerAnimation(UIContentInfo info)
        {
            yield return StartCoroutine(base.AfterAddToContainerAnimation(info));
            yield return new WaitForFixedUpdate();
        }
    }

}