using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using RTSEngine.RTSUserInterface;
using RTSEngine.Signal;

namespace RTSEngine.Integration.Scene
{
    public class UIPartyContent : UIContent, IPointerClickHandler
    {
        [SerializeField] private Text key;

        public override void UpdateAppearance()
        {
            key.text = (Info as UIPartyContentInfo).Key.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SignalBus.Fire(new ChangeSelectionSignal() { Selection = (Info as UIPartyContentInfo).Selection });
        }
    }
}