using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Core
{
    [CreateAssetMenu(fileName = "GameRuntimeSet", menuName = "ScriptableObjects/Game Runtime Set", order = 1)]
    public class GameRuntimeSet : RuntimeSet<SelectableObject>
    {
    }
}
