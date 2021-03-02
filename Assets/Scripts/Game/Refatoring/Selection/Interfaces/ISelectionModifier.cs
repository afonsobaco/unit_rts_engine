using System;
using RTSEngine.Core;

namespace RTSEngine.Refactoring
{
    public interface ISelectionModifier
    {
        SelectionType[] RestrictedTypes { get; set; }
        ISelectable[] Apply(SelectionInfo info);
    }

}