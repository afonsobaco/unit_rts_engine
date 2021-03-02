using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using RTSEngine.Signal;
using Zenject;

namespace RTSEngine.Refactoring.Scene.UInterface
{
    public class SceneUserInterfaceMiniatureButton : DefaultMiniatureButton
    {
        public override void UpdateApperance()
        {
            if (ObjectReference is SceneUserInterfaceObject)
            {
                if ((ObjectReference as SceneUserInterfaceObject).IsHighlighted)
                {
                    this.GetComponent<Image>().color = new Color(22, 255, 0);
                }
                else
                {
                    this.GetComponent<Image>().color = new Color(255, 255, 255);
                }
                this.GetComponentInChildren<Text>().text = (ObjectReference as SceneUserInterfaceObject).Type + " - " + (ObjectReference as SceneUserInterfaceObject).Index.ToString();
            }
        }

        public override void DoClick()
        {
            if (ObjectReference is SceneUserInterfaceObject)
            {
                SignalBus.Fire(new MiniatureClickedSignal()
                {
                    Selected = ObjectReference as SceneUserInterfaceObject
                });
            }
        }
    }
}