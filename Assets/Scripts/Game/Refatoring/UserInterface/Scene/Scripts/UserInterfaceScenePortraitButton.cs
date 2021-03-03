using UnityEngine.UI;
using RTSEngine.Signal;
using RTSEngine.Refactoring;

namespace RTSEngine.Refactoring.Scene.UInterface
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