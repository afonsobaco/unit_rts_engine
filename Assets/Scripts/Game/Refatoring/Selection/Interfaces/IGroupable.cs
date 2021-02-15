using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Refactoring
{
    public interface IGroupable
    {
        bool IsCompatible(object other);
    }
}
