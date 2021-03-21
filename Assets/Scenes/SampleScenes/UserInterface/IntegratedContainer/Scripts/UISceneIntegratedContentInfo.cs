using UnityEngine;
using RTSEngine.Core;

namespace RTSEngine.RTSUserInterface.Scene
{
    public class UISceneIntegratedContentInfo : UIContentInfo
    {
        private ISelectable _selectable;
        public ISelectable Selectable { get => _selectable; set => _selectable = value; }
    }
}