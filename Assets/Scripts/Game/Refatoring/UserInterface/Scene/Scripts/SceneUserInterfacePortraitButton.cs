using UnityEngine.UI;
using RTSEngine.Signal;
using RTSEngine.Refactoring;

namespace RTSEngine.Refactoring.Scene.UInterface
{
    public class SceneUserInterfacePortraitButton : DefaultPortraitButton
    {
        public override void UpdateApperance()
        {
            if (ObjectReference is SceneUserInterfaceObject)
            {
                this.GetComponentInChildren<Text>().text = (ObjectReference as SceneUserInterfaceObject).Type + " - " + (ObjectReference as SceneUserInterfaceObject).Index.ToString();
            }
        }

        public override void DoClick()
        {
            if (ObjectReference is SceneUserInterfaceObject)
                SignalBus.Fire(new PortraitClickedSignal() { Selected = ObjectReference as SceneUserInterfaceObject });
        }
    }
}