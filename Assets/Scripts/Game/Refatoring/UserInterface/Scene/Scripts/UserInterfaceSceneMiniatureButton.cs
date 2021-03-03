using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using RTSEngine.Signal;
using Zenject;

namespace RTSEngine.Refactoring.Scene.UInterface
{
    public class UserInterfaceSceneMiniatureButton : DefaultMiniatureButton
    {
        public override void UpdateApperance()
        {
            if (ObjectReference is UserInterfaceSceneObject)
            {
                if ((ObjectReference as UserInterfaceSceneObject).IsHighlighted)
                {
                    this.GetComponent<Image>().color = new Color(22, 255, 0);
                }
                else
                {
                    this.GetComponent<Image>().color = new Color(255, 255, 255);
                }
                this.GetComponentInChildren<Text>().text = (ObjectReference as UserInterfaceSceneObject).Type + " - " + (ObjectReference as UserInterfaceSceneObject).Index.ToString();
            }
        }

        public override void DoClick()
        {
            if (ObjectReference is UserInterfaceSceneObject)
            {
                SignalBus.Fire(new MiniatureClickedSignal()
                {
                    Selected = ObjectReference as UserInterfaceSceneObject
                });
            }
        }
    }
}