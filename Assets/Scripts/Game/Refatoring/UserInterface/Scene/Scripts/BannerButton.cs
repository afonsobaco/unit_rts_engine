using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Refactoring;
using Zenject;

public class BannerButton : DefaultBannerButton
{
    public override void UpdateApperance()
    {
        if (ObjectReference is int)
            this.GetComponentInChildren<Text>().text = (ObjectReference as int?).ToString();
    }

    public override void DoClick()
    {
        SignalBus.Fire(new BannerClickedSignal()
        {
            PartyId = (ObjectReference as int?),
        });
    }
}

