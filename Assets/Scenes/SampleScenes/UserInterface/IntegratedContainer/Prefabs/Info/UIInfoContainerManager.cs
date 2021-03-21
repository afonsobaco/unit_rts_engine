using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using RTSEngine.Utils;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UIInfoContainerManager : UILimitedContainerManager
    {

        [Inject] private SignalBus _signalBus;

        private Queue<UIInfoContentInfo> _infoQueue = new Queue<UIInfoContentInfo>();

        public override UIContent AddToContainer(UIContentInfo info)
        {
            if (GetUIContentChildren().Count == _limit)
            {
                _infoQueue.Enqueue(info as UIInfoContentInfo);
            }
            else
            {
                return base.AddToContainer(info);
            }
            return null;
        }

        public override IEnumerator AfterAddToContainerAnimation(UIContentInfo contentInfo)
        {
            if (contentInfo.Content)
            {
                var effect = contentInfo.Content.GetComponentInChildren<UIEffectBase>();
                if (effect)
                {
                    yield return StartCoroutine(effect.Create());
                }
            }
            yield return null;
        }

        public override IEnumerator BeforeRemoveFromContainerAnimation(UIContent content)
        {
            var effect = content.GetComponentInChildren<UIEffectBase>();
            var waitTime = 1f;
            if (effect)
            {
                waitTime = effect.DestroyAnimationTime;
                yield return StartCoroutine(effect.Destroy());
            }
            else
            {
                yield return null;
            }
            StartCoroutine(AddInfoFromQueue(waitTime));
        }

        private IEnumerator AddInfoFromQueue(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            if (_infoQueue.Count > 0)
            {
                var info = _infoQueue.Dequeue() as UIInfoContentInfo;
                _signalBus.Fire(new UIAddContentSignal { ContainerInfo = new UIContainerInfo() { ContainerId = container.ContainerId }, Info = info });
            }
        }

    }

}