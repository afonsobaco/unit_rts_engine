using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Utils;
using Zenject;

namespace RTSEngine.Refactoring
{
    public class DefaultMiniatureButton : DefaultClickableButton
    {
        public override void Clicked()
        {
            Debug.Log("Miniature");
        }

        public class Factory : PlaceholderFactory<DefaultMiniatureButton>
        {
        }
    }
}
