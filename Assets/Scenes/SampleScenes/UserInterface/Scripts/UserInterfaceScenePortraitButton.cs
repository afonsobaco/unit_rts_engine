using UnityEngine.UI;
using RTSEngine.Signal;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UserInterfaceScenePortraitButton : DefaultPortraitButton
    {
        public override void UpdateApperance()
        {
            if (ObjectReference is UserInterfaceSceneObject)
            {
                this.GetComponentInChildren<Text>().text = (ObjectReference as UserInterfaceSceneObject).Type + " - " + (ObjectReference as UserInterfaceSceneObject).Index.ToString();
            }
        }

        public override void DoClick()
        {
            if (ObjectReference is UserInterfaceSceneObject)
                SignalBus.Fire(new PortraitClickedSignal() { Selected = ObjectReference as UserInterfaceSceneObject });
        }
    }
}