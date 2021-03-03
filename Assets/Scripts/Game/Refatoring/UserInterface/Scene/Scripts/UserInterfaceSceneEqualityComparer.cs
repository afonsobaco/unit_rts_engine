using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Commons;
using RTSEngine.Core;

namespace RTSEngine.Refactoring.Scene.UInterface
{
    public class UserInterfaceSceneEqualityComparer : EqualityComparerComponent
    {
        public override bool Equals(ISelectable x, ISelectable y)
        {
            if (x is UserInterfaceSceneObject && y is UserInterfaceSceneObject)
            {
                var first = x as UserInterfaceSceneObject;
                var second = y as UserInterfaceSceneObject;
                return first.Type == second.Type;
            }
            return x.Equals(y);
        }

        public override int GetHashCode(ISelectable obj)
        {
            if (obj is UserInterfaceSceneObject)
            {
                var first = obj as UserInterfaceSceneObject;
                int hCode = first.Type.GetHashCode();
                return hCode;
            }
            return obj.GetHashCode();
        }
    }
}
