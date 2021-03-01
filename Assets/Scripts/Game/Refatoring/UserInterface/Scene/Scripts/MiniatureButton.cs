using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using RTSEngine.Signal;
using Zenject;

public class MiniatureButton : DefaultMiniatureButton
{
    public override void UpdateApperance()
    {
        if (ObjectReference is GameSelectable)
        {
            if ((ObjectReference as GameSelectable).IsHighlighted)
            {
                this.GetComponent<Image>().color = new Color(22, 255, 0);
            }
            else
            {
                this.GetComponent<Image>().color = new Color(255, 255, 255);
            }
            this.GetComponentInChildren<Text>().text = (ObjectReference as GameSelectable).Type + " - " + (ObjectReference as GameSelectable).Index.ToString();
        }
    }

    public override void DoClick()
    {
        if (ObjectReference is GameSelectable)
        {
            SignalBus.Fire(new MiniatureClickedSignal()
            {
                Selected = ObjectReference as GameSelectable
            });
        }
    }
}

