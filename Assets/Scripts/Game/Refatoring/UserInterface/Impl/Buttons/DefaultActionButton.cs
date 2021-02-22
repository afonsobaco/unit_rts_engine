using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class DefaultActionButton : DefaultClickableButton
    {
        public override void Clicked()
        {
            Debug.Log("Action");
        }
    }
}
