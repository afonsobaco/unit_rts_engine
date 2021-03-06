using UnityEngine.UI;
using RTSEngine.Signal;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class IntegrationSceneBannerButton : DefaultBannerButton
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
