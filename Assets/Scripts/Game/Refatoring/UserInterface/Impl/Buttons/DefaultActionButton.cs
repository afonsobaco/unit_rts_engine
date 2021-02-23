using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class DefaultActionButton : DefaultClickableButton
    {
        public override void DoClick()
        {
            Debug.Log("Action");
        }

        public override void DoPress()
        {
            Debug.Log("Action Press");
        }
    }
}
