using UnityEngine.UI;
using RTSEngine.Signal;
using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class IntegrationScenePortraitButton : DefaultPortraitButton
    {
        public override void UpdateApperance()
        {
            if (ObjectReference is IntegrationSceneObject)
            {
                var obj = ObjectReference as IntegrationSceneObject;
                this.GetComponentInChildren<Image>().sprite = obj.Picture;
            }
        }

        public override void DoPress()
        {
            if (ObjectReference is IntegrationSceneObject)
                SignalBus.Fire(new PortraitClickedSignal() { Selected = ObjectReference as IntegrationSceneObject });
        }
    }
}
