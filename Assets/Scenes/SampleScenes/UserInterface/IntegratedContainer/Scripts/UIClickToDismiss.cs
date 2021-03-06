using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using RTSEngine.Utils;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UIClickToDismiss : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private UIContent content;
        [Inject] private SignalBus _signalBus;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (content)
                _signalBus.Fire(new UIRemoveContentSignal() { ContainerInfo = new UIContainerInfo() { ContainerId = content.Container.ContainerId }, Content = content });
            else
                Debug.LogError(string.Format("There is no content on {0}, on object {1}", this.GetType().Name, this.gameObject.name));
        }
    }
}

