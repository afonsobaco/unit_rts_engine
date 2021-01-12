using System.Collections;
using System.Collections.Generic;
using RTSEngine.Core;
using RTSEngine.Selection.Mod;
using UnityEngine;

namespace RTSEngine.Selection
{
    public interface ISelectionSettings<T, E>
    {
        MainList<T> MainList { get; set; }
        Vector2 InitialGameScreenPos { get; set; }
        Vector2 FinalGameScreenPos { get; set; }
        int SelectionLimit { get; set; }

        List<E> CanSelectSameType { get; set; }
        List<E> PrimaryTypes { get; set; }
        List<E> SecondaryOrderedTypes { get; set; }
        List<E> CanGroupTypes { get; set; }
        List<IAbstractSelectionMod<T, E>> Mods { get; set; }
    }
}