using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class DefaultPortraitButton : DefaultClickableButton
    {
        public override void DoClick()
        {
            Debug.Log("Portrait");
        }

        public override void DoPress()
        {
            Debug.Log("Portrait Press");
        }

    }
}
