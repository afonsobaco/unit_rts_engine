using System.Collections;
using System.Collections.Generic;
using RTSEngine.Core;
using UnityEngine;

namespace RTSEngine.Refactoring.Scene.UInterface
{
    public class SceneUserInterfaceObject : ISelectable
    {
        public int Index { get; set; }
        public bool IsSelected { get; set; }
        public bool IsPreSelected { get; set; }
        public Vector3 Position { get; set; }
        public bool IsHighlighted { get; set; }
        public string Type { get; set; }
    }

}
