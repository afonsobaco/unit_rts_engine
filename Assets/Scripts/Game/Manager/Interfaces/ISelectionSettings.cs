using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core.Abstracts;

namespace RTSEngine.Manager.Interfaces
{
    public interface ISelectionSettings<T, E>
    {
        AbstractMainList<T> MainList { get; set; }
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