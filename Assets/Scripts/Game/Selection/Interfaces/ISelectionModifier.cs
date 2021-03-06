using System;
using RTSEngine.Core;

namespace RTSEngine.RTSSelection
{
    public interface ISelectionModifier
    {
        SelectionType[] RestrictedTypes { get; set; }
        ISelectable[] Apply(SelectionInfo info);
    }

}