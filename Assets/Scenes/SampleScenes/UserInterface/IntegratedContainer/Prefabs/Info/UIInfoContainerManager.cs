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

        [Inject] private GameSignalBus _signalBus;

        private Queue<UIInfoContentInfo> _infoQueue = new Queue<UIInfoContentInfo>();

        public override UIContent AddToContainer(UIAddContentSignal signal)
        {
            if (GetUIContentChildren().Count == _limit)
            {
                _infoQueue.Enqueue(signal.Info as UIInfoContentInfo);
            }
            else
            {
                return base.AddToContainer(signal);
            }
            return null;
        }

        public override IEnumerator PosAddToContainer(UIContent content)
        {
            var effect = content.GetComponentInChildren<UIEffectBase>();
            if (effect)
            {
                yield return StartCoroutine(effect.Create());
            }
            else
            {
                yield return null;
            }
        }

        public override IEnumerator BeforeRemoveFromContainer(UIContent content)
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
            Destroy(content.gameObject);
            StartCoroutine(AddInfoFromQueue(waitTime));
        }

        private IEnumerator AddInfoFromQueue(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            if (_infoQueue.Count > 0)
            {
                var info = _infoQueue.Dequeue() as UIInfoContentInfo;
                _signalBus.Fire(new UIAddContentSignal { Info = info });
            }
        }

    }

}