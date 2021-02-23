using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using Zenject;

namespace RTSEngine.Refactoring
{

    public class GameGroupableObject : GameDefaultObject, IGroupable
    {
        public bool IsCompatible(object other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }
            return this.objectType.Equals((other as GameGroupableObject).objectType);
        }

    }

}