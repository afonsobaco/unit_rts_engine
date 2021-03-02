using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Signal;
using RTSEngine.Refactoring;
using Zenject;
namespace RTSEngine.Refactoring.Scene.UInterface
{
    public class SceneUserInterfaceBannerButton : DefaultBannerButton
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
}

