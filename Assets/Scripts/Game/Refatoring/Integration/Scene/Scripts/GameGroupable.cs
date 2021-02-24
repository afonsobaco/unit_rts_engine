using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core;
using RTSEngine.Refactoring;
using Zenject;

namespace RTSEngine.Refactoring
{

    public class GameGroupable : MonoBehaviour, IGroupable
    {
        public string objectType;

        public bool IsCompatible(object other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }
            return this.objectType.Equals((other as GameGroupable).objectType);
        }

    }

}