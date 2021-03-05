using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Signal;
using System;

namespace RTSEngine.Refactoring
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
