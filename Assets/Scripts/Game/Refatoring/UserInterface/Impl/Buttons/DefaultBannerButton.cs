using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class DefaultBannerButton : DefaultClickableButton
    {
        public override void DoClick()
        {
            Debug.Log("Banner");
        }

        public override void DoPress()
        {
            Debug.Log("Banner Press");
        }
    }
}
