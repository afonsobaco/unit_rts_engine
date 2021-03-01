using UnityEngine.UI;
using RTSEngine.Signal;
using RTSEngine.Refactoring;

public class PortraitButton : DefaultPortraitButton
{

    public override void UpdateApperance()
    {
        if (ObjectReference is GameSelectable)
        {
            this.GetComponentInChildren<Text>().text = (ObjectReference as GameSelectable).Type + " - " + (ObjectReference as GameSelectable).Index.ToString();
        }
    }

    public override void DoClick()
    {
        if (ObjectReference is GameSelectable)
            SignalBus.Fire(new PortraitClickedSignal() { Selection = ObjectReference as GameSelectable });
    }
}