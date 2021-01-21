using System.Collections.Generic;
using UnityEngine;
using RTSEngine.Core.Abstracts;

namespace RTSEngine.Manager.Interfaces
{
    public interface ISelectionSettings<T, S, O>
    {
        AbstractMainList<T> MainList { get; set; }
        Vector2 InitialGameScreenPos { get; set; }
        Vector2 FinalGameScreenPos { get; set; }
        int SelectionLimit { get; set; }

        List<S> CanSelectSameType { get; set; }
        List<S> PrimaryTypes { get; set; }
        List<S> SecondaryOrderedTypes { get; set; }
        List<S> CanGroupTypes { get; set; }
        List<IAbstractSelectionMod<T, S, O>> Mods { get; set; }
    }
}