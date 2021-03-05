using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTSEngine.Signal;
using System;

namespace RTSEngine.Refactoring
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
