using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Commons;
using RTSEngine.Core;

namespace RTSEngine.Refactoring.Scene.UInterface
{
    public class SceneUserInterfaceEqualityComparer : EqualityComparerComponent
    {
        public override bool Equals(ISelectable x, ISelectable y)
        {
            if (x is SceneUserInterfaceObject && y is SceneUserInterfaceObject)
            {
                var first = x as SceneUserInterfaceObject;
                var second = y as SceneUserInterfaceObject;
                return first.Type == second.Type;
            }
            return x.Equals(y);
        }

        public override int GetHashCode(ISelectable obj)
        {
            if (obj is SceneUserInterfaceObject)
            {
                var first = obj as SceneUserInterfaceObject;
                int hCode = first.Type.GetHashCode();
                return hCode;
            }
            return obj.GetHashCode();
        }
    }
}
