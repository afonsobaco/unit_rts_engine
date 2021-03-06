using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using Zenject;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceSceneBannerButton : DefaultBannerButton
    {
        public override void UpdateApperance()
        {
            if (ObjectReference is int)
                this.GetComponentInChildren<Text>().text = (ObjectReference as int?).ToString();
        }

        public override void DoClick()
        {
            SignalBus.Fire(new PartySelectedSignal()
            {
                PartyId = (ObjectReference as int?),
            });
        }
    }
}

